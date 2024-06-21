using Content.Shared.Goobstation.Cult.RuneCaster;

using Robust.Shared.GameObjects;
using Robust.Shared.ViewVariables;

namespace Content.Client.Goobstation.Cult.RuneCaster
{
    [RegisterComponent]
    public sealed partial class RuneCasterComponent : SharedRuneCasterComponent
    {
        [ViewVariables(VVAccess.ReadWrite)] public bool UIUpdateNeeded;
        [ViewVariables] public int Charges { get; set; }
        [ViewVariables] public int Capacity { get; set; }
    }
}
