using Norma.Models.Config;
using Norma.ViewModels.Internal;

using Reactive.Bindings;

namespace Norma.ViewModels.Tabs.Options
{
    internal class OperationViewModel : ViewModel
    {
        public ReactiveProperty<uint> UpdateIntervalOfProgram { get; private set; }
        public ReactiveProperty<uint> UpdateIntervalOfThumbnails { get; private set; }
        public ReactiveProperty<uint> ReceptionIntervalOfComments { get; private set; }
        public ReactiveProperty<uint> SamplingIntervalOfProgramState { get; private set; }
        public ReactiveProperty<uint> NumberOfHoldingComments { get; private set; }

        public OperationViewModel(OperationConfig oc)
        {
            UpdateIntervalOfProgram = ReactiveProperty.FromObject(oc, w => w.UpdateIntervalOfProgram);
            UpdateIntervalOfThumbnails = ReactiveProperty.FromObject(oc, w => w.UpdateIntervalOfThumbnails);
            ReceptionIntervalOfComments = ReactiveProperty.FromObject(oc, w => w.ReceptionIntervalOfComments);
            SamplingIntervalOfProgramState = ReactiveProperty.FromObject(oc, w => w.SamplingIntervalOfProgramState);
            NumberOfHoldingComments = ReactiveProperty.FromObject(oc, w => w.NumberOfHoldingComments);
        }
    }
}