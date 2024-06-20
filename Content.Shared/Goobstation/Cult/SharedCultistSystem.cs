using Content.Shared.Antag;
using Content.Shared.Goobstation.Cult.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Mindshield.Components;
using Content.Shared.Popups;
using Content.Shared.Stunnable;

namespace Content.Shared.Goobstation.Cult;

public abstract class SharedCultistSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedStunSystem _sharedStun = default!;

    public override void Initialize()
    {
        base.Initialize();

        //SubscribeLocalEvent<MindShieldComponent, MapInitEvent>(MindshieldImplanted);
        SubscribeLocalEvent<CultistComponent, ComponentStartup>(DirtyUp);
        //SubscribeLocalEvent<ShowAntagIconsComponent, ComponentStartup>(DirtyUp);
    }

    private void MindshieldImplanted(EntityUid uid, MindShieldComponent comp, MapInitEvent init)
    {
        if (HasComp<CultistComponent>(uid))
        {
            var stunTime = TimeSpan.FromSeconds(4);
            var name = Identity.Entity(uid, EntityManager);
            RemComp<CultistComponent>(uid);
            _sharedStun.TryParalyze(uid, stunTime, true);
            // rev loc because it kinda fits
            _popupSystem.PopupEntity(Loc.GetString("rev-break-control", ("name", name)), uid);
        }
    }

    private void DirtyUp<T>(EntityUid someUid, T someComp, ComponentStartup ev)
    {
        var revComps = AllEntityQuery<CultistComponent>();
        while (revComps.MoveNext(out var uid, out var comp))
        {
            Dirty(uid, comp);
        }
    }
}
