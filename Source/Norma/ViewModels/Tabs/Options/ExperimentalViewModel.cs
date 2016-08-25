using System;
using System.Collections.Generic;
using System.Linq;

using Norma.Eta.Models;
using Norma.Eta.Models.Enums;
using Norma.Eta.Mvvm;

using Reactive.Bindings;

namespace Norma.ViewModels.Tabs.Options
{
    internal class ExperimentalViewModel : ViewModel
    {
        private readonly Configuration _configuration;

        public ReactiveProperty<bool> IsEnabledExperimentalFeatures { get; }

        public List<EnumWrap<VideoQuality>> VideoQualities
            => ((VideoQuality[]) Enum.GetValues(typeof(VideoQuality))).Select(w => new EnumWrap<VideoQuality>(w))
                                                                      .ToList();

        public ReactiveProperty<EnumWrap<VideoQuality>> VideoQuality { get; private set; }

        public ExperimentalViewModel(Configuration configuration)
        {
            _configuration = configuration;
            IsEnabledExperimentalFeatures = ReactiveProperty.FromObject(configuration.Root.Others,
                                                                        w => w.IsEnabledExperimentalFeatures)
                                                            .AddTo(this);
            VideoQuality = ReactiveProperty.FromObject(configuration.Root.Operation, w => w.VideoQuality,
                                                       x => new EnumWrap<VideoQuality>(x),
                                                       w => w.EnumValue)
                                           .AddTo(this);
        }
    }
}