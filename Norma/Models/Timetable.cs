using System;
using System.Threading.Tasks;

using Norma.Gamma.Models;

namespace Norma.Models
{
    internal class Timetable
    {
        private readonly AbemaApiHost _abemaApiHost;
        public DateTime LastSyncTime { get; private set; }

        public Media Media { get; private set; }

        public Timetable(AbemaApiHost abemaApiHost)
        {
            _abemaApiHost = abemaApiHost;

            var task = new Task(async () => await Sync());
            task.Start();
            task.Wait();
        }

        public async Task Sync()
        {
            Media = await _abemaApiHost.MediaOfCurrent();
            LastSyncTime = DateTime.Now;
        }
    }
}