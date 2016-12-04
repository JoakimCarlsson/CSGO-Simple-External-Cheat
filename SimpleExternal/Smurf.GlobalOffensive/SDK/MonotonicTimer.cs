using System;
using System.Diagnostics;

namespace Smurf.GlobalOffensive.SDK
{
    public static class MonotonicTimer
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public static TimeSpan GetTimeStamp()
        {
            return Stopwatch.Elapsed;
        }
    }
}
