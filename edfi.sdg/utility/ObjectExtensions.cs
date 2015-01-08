using edfi.sdg.generators;

namespace edfi.sdg.utility
{
    public static class ObjectExtensions
    {
        public static object GetValue(this object o, string propertyName)
        {
            var property = o.GetType().GetProperty(propertyName);
            if (property == null) throw new InvalidPropertyException(propertyName);
            return property.GetValue(o);
        }

        public static void SetValue(this object o, string propertyName, object value)
        {
            var property = o.GetType().GetProperty(propertyName);
            if(property == null) throw new InvalidPropertyException(propertyName);
            property.SetValue(o, value);
        }

        public static string FirstSegment(this string propertyName)
        {
            return propertyName.Split(new[] {'.'})[0];
        }

        public static string LastSegment(this string propertyName)
        {
            var splitted = propertyName.Split(new[] {'.'});
            return splitted[splitted.Length - 1];
        }
    }
}