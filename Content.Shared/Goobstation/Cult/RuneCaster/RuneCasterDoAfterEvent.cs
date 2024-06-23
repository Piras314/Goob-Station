using Content.Shared.DoAfter;
using Robust.Shared.Map;

namespace Content.Shared.Goobstation.Cult.RuneCaster;

public sealed partial class RuneCasterDoAfterEvent : SimpleDoAfterEvent
{
    /// <summary>
    ///     Location that the user clicked outside of their interaction range.
    /// </summary>
    public EntityCoordinates ClickLocation { get; }

    public RuneCasterDoAfterEvent(EntityCoordinates clickLocation) : base()
    {
        ClickLocation = clickLocation;
    }
}
