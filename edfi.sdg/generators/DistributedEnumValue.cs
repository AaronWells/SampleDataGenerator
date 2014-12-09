namespace edfi.sdg.generators
{
    using System;

    [System.SerializableAttribute()]
    public class DistributedEnumValue<T>
    {
        public T Value { get; set; }
        public Double Weight { get; set; }
    }
}