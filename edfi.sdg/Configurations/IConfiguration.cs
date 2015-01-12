namespace EdFi.SampleDataGenerator.Configurations
{
    public interface IConfiguration
    {
        /// <summary>
        /// The maximum number of messages that a single generator should put on the queue
        /// </summary>
        int MaxQueueWrites { get; set; }

        /// <summary>
        /// The MSMQ address of the worker queue
        /// </summary>
        string WorkQueueName { get; set; }

        ValueRule[] GetValueRules();
    }
}