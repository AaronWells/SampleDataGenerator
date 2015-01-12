using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Messaging;
using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.WorkItems;

namespace EdFi.SampleDataGenerator
{

    public class ServiceParams
    {
        public bool Bootstrap { get; set; }

        public string ConfigFile { get; set; }
    }

    public class Service
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly Configuration _configuration;
        private readonly ConcurrentBag<Task> _tasks;
        private readonly ServiceParams _serviceParams;

        public Service(ServiceParams serviceParams)
        {
            _serviceParams = serviceParams;
            //todo: load configuration from filename in serviceParams
            _configuration = Configuration.DefaultConfiguration;
            _tasks = new ConcurrentBag<Task>();
            _tokenSource = new CancellationTokenSource();
        }

        public void Bootstrap()
        {
            var workQueue = new WorkQueue(_configuration.WorkQueueName);
            var generator = _configuration.WorkFlow.First();
            var workItems = generator.DoWork(workQueue, _configuration);
            EnqueueWorkItems(workItems, generator.Id);
        }

        public void Start()
        {
            if (_serviceParams.Bootstrap)
                Bootstrap();

            //for (var i = 0; i < configuration.ThreadCount; i++)
            //{
                _tasks.Add(Task.Factory.StartNew(() => DoWorkAsync(_tokenSource.Token), _tokenSource.Token));
            //}
        }

        private void EnqueueWorkItems(IEnumerable<object> workItems, int generatorId)
        {
            Parallel.ForEach(
                workItems,
                item =>
                {
                    using (var workQueue = new WorkQueue(_configuration.WorkQueueName))
                    {
                        var objectRepository = new ComplexObjectRepository();
                        if (item is IWorkItem)
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
            var workQueue = new WorkQueue(_configuration.WorkQueueName);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var workItem = await workQueue.ReadObjectAsync();
                    var generator = workItem as IWorkItem;
                    if (generator != null)
                    {
                        var generatedWorkItems = generator.DoWork(null, _configuration);
                        EnqueueWorkItems(generatedWorkItems, generator.Id);
                    }
                    else
                    {
                        var workEnvelope = (WorkEnvelope)workItem;
                        if (workEnvelope.NextStep <= _configuration.WorkFlow.GetUpperBound(0))
                        {
                            var nextGenerator = _configuration.WorkFlow[workEnvelope.NextStep];
                            var generatedWorkItems = nextGenerator.DoWork(workEnvelope.Model, _configuration);
                            EnqueueWorkItems(generatedWorkItems, nextGenerator.Id);
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    // the task is cancelled on a timeout, so we'll wait again for a message
                }
            }
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            try
            {
                Task.WaitAll(_tasks.ToArray());
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
