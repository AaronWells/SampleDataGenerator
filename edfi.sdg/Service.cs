namespace edfi.sdg
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using edfi.sdg.configurations;
    using edfi.sdg.interfaces;
    using edfi.sdg.messaging;
    using edfi.sdg.models;
    using edfi.sdg.utility;

    public class Service
    {
        private readonly CancellationTokenSource tokenSource;
        private readonly Configuration configuration;
        private readonly ConcurrentBag<Task> tasks;

        public Service()
        {
            configuration = Configuration.DefaultConfiguration;
            tasks = new ConcurrentBag<Task>();
            tokenSource = new CancellationTokenSource();
        }

        public void Bootstrap()
        {
            var workQueue = new WorkQueue(configuration.WorkQueueName);
            var generator = configuration.Generators.First();
            var workItems = generator.Generate(workQueue, configuration);
            EnqueueWorkItems(workItems, generator);
        }

        public void Start()
        {
            for (var i = 0; i < configuration.NumThreads; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => this.DoWorkAsync(tokenSource.Token), tokenSource.Token));
            }
        }

        private void EnqueueWorkItems(IEnumerable<object> workItems, IGenerator generator)
        {
            Parallel.ForEach(
                workItems,
                item =>
                {
                    var workQueue = new WorkQueue(configuration.WorkQueueName);
                    var objectRepository = new ComplexObjectRepository();
                    if (item is IGenerator)
                    {
                        workQueue.WriteObject(item);
                    }
                    else
                    {
                        var envelope = new WorkEnvelope { NextStep = generator.Id + 1, Model = item };
                        objectRepository.Save(item as ComplexObjectType);
                        workQueue.WriteObject(envelope);
                    }
                });
        }

        private async void DoWorkAsync(CancellationToken token)
        {
            var workQueue = new WorkQueue(configuration.WorkQueueName);

            while (!token.IsCancellationRequested)
            {
                var workItem = await workQueue.ReadObjectAsync();

                var generator = workItem as IGenerator;
                if (generator != null)
                {
                    var generatedWorkItems = generator.Generate(null, configuration);
                    this.EnqueueWorkItems(generatedWorkItems, generator);
                }
                else
                {
                    var workEnvelope = (WorkEnvelope)workItem;
                    var nextGenerator = configuration.Generators[workEnvelope.NextStep];
                    var generatedWorkItems = nextGenerator.Generate(workEnvelope.Model, configuration);
                    EnqueueWorkItems(generatedWorkItems, nextGenerator);
                }
            }
        }

        public void Stop()
        {
            tokenSource.Cancel();
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
