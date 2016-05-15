﻿using System.Reactive.Linq;

using Norma.Extensions;
using Norma.Models;
using Norma.ViewModels.Internal;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }
        public ReadOnlyReactiveProperty<bool> IsEnableCommentArea { get; }

        public AbemaCommentViewModel(AbemaState abemaState)
        {
            var commentHost = new CommentHost(abemaState).AddTo(this);
            Comments = commentHost.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w)).AddTo(this);
            IsEnableCommentArea = abemaState.ObserveProperty(w => w.IsBroadcastCm)
                                            .Select(w => !w)
                                            .ToReadOnlyReactiveProperty().AddTo(this);
        }
    }
}