using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Goobstation.Cult.RuneCaster
{
    [NetworkedComponent, ComponentProtoName("RuneCaster"), Access(typeof(SharedRuneCasterSystem))]
    public abstract partial class SharedRuneCasterComponent : Component
    {
        public string SelectedState { get; set; } = string.Empty;

        [DataField("color")] public Color Color;

        [Serializable, NetSerializable]
        public enum RuneCasterUiKey : byte
        {
            Key,
        }
    }

    [Serializable, NetSerializable]
    public sealed class RuneCasterSelectMessage : BoundUserInterfaceMessage
    {
        public readonly string State;
        public RuneCasterSelectMessage(string selected)
        {
            State = selected;
        }
    }

    [Serializable, NetSerializable]
    public sealed class RuneCasterColorMessage : BoundUserInterfaceMessage
    {
        public readonly Color Color;
        public RuneCasterColorMessage(Color color)
        {
            Color = color;
        }
    }

    [Serializable, NetSerializable]
    public enum RuneCasterVisuals
    {
        State,
        Color
    }

    [Serializable, NetSerializable]
    public sealed class RuneCasterComponentState : ComponentState
    {
        public readonly Color Color;
        public readonly string State;
        public readonly float Charges;
        public readonly float Capacity;

        public RuneCasterComponentState(Color color, string state, float charges, float capacity)
        {
            Color = color;
            State = state;
            Charges = charges;
            Capacity = capacity;
        }
    }

    [Serializable, NetSerializable]
    public sealed class RuneCasterBoundUserInterfaceState : BoundUserInterfaceState
    {
        public string Selected;
        public bool SelectableColor;
        public Color Color;

        public RuneCasterBoundUserInterfaceState(string selected, bool selectableColor, Color color)
        {
            Selected = selected;
            SelectableColor = selectableColor;
            Color = color;
        }
    }
}
