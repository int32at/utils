using System;

namespace int32.Utils.Core.Generic.Engine.Exceptions
{
    public class StepFailedException : Exception
    {
        public Step Step { get; set; }
    }
}
