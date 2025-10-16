using System;

namespace Cocoar.Reflectensions.Exceptions
{
    public class PropertyNotFoundException: Exception
    {
        public PropertyNotFoundException(string message) : base(message)
        {
        }
    }
}
