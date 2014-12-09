namespace edfi.sdg.messaging
{
    [System.SerializableAttribute()]
    public class WorkEnvelope
    {
        public int NextStep { get; set; }
        public object Model { get; set; }
    }
}
