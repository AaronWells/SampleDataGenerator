using System;

namespace edfi.sdg.configurations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SerializableGenericAttribute : Attribute
    {
    }

    [AttributeUsage( AttributeTargets.Property | AttributeTargets.GenericParameter)]
    public class SerializableGenericEnumAttribute : Attribute
    {
    }
}
