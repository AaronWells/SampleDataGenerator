namespace EdFi.SampleDataGenerator.ValueProvider
{
    public interface IValueProvider
    {
        string[] LookupProperties { get; set; }

        object GetValue(params string[] lookupPropertyValues);
    }
}
