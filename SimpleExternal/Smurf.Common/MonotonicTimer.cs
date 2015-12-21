using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurf.Common
{
    public static class MonotonicTimer
    {
        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        /// <summary>
        ///     Gets a time stamp relative to the instance's epoch.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetTimeStamp()
        {
            return _stopwatch.Elapsed;
        }
    }
}
