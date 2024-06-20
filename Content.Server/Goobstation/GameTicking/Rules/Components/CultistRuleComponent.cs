using Content.Shared.Roles;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using System.Collections.Generic;

namespace Content.Server.Goobstation.GameTicking.Rules.Components;

[RegisterComponent, Access(typeof(CultistRuleSystem))]
public sealed partial class CultistRuleComponent : Component
{
    public SoundSpecifier BriefingSound = new SoundPathSpecifier("/Audio/Goobstation/Ambience/Antag/cultist_start.ogg");
    public List<EntityUid> Cultists = new();
}
