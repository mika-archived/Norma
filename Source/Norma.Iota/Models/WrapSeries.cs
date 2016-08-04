using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Practices.ServiceLocation;

using Norma.Delta.Models;
using Norma.Delta.Services;

namespace Norma.Iota.Models
{
    internal class WrapSeries
    {
        // #9 以降を削除
        private readonly Regex _regex = new Regex(@"#[0-9]+.*", RegexOptions.Compiled);

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
                var sb = new StringBuilder();
                foreach (var c in slots.First().Title)
                {
                    sb.Append(c);
                    if (slots.All(w => w.Title.StartsWith(sb.ToString())))
                        continue;
                    SeriesName = sb.ToString().Substring(0, sb.Length - 1);
                    break;
                }
                // 週1配信系(再放送除く)はここに来る
                if (sb.ToString() == slots.First().Title)
                    SeriesName = _regex.Replace(sb.ToString(), "");
            }
            SeriesName = string.IsNullOrWhiteSpace(SeriesName) ? _series.SeriesId : SeriesName.Replace("#", "").Trim();
        }
    }
}