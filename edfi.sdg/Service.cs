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

    public class ServiceParams
    {
        public bool Bootstrap { get; set; }

        public string ConfigFile { get; set; }
    }

    public class Service
    {
        private readonly CancellationTokenSource tokenSource;
        private readonly Configuration configuration;
        private readonly ConcurrentBag<Task> tasks;
        private readonly ServiceParams serviceParams;

        public Service(ServiceParams serviceParams)
        {
            this.serviceParams = serviceParams;
            //todo: load configuration from filename in serviceParams
            configuration = Configuration.DefaultConfiguration;
            tasks = new ConcurrentBag<Task>();
            tokenSource = new CancellationTokenSource();
        }

        public void Bootstrap()
        {
            var workQueue = new WorkQueue(configuration.WorkQueueName);
            var generator = configuration.Generators.First();
            var workItems = generator.Generate(workQueue, configuration);
            EnqueueWorkItems(workItems, generator.Id);
        }

        public void Start()
        {
            if (serviceParams.Bootstrap)
                this.Bootstrap();

            //for (var i = 0; i < configuration.NumThreads; i++)
            //{
                tasks.Add(Task.Factory.StartNew(() => this.DoWorkAsync(tokenSource.Token), tokenSource.Token));
            //}
        }

        private void EnqueueWorkItems(IEnumerable<object> workItems, int generatorId)
        {
            Parallel.ForEach(
                workItems,
                item =>
                {
                    using (var workQueue = new WorkQueue(configuration.WorkQueueName))
                    {
                        var objectRepository = new ComplexObjectRepository();
                        if (item is IGenerator)
                        {
                            workQueue.WriteObject(item);
                        }
                        else
                        {
                            var envelope = new WorkEnvelope { NextStep = generatorId, Model = item };
                            objectRepository.Save(item as ComplexObjectType);
                            workQueue.WriteObject(envelope);
                        }
                    }
                });
        }

        private async void DoWorkAsync(CancellationToken token)
        {
            var workQueue = new WorkQueue(configuration.WorkQueueName);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var workItem = await workQueue.ReadObjectAsync();
                    var generator = workItem as IGenerator;
                    if (generator != null)
                    {
                        var generatedWorkItems = generator.Generate(null, configuration);
                        this.EnqueueWorkItems(generatedWorkItems, generator.Id);
                    }
                    else
                    {
                        var workEnvelope = (WorkEnvelope)workItem;
                        if (workEnvelope.NextStep <= configuration.Generators.GetUpperBound(0))
                        {
                            var nextGenerator = configuration.Generators[workEnvelope.NextStep];
                            var generatedWorkItems = nextGenerator.Generate(workEnvelope.Model, configuration);
                            EnqueueWorkItems(generatedWorkItems, nextGenerator.Id);
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    // the task is cancelled on a timeout, so we'll wait again for a message
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
