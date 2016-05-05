using System;

namespace Norma.Models
{
    internal enum AbemaChannels
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
        public static string ToUrlString(this AbemaChannels channel)
        {
            switch (channel)
            {
                case AbemaChannels.Documentary:
                case AbemaChannels.Pet:
                case AbemaChannels.Vice:
                case AbemaChannels.Anime24:
                case AbemaChannels.Fishing:
                case AbemaChannels.Mahjong:
                    return channel.ToString().ToLower();

                case AbemaChannels.AbemaNews:
                    return "abema-news";

                case AbemaChannels.AbemaSpecial:
                    return "abema-special";

                case AbemaChannels.SpecialPlus:
                    return "special-plus";

                case AbemaChannels.RealityShow:
                    return "reality-show";

                case AbemaChannels.MtvHits:
                    return "mtv-hits";

                case AbemaChannels.SpaceShowerMusic:
                    return "space-shower";

                case AbemaChannels.DramaChannel:
                    return "drama";

                case AbemaChannels.VarietyChannel:
                    return "variety";

                case AbemaChannels.ClubChannel:
                    return "club-channel";

                case AbemaChannels.WorldSports:
                    return "world-sports";

                case AbemaChannels.YokonoriSports:
                    return "yokonori-sports";

                case AbemaChannels.MidnightAnime:
                    return "midnight-anime";

                case AbemaChannels.OldtimeAnime:
                    return "oldtime-anime";

                case AbemaChannels.FamilyAnime:
                    return "family-anime";

                case AbemaChannels.EdgeSportHd:
                    return "edge-sport";

                // case AbemaChannels.AbemaTvFresh:
                //    return "abematv-fresh";

                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
        }
    }
}