namespace EdFi.SampleDataGenerator.Messaging
{
    [System.SerializableAttribute]
    public class WorkEnvelope
    {
        public int NextStep { get; set; }
        public object Model { get; set; }
    }
}
