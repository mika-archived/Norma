using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

using Norma.Delta.Models;
using Norma.Delta.Services;

namespace Norma.Iota.Models
{
    internal class SearchTable
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<Slot> ResultSlots { get; }

        public SearchTable(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            ResultSlots = new ObservableCollection<Slot>();
        }

        // チャンネル指定とかできるようにしたいよね。
        public void Query(string query)
        {
            ResultSlots.Clear();
            using (var connection = _databaseService.Connect())
            {
                connection.TurnOffLazyLoading();
                // 文字列比較系が死んでるかもしれない。
                var slots = connection.Slots.Include(w => w.Channel).ToList().Where(w => w.Title.Contains(query));
                foreach (var slot in slots)
                    ResultSlots.Add(slot);
            }
        }
    }
}