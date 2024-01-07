using Godot;
using System;
using System.Diagnostics;

public partial class Player : RigidBody3D
{
	[Export(PropertyHint.Range, "750, 3000, 1.0")]
	public float thrust = 1000.0f;
    [Export]
    public float torque = 100.0f;

	private bool m_isTransitioning = false;
	private AudioStreamPlayer explosionAudio;
	private AudioStreamPlayer winAudio;
	private AudioStreamPlayer3D engineAudio;
	private GpuParticles3D boosterParticles;
    private GpuParticles3D leftboosterParticles;
    private GpuParticles3D rightboosterParticles;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		explosionAudio = GetNode<AudioStreamPlayer>("ExplosionAudio");
        winAudio = GetNode<AudioStreamPlayer>("WinAudio");
        engineAudio = GetNode<AudioStreamPlayer3D>("EngineAudio");
		boosterParticles = GetNode<GpuParticles3D>("BoosterParticles");
        leftboosterParticles = GetNode<GpuParticles3D>("LeftBoosterParticles");
        rightboosterParticles = GetNode<GpuParticles3D>("RightBoosterParticles");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("boost"))
		{
			ApplyCentralForce(MultiplyByFloat(Transform.Basis.Y,  (float)delta * thrust));
			boosterParticles.Emitting = true;
			if(!engineAudio.Playing)
			{
				engineAudio.Play();
			}
		}
		else
		{
            engineAudio.Stop();
			boosterParticles.Emitting = false;
        }

		if (Input.IsActionPressed("rotate_left"))
		{
			ApplyTorque(new Vector3(0, 0, torque * (float)delta));
			rightboosterParticles.Emitting = true;
		}
		else 
		{ rightboosterParticles.Emitting = false;}

        if (Input.IsActionPressed("rotate_right"))
        {
            ApplyTorque(new Vector3(0, 0, -torque * (float)delta));
			leftboosterParticles.Emitting = true;
        }
		else
		{  leftboosterParticles.Emitting = false;}
    }

    private Vector3 MultiplyByFloat(Vector3 left, float right)
    {
        return new Vector3(left.X * right, left.Y * right, left.Z * right);
    }

	public void OnBodyEntered(Node body) 
	{
		if (!m_isTransitioning)
		{
			if (body.IsInGroup("Goal"))
			{
				m_isTransitioning = true;
				CompleteLevel(((LandingPad)body).NextLevelFile);
			}
			else if (body.IsInGroup("Hazard"))
			{
				m_isTransitioning = true;
				CrashSequence();
			}
		}
	}

	private void CrashSequence()
	{
		SetProcess(false);
		explosionAudio.Play();
		boosterParticles.Emitting = false;
		rightboosterParticles.Emitting = false;
		leftboosterParticles.Emitting = false;
		Tween tween = CreateTween();
		tween.TweenInterval(2.5f);
		tween.TweenCallback(Callable.From(GetTree().ReloadCurrentScene));
		tween.Play();
	}

	private void CompleteLevel(string next_level_file)
	{
        SetProcess(false);
		winAudio.Play();
        Tween tween = CreateTween();
        tween.TweenInterval(2.5f);
        tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToFile(next_level_file)));
        tween.Play();
    }
}
