using System.Linq;
using System.Text;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Services;

namespace Norma.Iota.Models
{
    internal class WrapSeries
    {
        private readonly Series _series;

        public string SeriesName { get; private set; }

        public WrapSeries(Series series)
        {
            _series = series;
            RequestDetail();
        }

        private void RequestDetail()
        {
            var databaseService = ServiceLocator.Current.GetInstance<DatabaseService>();
            using (var connection = databaseService.Connect())
            {
                // FirstOrDefault で null は来ないはず
                var slots = connection.Slots.Where(w => w.Episodes.FirstOrDefault().Series.SeriesId == _series.SeriesId).ToList();
                foreach (var slot in slots)
                {
                    var sb = new StringBuilder();
                    foreach (var c in slot.Title)
                    {
                        sb.Append(c);
                        if (slots.All(w => w.Title.StartsWith(sb.ToString())))
                            continue;
                        SeriesName = sb.ToString().Substring(0, sb.Length - 1);
                        break;
                    }
                }
            }
            SeriesName = string.IsNullOrWhiteSpace(SeriesName) ? _series.SeriesId : SeriesName.Replace("#", "").Trim();
        }
    }
}