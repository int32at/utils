using System;
using int32.Utils.Core.Exceptions;

namespace int32.Utils.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static SerializableException ToSerializable(this Exception ex)
        {
            return new SerializableException(ex);
        }
    }
}