namespace edfi.sdg.interfaces
{
    public interface IValueProvider
    {
        string[] LookupProperties { get; set; }

        object GetValue();

        object GetValue(params string[] lookupPropertyValues);
    }
}
