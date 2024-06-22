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
        public bool SelectableColor;

        [ViewVariables(VVAccess.ReadWrite)]
        public float Charges;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("capacity")]
        public float Capacity = 30f;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("runeCastDuration")]
        public float RuneCasterDoAfterDuration = 1f; // TODO: This should be longer, short for debug purposes only
    }
}
