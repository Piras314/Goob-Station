// SPDX-FileCopyrightText: 2023 Just-a-Unity-Dev <just-a-unity-dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 LankLTE <135308300+LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 LankLTE <twlowe06@gmail.com>
// SPDX-FileCopyrightText: 2023 eclips_e <67359748+Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Defusable.Components;
using Content.Server.Defusable.Systems;
using Content.Server.Wires;
using Content.Shared.Defusable;
using Content.Shared.Wires;

namespace Content.Server.Defusable.WireActions;

public sealed partial class BoltWireAction : ComponentWireAction<DefusableComponent>
{
    public override Color Color { get; set; } = Color.Red;
    public override string Name { get; set; } = "wire-name-bomb-bolt";
    public override bool LightRequiresPower { get; set; } = false;

    public override StatusLightState? GetLightState(Wire wire, DefusableComponent comp)
    {
        return comp.Bolted ? StatusLightState.On : StatusLightState.Off;
    }

    public override object StatusKey { get; } = DefusableWireStatus.BoltIndicator;

    public override bool Cut(EntityUid user, Wire wire, DefusableComponent comp)
    {
        return EntityManager.System<DefusableSystem>().BoltWireCut(user, wire, comp);
    }

    public override bool Mend(EntityUid user, Wire wire, DefusableComponent comp)
    {
        return EntityManager.System<DefusableSystem>().BoltWireMend(user, wire, comp);
    }

    public override void Pulse(EntityUid user, Wire wire, DefusableComponent comp)
    {
        EntityManager.System<DefusableSystem>().BoltWirePulse(user, wire, comp);
    }
}