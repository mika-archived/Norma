using System.Collections.Generic;
using System.Linq;

using Norma.Delta.Models;

using ApiModel = Norma.Gamma.Models;

namespace Norma.Eta.Helpers
{
    internal static class ApiModelToFlatModelHelper
    {
        public static Channel ConvertToChannel(this ApiModel.Channel channel)
        {
            return new Channel
            {
                ChannelId = channel.Id,
                Name = channel.Name,
                Order = channel.Order
            };
        }

        public static Episode ConvertToEpisode(this ApiModel.Program program)
        {
            return new Episode
            {
                EpisodeId = program.Id,
                Sequence = int.Parse(program.Episode.Sequence)
            };
        }

        public static Slot ConvertToSlot(this ApiModel.Slot slot)
        {
            return new Slot
            {
                Description = slot.Content,
                EndAt = slot.EndAt,
                Highlight = slot.Highlight,
                HighlightDetail = slot.DetailHighlight,
                IsFirst = slot.Mark.IsFirst,
                IsLast = false,
                SlotId = slot.Id,
                StartAt = slot.StartAt,
                Title = slot.Title
            };
        }

        public static IEnumerable<Thumbnail> ConvertToThumbnail(this ApiModel.ProvidedInfo providedInfo)
        {
            var list = new List<Thumbnail>();
            if (!string.IsNullOrWhiteSpace(providedInfo.ThumbImg))
                list.Add(new Thumbnail {Path = providedInfo.ThumbImg});
            if (providedInfo.SceneThumbImgs != null)
                list.AddRange(providedInfo.SceneThumbImgs.Select(path => new Thumbnail {Path = path}));
            return list;
        }
    }
}