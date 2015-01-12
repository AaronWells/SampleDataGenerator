namespace EdFi.SampleDataGenerator.Messaging
{
    public interface IQueueWriter
    {
        void WriteObject(object obj);
    }
}