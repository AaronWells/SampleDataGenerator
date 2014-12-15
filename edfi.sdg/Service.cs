namespace edfi.sdg
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using edfi.sdg.configurations;
    using edfi.sdg.interfaces;
    using edfi.sdg.messaging;

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
            generator.Generate(null, workQueue, configuration);
        }

        public void Start()
        {
            for (var i = 0; i < configuration.NumThreads; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => this.DoWorkAsync(tokenSource.Token), tokenSource.Token));
            }
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
                    generator.Generate(null, workQueue, configuration);
                    continue;
                }
                // todo: handle work object based on configuration steps
                var workEnvelope = workItem as WorkEnvelope;
                if (workEnvelope != null)
                {
                    //get next generator
                    generator = configuration.Generators[workEnvelope.NextStep];
                    generator.Id = workEnvelope.NextStep + 1;
                    generator.Generate(workEnvelope.Model, workQueue, configuration);
                    continue;
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
