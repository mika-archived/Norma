using System;
using System.Collections.Generic;
using System.Linq;

using Norma.Models;
using Norma.Models.Config;
using Norma.ViewModels.Internal;

using Reactive.Bindings;

namespace Norma.ViewModels.Tabs.Options
{
    internal class OperationViewModel : ViewModel
    {
        public List<ShortcutKey> KeyTypes
            => ((PostKey[]) Enum.GetValues(typeof(PostKey))).Select(w => new ShortcutKey(w)).ToList();

        public ReactiveProperty<uint> UpdateIntervalOfProgram { get; private set; }
        public ReactiveProperty<uint> UpdateIntervalOfThumbnails { get; private set; }
        public ReactiveProperty<uint> ReceptionIntervalOfComments { get; private set; }
        public ReactiveProperty<uint> SamplingIntervalOfProgramState { get; private set; }
        public ReactiveProperty<uint> NumberOfHoldingComments { get; private set; }
        public ReactiveProperty<ShortcutKey> PostKey { get; private set; }

        public OperationViewModel(OperationConfig oc)
        {
            UpdateIntervalOfProgram = ReactiveProperty.FromObject(oc, w => w.UpdateIntervalOfProgram);
            UpdateIntervalOfThumbnails = ReactiveProperty.FromObject(oc, w => w.UpdateIntervalOfThumbnails);
            ReceptionIntervalOfComments = ReactiveProperty.FromObject(oc, w => w.ReceptionIntervalOfComments);
            SamplingIntervalOfProgramState = ReactiveProperty.FromObject(oc, w => w.SamplingIntervalOfProgramState);
            NumberOfHoldingComments = ReactiveProperty.FromObject(oc, w => w.NumberOfHoldingComments);
            PostKey = ReactiveProperty.FromObject(oc, w => w.PostKeyType, x => new ShortcutKey(x), w => w.PostKey);
        }
    }
}