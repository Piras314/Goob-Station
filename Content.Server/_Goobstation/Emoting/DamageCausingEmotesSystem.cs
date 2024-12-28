using Robust.Shared.GameStates;
using Content.Server.Chat.Systems;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Emoting;
using Robust.Shared.Prototypes;

namespace Content.Server.Emoting;

public sealed partial class DamageCausingEmotesSystem : SharedDamageCausingEmotesSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageCausingEmotesComponent, EmoteEvent>(OnEmote);
    }

    private void OnEmote(EntityUid uid, DamageCausingEmotesComponent component, ref EmoteEvent args)
    {
        PlayEmoteAnimation(uid, component, args.Emote.ID);
    }

    public void PlayEmoteAnimation(EntityUid uid, DamageCausingEmotesComponent component, ProtoId<EmotePrototype> prot)
    {
        component.Emote = prot;
        Dirty(uid, component);
    }
}
