using Godot;
using Game.Globals;

namespace Game.Components;

[GlobalClass, Icon("Accelerator.svg")]
public partial class Accelerator : Node
{
	private CharacterBody2D _moveableEntity;

	[Export] private AcceleratorStats _stats;

	public float? MaxSpeedOverride { get; set; }
	public Vector2 Velocity { get; private set; } = Vector2.Zero;
	public float Speed => Velocity.Length();
	public Vector2? VelocityOverride { get; set; }
	public float? AccelerationOverride { get; set; }

	public override void _Ready()
	{
		_moveableEntity = GetOwner<CharacterBody2D>();
	}

	public void AccelerateTowards(Vector2 direction, double delta)
	{
		Vector2 targetVelocity = direction * ( MaxSpeedOverride ?? _stats.MaxSpeed );

		float velocityDelta = (float)delta * _stats.AcceleractionCoefficient;
		Velocity = Velocity.MoveToward(targetVelocity, velocityDelta);

		ApplyVelocity();
	}

	public void ApplyVelocity()
	{
		// Only apply non-zero velocities to save on unnecessary physics calculations. 
		if (Velocity != Vector2.Zero || VelocityOverride != null)
		{
			_moveableEntity.Velocity = VelocityOverride ?? Velocity;
			_moveableEntity.MoveAndSlide();
		}
	}
}