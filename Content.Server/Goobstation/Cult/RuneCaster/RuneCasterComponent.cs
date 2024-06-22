using Content.Server.UserInterface;

using Content.Shared.Goobstation.Cult.RuneCaster;

using Robust.Server.GameObjects;
using Robust.Shared.Audio;

namespace Content.Server.Goobstation.Cult.RuneCaster
{
    [RegisterComponent]
    public sealed partial class RuneCasterComponent : SharedRuneCasterComponent
    {
        [DataField("useSound")] public SoundSpecifier? UseSound;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("selectableColor")]
        public bool SelectableColor { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        public int Charges { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("capacity")]
        public int Capacity { get; set; } = 30;
    }
}
