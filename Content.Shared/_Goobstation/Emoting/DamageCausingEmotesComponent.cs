using Content.Shared.Chat.Prototypes;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Emoting;

// use as a template
//[Serializable, NetSerializable, DataDefinition] public sealed partial class AnimationNameEmoteEvent : EntityEventArgs { }

[Serializable, NetSerializable, DataDefinition] public sealed partial class DamageCausingEmoteEvent : EntityEventArgs { DamageTypePrototype _damageType; int _damageAmount; }

[RegisterComponent, NetworkedComponent] public sealed partial class DamageCausingEmotesComponent : Component
{
    [DataField] public ProtoId<EmotePrototype>? Emote;
}

[Serializable, NetSerializable] public sealed partial class DamageCausingEmotesComponentState : ComponentState
{
    public ProtoId<EmotePrototype>? Emote;

    public DamageCausingEmotesComponentState(ProtoId<EmotePrototype>? emote)
    {
        Emote = emote;
    }
}
