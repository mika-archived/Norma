using System.Collections.ObjectModel;

using Newtonsoft.Json;

namespace Norma.Eta.Models.Configurations
{
    public class OperationConfig
    {
        [JsonProperty]
        public uint UpdateIntervalOfProgram { get; set; }

        [JsonProperty]
        public uint UpdateIntervalOfThumbnails { get; set; }

        [JsonProperty]
        public uint ReceptionIntervalOfComments { get; set; }

        [JsonProperty]
        public uint SamplingIntervalOfProgramState { get; set; }

        [JsonProperty]
        public uint NumberOfHoldingComments { get; set; }

        [JsonProperty]
        public PostKey PostKeyType { get; set; }

        [JsonProperty]
        public uint ToastNotificationBeforeMinutes { get; set; }

        [JsonProperty]
        public ObservableCollection<MuteKeyword> MuteKeywords { get; set; }

        [JsonProperty]
        public VideoQuality VideoQuality { get; set; }

        public OperationConfig()
        {
            UpdateIntervalOfProgram = 1;
            UpdateIntervalOfThumbnails = 30;
            ReceptionIntervalOfComments = 10;
            SamplingIntervalOfProgramState = 1;
            NumberOfHoldingComments = 200;
            PostKeyType = PostKey.EnterOnly;
            ToastNotificationBeforeMinutes = 5;
            MuteKeywords = new ObservableCollection<MuteKeyword>();
            VideoQuality = VideoQuality.Auto;
        }
    }
}