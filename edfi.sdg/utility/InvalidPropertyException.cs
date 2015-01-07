using System;

namespace edfi.sdg.utility
{
    public class InvalidPropertyException : Exception
    {
        public InvalidPropertyException(string propertyName)
        {
            _propertyName = propertyName;
        }

        private readonly string _propertyName;

        public override string Message
        {
            get
            {
                return string.Format("Invalid property name '{0}'", _propertyName);
            }
        }
    }
}