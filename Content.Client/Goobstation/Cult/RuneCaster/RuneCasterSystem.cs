using Content.Client.Items;
using Content.Client.Message;
using Content.Client.Stylesheets;

using Content.Shared.Goobstation.Cult.RuneCaster;

using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Localization;
using Robust.Shared.Timing;

namespace Content.Client.Goobstation.Cult.RuneCaster;

public sealed class RuneCasterSystem : SharedRuneCasterSystem
{
    // Didn't do in shared because I don't think most of the server stuff can be predicted.
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RuneCasterComponent, ComponentHandleState>(OnRuneCasterHandleState);
        Subs.ItemStatus<RuneCasterComponent>(ent => new StatusControl(ent));
    }

    private static void OnRuneCasterHandleState(EntityUid uid, RuneCasterComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not RuneCasterComponentState state)
            return;

        component.Color = state.Color;
        component.SelectedState = state.State;
        component.Charges = state.Charges;
        component.Capacity = state.Capacity;

        component.UIUpdateNeeded = true;
    }

    private sealed class StatusControl : Control
    {
        private readonly RuneCasterComponent _parent;
        private readonly RichTextLabel _label;

        public StatusControl(RuneCasterComponent parent)
        {
            _parent = parent;
            _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };
            AddChild(_label);

            parent.UIUpdateNeeded = true;
        }

        protected override void FrameUpdate(FrameEventArgs args)
        {
            base.FrameUpdate(args);

            if (!_parent.UIUpdateNeeded)
            {
                return;
            }

            _parent.UIUpdateNeeded = false;
            _label.SetMarkup(Robust.Shared.Localization.Loc.GetString("goobstation-runecaster-casting-label",
                ("color",_parent.Color),
                ("state",_parent.SelectedState),
                ("charges", _parent.Charges),
                ("capacity",_parent.Capacity)));
        }
    }
}
