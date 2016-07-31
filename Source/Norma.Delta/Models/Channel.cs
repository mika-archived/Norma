using System.Collections.Generic;

namespace Norma.Delta.Models
{
    public class Channel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public virtual ICollection<Slot> Slots { get; set; }

        public Channel()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Slots = new List<Slot>();
        }
    }
}