namespace EdFi.SampleDataGenerator.ValueProvider
{
    public class TestValueProvider : ValueProviderBase
    {
        public override object GetValue()
        {
            return "ThisIsATest";
        }

        public override object GetValue(params string[] lookupPropertyValues)
        {
            return "ThisIsATestWithParameter";
        }
    }
}