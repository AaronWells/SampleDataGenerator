namespace edfi.sdg.generators
{
    using System;

    [System.SerializableAttribute()]
    public class Weighting<T>
    {
        public T Value { get; set; }
        public Double Weight { get; set; }
    }
}