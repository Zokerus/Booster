using Godot;
using System;

public partial class Player : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_accept"))
		{
			ApplyCentralForce(MultiplyByFloat(Vector3.Up,  (float)delta * 1000));
		}

		if (Input.IsActionPressed("ui_left"))
		{
			ApplyTorque(new Vector3(0, 0, 100 * (float)delta));
		}
        if (Input.IsActionPressed("ui_right"))
        {
            ApplyTorque(new Vector3(0, 0, -100 * (float)delta));
        }
    }

    private Vector3 MultiplyByFloat(Vector3 left, float right)
    {
        return new Vector3(left.X * right, left.Y * right, left.Z * right);
    }
}
