using Godot;
using System;

public partial class moving_hazard : AnimatableBody3D
{
	[Export]
	public Vector3 direction;

	[Export]
	public float duration = 1.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Tween tween = GetTree().CreateTween();
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(this, "position", Position + direction, duration).SetDelay(1);
		tween.TweenProperty(this, "position", Position, duration).SetDelay(1);
		tween.SetLoops();
        tween.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
