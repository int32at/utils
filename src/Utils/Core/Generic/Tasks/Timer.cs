using System;
using System.Diagnostics;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Tasks
{
    public static class Timing
    {
        public static TimeSpan Measure(Action action)
        {
            Stopwatch watch = null;
            try
            {
                watch = Stopwatch.StartNew();
                action();
            }
            finally
            {
                watch.IfNotNull(() => watch.Stop());
            }

            return watch.Elapsed;
        }
    }
}
