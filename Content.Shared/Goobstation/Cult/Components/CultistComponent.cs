using Content.Shared.StatusIcon;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Goobstation.Cult.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedCultistSystem))]
public sealed partial class CultistComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<StatusIconPrototype> StatusIcon { get; set; } = "CultistFaction";

    [DataField]
    public SoundSpecifier StartSound = new SoundPathSpecifier("/Audio/Goobstation/Ambience/Antag/cultist_start.ogg");

    public override bool SessionSpecific => true;
}
