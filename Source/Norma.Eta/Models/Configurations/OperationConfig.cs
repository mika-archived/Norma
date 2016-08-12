using System.Collections.ObjectModel;

using Norma.Eta.Models.Enums;

using Prism.Mvvm;

namespace Norma.Eta.Models.Configurations
{
    public class OperationConfig : BindableBase
    {
        public uint UpdateIntervalOfThumbnails { get; set; }

        public uint ReceptionIntervalOfComments { get; set; }

        public uint SamplingIntervalOfProgramState { get; set; }

        public uint NumberOfHoldingComments { get; set; }

        public PostKey PostKeyType { get; set; }

        public uint ToastNotificationBeforeMinutes { get; set; }

        public ObservableCollection<MuteKeyword> MuteKeywords { get; set; }

        public VideoQuality VideoQuality { get; set; }

        public Branch Branch { get; set; }

        public bool IsAbsoluteTime { get; set; }

        public uint Delay { get; set; }

        public OperationConfig()
        {
            UpdateIntervalOfThumbnails = 30;
            ReceptionIntervalOfComments = 10;
            SamplingIntervalOfProgramState = 1;
            NumberOfHoldingComments = 200;
            PostKeyType = PostKey.EnterOnly;
            ToastNotificationBeforeMinutes = 5;
            MuteKeywords = new ObservableCollection<MuteKeyword>();
            VideoQuality = VideoQuality.Auto;
            Branch = Branch.Master;
            IsAbsoluteTime = true;
            IsShowFavoriteOnly = false;
            Delay = 100;
        }

        #region IsShowFavoriteOnly

        private bool _isShowFavoriteOnly;

        public bool IsShowFavoriteOnly
        {
            get { return _isShowFavoriteOnly; }
            set { SetProperty(ref _isShowFavoriteOnly, value); }
        }

        #endregion
    }
}