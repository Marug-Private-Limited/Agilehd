using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileHDWPF.AgileHelpers
{
    public static class Global
    {
        /// <summary>
        /// // Created for ShortCuts on 08/11/18
        /// </summary>
        public static List<string> Transition = new List<string>();
        public static int index = -1;
        //public static dashboard db_class = new dashboard();
        public static string SequencerKey = string.Empty;
        public static bool FirstSequencerCall = true;
        public static bool isLiveRunning = false;
    }
}
