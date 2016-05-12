using System;

namespace Norma.Models
{
    internal enum AbemaChannel
    {
        /// <summary>
        ///     1ch - Abema news/
        /// </summary>
        AbemaNews,

        /// <summary>
        ///     2ch - Abema SPECIAL
        /// </summary>
        AbemaSpecial,

        /// <summary>
        ///     3ch - SPECIAL PLUS
        /// </summary>
        SpecialPlus,

        /// <summary>
        ///     4ch - REALITY SHOW
        /// </summary>
        RealityShow,

        /// <summary>
        ///     5ch - MTV HITS
        /// </summary>
        MtvHits,

        /// <summary>
        ///     6ch - SPACE SHOWER MUSIC
        /// </summary>
        SpaceShowerMusic,

        /// <summary>
        ///     7ch - ドラマ CHANNEL
        /// </summary>
        DramaChannel,

        /// <summary>
        ///     8ch - Documentary
        /// </summary>
        Documentary,

        /// <summary>
        ///     9ch - バラエティ CHANNEL
        /// </summary>
        VarietyChannel,

        /// <summary>
        ///     10ch - ペット
        /// </summary>
        Pet,

        /// <summary>
        ///     11ch - CLUB CHANNEL
        /// </summary>
        ClubChannel,

        /// <summary>
        ///     12ch - WORLD SPORTS
        /// </summary>
        WorldSports,

        /// <summary>
        ///     13ch - ヨコノリ Surf Snow Skate
        /// </summary>
        YokonoriSports,

        /// <summary>
        ///     14ch - VICE
        /// </summary>
        Vice,

        /// <summary>
        ///     15ch - アニメ24
        /// </summary>
        Anime24,

        /// <summary>
        ///     16ch - 深夜アニメ
        /// </summary>
        MidnightAnime,

        /// <summary>
        ///     17ch - なつかしアニメ
        /// </summary>
        OldtimeAnime,

        /// <summary>
        ///     18ch - 家族アニメ
        /// </summary>
        FamilyAnime,

        /// <summary>
        ///     19ch - EDGE SPORT HD
        /// </summary>
        EdgeSportHd,

        /// <summary>
        ///     20ch - 釣り
        /// </summary>
        Fishing,

        /// <summary>
        ///     21ch - 麻雀
        /// </summary>
        Mahjong

        /// <summary>
        ///     22ch - AbemaTV FRESH!
        /// </summary>
        // AbemaTvFresh
    }

    internal static class AbemaChannelExt
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

                case AbemaChannel.AbemaNews:
                    return "abema-news";

                case AbemaChannel.AbemaSpecial:
                    return "abema-special";

                case AbemaChannel.SpecialPlus:
                    return "special-plus";

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
                case "abema-news":
                    return AbemaChannel.AbemaNews;

                case "abema-special":
                    return AbemaChannel.AbemaSpecial;

                case "special-plus":
                    return AbemaChannel.SpecialPlus;

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
    }
}