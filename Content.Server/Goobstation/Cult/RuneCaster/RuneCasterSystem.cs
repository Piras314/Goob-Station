using System.Linq;
using System.Numerics;
using Content.Server.Administration.Logs;
using Content.Server.Decals;
using Content.Server.Nutrition.EntitySystems;
using Content.Server.Popups;
using Content.Shared.RuneCaster;
using Content.Shared.Database;
using Content.Shared.Decals;
using Content.Shared.DoFfter;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Goobstation.Cult.RuneCaster;

/// <summary>
/// This class handles casting runes.
/// </summary>
public sealed class RuneCasterSystem : SharedRuneCasterSystem
{
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly DecalSystem _decals = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<RuneCasterComponent, ComponentInit>(OnRuneCasterInit);

        SubscribeLocalEvent<RuneCasterComponent, RuneCasterSelectMessage>(OnRuneCasterBoundUI);
        SubscribeLocalEvent<RuneCasterComponent, RuneCasterColorMessage>(OnRuneCasterBoundUIColor);

        SubscribeLocalEvent<RuneCasterComponent, UseInHandEvent>(OnRuneCasterUse);
        SubscribeLocalEvent<RuneCasterComponent, DroppedEvent>(OnRuneCasterDropped);

        SubscribeLocalEvent<RuneCasterComponent, ComponentGetState>(OnRuneCasterGetState);

        SubscribeLocalEvent<RuneCasterComponent, AfterInteractEvent>(OnRuneCasterAfterInteract);
        SubscribeLocalEvent<RuneCasterComponent, RuneCasterDoAfterEvent>(DoAfterEvent);
    }

    private static void OnRuneCasterGetState(EntityUid uid, RuneCasterComponent component, ref ComponentGetState args)
    {
        args.State = new RuneCasterComponentState(component.Color, component.SelectedState, component.Charges, component.Capacity);
    }

    private void OnRuneCasterAfterInteract(EntityUid uid, RuneCasterComponent component, AfterInteractEvent args)
    {
        // Verify that we can place a rune
        if (args.Handled || !args.CanReach)
            return;

        if (component.Charges <= 0)
        {
            _popup.PopupEntity(Loc.GetString("runecaster-interact-not-enough-left-text"), uid, args.User);

            args.Handled = true;
            return;
        }

        if (!args.ClickLocation.IsValid(EntityManager))
        {
            _popup.PopupEntity(Loc.GetString("runecaster-interact-invalid-location"), uid, args.User);

            args.Handled = true;
            return;
        }

        // Wait, then cast the rune
        _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, component.ScanDoAfterDuration, new RuneCasterDoAfterEvent(), uid, target: target, used: uid) {});
    }

    private void OnDoAfter(EntityUid uid, RuneCasterComponent component, DoAfterEvent args)
    {
        // Make sure this do-after is still valid
        if (args.Cancelled || args.Handled || args.Args.Target == null)
            return;
        
        if (!_decals.TryAddDecal(component.SelectedState, args.ClickLocation.Offset(new Vector2(-0.5f, -0.5f)), out _, component.Color, cleanable: false))
            return;

        if (component.UseSound != null)
            _audio.PlayPvs(component.UseSound, uid, AudioParams.Default.WithVariation(0.125f));

        // Use up a charge
        component.Charges--;
        Dirty(uid, component);

        _adminLogger.Add(LogType.RuneCasterCast, LogImpact.Low, $"{EntityManager.ToPrettyString(args.User):user} cast a {component.Color:color} {component.SelectedState} rune");

        args.Handled = true;
    }

    private void OnRuneCasterUse(EntityUid uid, RuneCasterComponent component, UseInHandEvent args)
    {
        // Open rune selection window if neccessary.
        if (args.Handled)
            return;

        if (!_uiSystem.HasUi(uid, SharedRuneCasterComponent.RuneCasterUiKey.Key))
            return;

        _uiSystem.TryToggleUi(uid, SharedRuneCasterComponent.RuneCasterUiKey.Key, args.User);
        _uiSystem.SetUiState(uid, SharedRuneCasterComponent.RuneCasterUiKey.Key, new RuneCasterBoundUserInterfaceState(component.SelectedState, component.SelectableColor, component.Color));

        args.Handled = true;
    }

    private void OnRuneCasterBoundUI(EntityUid uid, RuneCasterComponent component, RuneCasterSelectMessage args)
    {
        // Check if the selected state is valid
        if (!_prototypeManager.TryIndex<DecalPrototype>(args.State, out var prototype) || !prototype.Tags.Contains("RuneCaster"))
            return;

        component.SelectedState = args.State;

        Dirty(uid, component);
    }

    private void OnRuneCasterBoundUIColor(EntityUid uid, RuneCasterComponent component, RuneCasterColorMessage args)
    {
        // you still need to ensure that the given color is a valid color
        if (!component.SelectableColor || args.Color == component.Color)
            return;

        component.Color = args.Color;
        Dirty(uid, component);
    }

    private void OnRuneCasterInit(EntityUid uid, RuneCasterComponent component, ComponentInit args)
    {
        component.Charges = component.Capacity;

        // Get the first one from the catalog and set it as default
        var decal = _prototypeManager.EnumeratePrototypes<DecalPrototype>().FirstOrDefault(x => x.Tags.Contains("RuneCaster"));
        component.SelectedState = decal?.ID ?? string.Empty;
        Dirty(uid, component);
    }

    private void OnRuneCasterDropped(EntityUid uid, RuneCasterComponent component, DroppedEvent args)
    {
        // TODO: Use the existing event.
        _uiSystem.CloseUi(uid, SharedRuneCasterComponent.RuneCasterUiKey.Key, args.User);
    }
}
