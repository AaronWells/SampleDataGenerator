namespace edfi.sdg.interfaces
{
    public interface IValueProvider
    {
        string[] LookupProperties { get; set; }

        object GetValue(params string[] lookupPropertyValues);
    }
}
