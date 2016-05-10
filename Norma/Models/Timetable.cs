using System;
using System.Threading.Tasks;

using Norma.Gamma.Models;

namespace Norma.Models
{
    internal class Timetable
    {
        private static Timetable _instance;

        public DateTime LastSyncTime { get; private set; }
        public static Timetable Instance => _instance ?? (_instance = new Timetable());

        public Media Media { get; private set; }

        private Timetable()
        {

        }

        public async Task Sync()
        {
            Media = await AbemaApiHost.Instance.MediaOfCurrent();
            LastSyncTime = DateTime.Now;
        }
    }
}