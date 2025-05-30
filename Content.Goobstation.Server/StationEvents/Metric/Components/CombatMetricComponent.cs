// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2025 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 2025 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Goobstation.Maths.FixedPoint;

namespace Content.Goobstation.Server.StationEvents.Metric.Components;

[RegisterComponent, Access(typeof(CombatMetricSystem))]
public sealed partial class CombatMetricComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double HostileScore = 10.0f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double FriendlyScore = 10.0f;

    /// <summary>
    ///   Cost per point of medical damage for friendly entities
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double MedicalMultiplier = 0.05f;

    /// <summary>
    ///   Cost for friendlies who are in crit
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double CritScore = 10.0f;

    /// <summary>
    ///   Cost for friendlies who are dead
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double DeadScore = 20.0f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double maxItemThreat = 15.0f;

    /// <summary>
    ///   ItemThreat - evaluate based on item tags how powerful a player is
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<string, double> ItemThreat =
        new()
        {
            { "Taser", 2.0f },
            { "Sidearm", 2.0f },
            { "Rifle", 5.0f },
            { "HighRiskItem", 2.0f },
            { "CombatKnife", 1.0f },
            { "Knife", 1.0f },
            { "Grenade", 2.0f },
            { "Bomb", 2.0f },
            { "MagazinePistol", 0.5f },
            { "Hacking", 1.0f },
            { "Jetpack", 1.0f },
        };

}
