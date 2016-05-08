using Norma.Gamma.Models;

namespace Norma.Models
{
    internal class Timetable
    {
        private static Timetable _instance;
        public static Timetable Instance => _instance ?? (_instance = new Timetable());

        public Media Media { get; private set; }

        private Timetable()
        {

        }

        public async void Sync() => Media = await AbemaApiHost.Instance.MediaOfCurrent();
    }
}