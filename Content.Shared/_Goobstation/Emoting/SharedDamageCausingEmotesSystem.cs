using Robust.Shared.GameStates;

namespace Content.Shared.Emoting;

public abstract class SharedDamageCausingEmotesSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageCausingEmotesComponent, ComponentGetState>(OnGetState);
    }

    private void OnGetState(Entity<DamageCausingEmotesComponent> ent, ref ComponentGetState args)
    {
        args.State = new DamageCausingEmotesComponentState(ent.Comp.Emote);
    }
}
