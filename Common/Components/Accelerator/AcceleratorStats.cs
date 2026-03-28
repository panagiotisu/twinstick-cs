using Godot;

namespace Game.Components;

[GlobalClass]
public partial class AcceleratorStats : Resource
{
    [Export] public float MaxSpeed { get; private set; } = 80f;
    [Export] public float AcceleractionCoefficient { get; private set; } = 100f;
}
