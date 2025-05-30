// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2025 BombasterDS <115770678+BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 2025 BombasterDS2 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 2025 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 2025 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Goobstation.Shared.Clothing.Components;
using Content.Shared.Popups;
using Content.Shared.PowerCell;
using Content.Shared.Wires;

namespace Content.Goobstation.Shared.Clothing.Systems;

/// <summary>
/// Used for sealable clothing that requires power to work
/// </summary>
public abstract class SharedPoweredSealableClothingSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedPowerCellSystem _powerCellSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SealableClothingRequiresPowerComponent, MapInitEvent>(OnRequiresPowerMapInit);
        SubscribeLocalEvent<SealableClothingRequiresPowerComponent, ClothingSealAttemptEvent>(OnRequiresPowerSealAttempt);
        SubscribeLocalEvent<SealableClothingRequiresPowerComponent, AttemptChangePanelEvent>(OnRequiresPowerChangePanelAttempt);
    }

    private void OnRequiresPowerMapInit(Entity<SealableClothingRequiresPowerComponent> entity, ref MapInitEvent args)
    {
        if (!TryComp(entity, out SealableClothingControlComponent? control) || !TryComp(entity, out PowerCellDrawComponent? draw))
            return;

        draw.Enabled = control.IsCurrentlySealed;
    }

    /// <summary>
    /// Checks if control have enough power to seal
    /// </summary>
    private void OnRequiresPowerSealAttempt(Entity<SealableClothingRequiresPowerComponent> entity, ref ClothingSealAttemptEvent args)
    {
        if (!TryComp(entity, out SealableClothingControlComponent? controlComp) || !TryComp(entity, out PowerCellDrawComponent? cellDrawComp) || args.Cancelled)
            return;

        // Prevents sealing if wires panel is opened
        if (TryComp(entity, out WiresPanelComponent? panel) && panel.Open)
        {
            _popupSystem.PopupClient(Loc.GetString(entity.Comp.ClosePanelFirstPopup), entity, args.User);
            args.Cancel();
            return;
        }

        // Control shouldn't use charge on unsealing
        if (controlComp.IsCurrentlySealed)
            return;

        if (!_powerCellSystem.HasDrawCharge(entity, cellDrawComp) || !_powerCellSystem.HasActivatableCharge(entity, cellDrawComp))
        {
            _popupSystem.PopupClient(Loc.GetString(entity.Comp.NotPoweredPopup), entity, args.User);
            args.Cancel();
        }
    }

    /// <summary>
    /// Prevents wires panel from opening if clothing is sealed
    /// </summary>
    private void OnRequiresPowerChangePanelAttempt(Entity<SealableClothingRequiresPowerComponent> entity, ref AttemptChangePanelEvent args)
    {
        if (args.Cancelled || !TryComp(entity, out SealableClothingControlComponent? controlComp))
            return;

        if (controlComp.IsCurrentlySealed || controlComp.IsInProcess)
        {
            _popupSystem.PopupClient(Loc.GetString(entity.Comp.OpenSealedPanelFailPopup), entity, args.User);
            args.Cancelled = true;
        }
    }
}

