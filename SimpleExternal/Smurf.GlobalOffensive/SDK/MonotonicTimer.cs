using System;
using System.Diagnostics;

namespace Smurf.GlobalOffensive.SDK
{
    public static class MonotonicTimer
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        /// <summary>
        ///     Gets a time stamp relative to the instance's epoch.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetTimeStamp()
        {
            return Stopwatch.Elapsed;
        }
    }
}
