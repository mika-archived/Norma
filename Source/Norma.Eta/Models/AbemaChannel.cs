using System;

using Norma.Eta.Properties;

namespace Norma.Eta.Models
{
    public enum AbemaChannel
    {
        /// <summary>
        ///     1ch - Onegai Ranking
        /// </summary>
        OnegaiRanking,

        /// <summary>
        ///     2ch - Abema news/
        /// </summary>
        AbemaNews,

        /// <summary>
        ///     3ch - Abema SPECIAL
        /// </summary>
        AbemaSpecial,

        /// <summary>
        ///     4ch - SPECIAL PLUS
        /// </summary>
        SpecialPlus,

        /// <summary>
        ///     5ch - SPECIAL PLUS 2
        /// </summary>
        SpecialPlus2,

        /// <summary>
        ///     6ch - REALITY SHOW
        /// </summary>
        RealityShow,

        /// <summary>
        ///     7ch - MTV HITS
        /// </summary>
        MtvHits,

        /// <summary>
        ///     8ch - SPACE SHOWER MUSIC
        /// </summary>
        SpaceShowerMusic,

        /// <summary>
        ///     9ch - ドラマ CHANNEL
        /// </summary>
        DramaChannel,

        /// <summary>
        ///     10ch - Documentary
        /// </summary>
        Documentary,

        /// <summary>
        ///     11ch - バラエティ CHANNEL
        /// </summary>
        VarietyChannel,

        /// <summary>
        ///     12ch - ペット
        /// </summary>
        Pet,

        /// <summary>
        ///     13ch - CLUB CHANNEL
        /// </summary>
        ClubChannel,

        /// <summary>
        ///     14ch - WORLD SPORTS
        /// </summary>
        WorldSports,

        /// <summary>
        ///     15ch - ヨコノリ Surf Snow Skate
        /// </summary>
        YokonoriSports,

        /// <summary>
        ///     16ch - VICE
        /// </summary>
        Vice,

        /// <summary>
        ///     17ch - アニメ24
        /// </summary>
        Anime24,

        /// <summary>
        ///     18ch - 深夜アニメ
        /// </summary>
        MidnightAnime,

        /// <summary>
        ///     19ch - なつかしアニメ
        /// </summary>
        OldtimeAnime,

        /// <summary>
        ///     20ch - 家族アニメ
        /// </summary>
        FamilyAnime,

        /// <summary>
        ///     21ch - 新作アニメ
        /// </summary>
        NewAnime,

        /// <summary>
        ///     22ch - EDGE SPORT HD
        /// </summary>
        EdgeSportHd,

        /// <summary>
        ///     23ch - 釣り
        /// </summary>
        Fishing,

        /// <summary>
        ///     24ch - 麻雀
        /// </summary>
        Mahjong
    }

    public static class AbemaChannelExt
    {
        public static string ToUrlString(this AbemaChannel channel)
        {
            switch (channel)
            {
                case AbemaChannel.Documentary:
                case AbemaChannel.Pet:
                case AbemaChannel.Vice:
                case AbemaChannel.Anime24:
                case AbemaChannel.Fishing:
                case AbemaChannel.Mahjong:
                    return channel.ToString().ToLower();

                case AbemaChannel.OnegaiRanking:
                    return "onegai-ranking";

                case AbemaChannel.AbemaNews:
                    return "abema-news";

                case AbemaChannel.AbemaSpecial:
                    return "abema-special";

                case AbemaChannel.SpecialPlus:
                    return "special-plus";

                case AbemaChannel.SpecialPlus2:
                    return "special-plus-2";

                case AbemaChannel.RealityShow:
                    return "reality-show";

                case AbemaChannel.MtvHits:
                    return "mtv-hits";

                case AbemaChannel.SpaceShowerMusic:
                    return "space-shower";

                case AbemaChannel.DramaChannel:
                    return "drama";

                case AbemaChannel.VarietyChannel:
                    return "variety";

                case AbemaChannel.ClubChannel:
                    return "club-channel";

                case AbemaChannel.WorldSports:
                    return "world-sports";

                case AbemaChannel.YokonoriSports:
                    return "yokonori-sports";

                case AbemaChannel.MidnightAnime:
                    return "midnight-anime";

                case AbemaChannel.OldtimeAnime:
                    return "oldtime-anime";

                case AbemaChannel.FamilyAnime:
                    return "family-anime";

                case AbemaChannel.NewAnime:
                    return "new-anime";

                case AbemaChannel.EdgeSportHd:
                    return "edge-sport";

                // case AbemaChannels.AbemaTvFresh:
                //    return "abematv-fresh";

                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
        }

        public static AbemaChannel FromUrlString(string url)
        {
            var str = url.Replace("https://abema.tv/now-on-air/", "");
            switch (str)
            {
                case "onegai-ranking":
                    return AbemaChannel.OnegaiRanking;

                case "abema-news":
                    return AbemaChannel.AbemaNews;

                case "abema-special":
                    return AbemaChannel.AbemaSpecial;

                case "special-plus":
                    return AbemaChannel.SpecialPlus;

                case "special-plus-2":
                    return AbemaChannel.SpecialPlus2;

                case "reality-show":
                    return AbemaChannel.RealityShow;

                case "mtv-hits":
                    return AbemaChannel.MtvHits;

                case "space-shower":
                    return AbemaChannel.SpaceShowerMusic;

                case "drama":
                    return AbemaChannel.DramaChannel;

                case "documentary":
                    return AbemaChannel.Documentary;

                case "variety":
                    return AbemaChannel.VarietyChannel;

                case "pet":
                    return AbemaChannel.Pet;

                case "club-channel":
                    return AbemaChannel.ClubChannel;

                case "world-sports":
                    return AbemaChannel.WorldSports;

                case "yokonori-sports":
                    return AbemaChannel.YokonoriSports;

                case "vice":
                    return AbemaChannel.Vice;

                case "anime24":
                    return AbemaChannel.Anime24;

                case "midnight-anime":
                    return AbemaChannel.MidnightAnime;

                case "oldtime-anime":
                    return AbemaChannel.OldtimeAnime;

                case "family-anime":
                    return AbemaChannel.FamilyAnime;

                case "new-anime":
                    return AbemaChannel.NewAnime;

                case "edge-sport":
                    return AbemaChannel.EdgeSportHd;

                case "fishing":
                    return AbemaChannel.Fishing;

                case "mahjong":
                    return AbemaChannel.Mahjong;

                default:
                    throw new ArgumentOutOfRangeException(nameof(url), url, null);
            }
        }

        public static string ToLocaleString(this AbemaChannel obj)
        {
            return (string) typeof(Resources).GetProperty(obj.ToString()).GetValue(null);
        }
    }
}