using Godot;
using System;
using System.Diagnostics;

public partial class Player : RigidBody3D
{
	/// <summary>
	/// Text
	/// </summary>
	[Export(PropertyHint.Range, "750, 3000, 1.0")]
	public float thrust = 1000.0f;
    [Export]
    public float torque = 100.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("boost"))
		{
			ApplyCentralForce(MultiplyByFloat(Transform.Basis.Y,  (float)delta * thrust));
		}

		if (Input.IsActionPressed("rotate_left"))
		{
			ApplyTorque(new Vector3(0, 0, torque * (float)delta));
		}
        if (Input.IsActionPressed("rotate_right"))
        {
            ApplyTorque(new Vector3(0, 0, -torque * (float)delta));
        }
    }

    private Vector3 MultiplyByFloat(Vector3 left, float right)
    {
        return new Vector3(left.X * right, left.Y * right, left.Z * right);
    }

	public void OnBodyEntered(Node body) 
	{
		if(body.IsInGroup("Goal"))
		{

		}
		else if(body.IsInGroup("Hazard"))
		{
			CrashSequence();
		}
	}

	public void CrashSequence()
	{
		Debug.Print("Kaboom");
	}
}
