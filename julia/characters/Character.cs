using Godot;
using System;

public partial class Character : CharacterBody2D {
    [ExportGroup("Health")]
	[Export]
	public int MaxHealth = 100;
	protected int Health; 
	public bool HasWings = false;

	public AudioStreamPlayer PopStream;

    virtual public int GetHealth() { return Health; }

    virtual public void TakeDamage(int loss) 
	{
		Health -= loss;
	}
	private float FallingVelocity = 0.0f;

    public void GravityFallDamage(double delta) 
    {
		Velocity += GetGravity() * (float)delta;
        if (!IsOnFloor())
		{
			FallingVelocity = Velocity.Y;
		}
		else
		{
			if (FallingVelocity > 500.0f)
			{
				TakeDamage((int)FallingVelocity / 150);
				FallingVelocity = 0.0f;
			}
		}

    }

    public override void _Ready()
    {
        var sound = ResourceLoader.Load<AudioStream>(
			"res://assets/pop.mp3"
		);

		PopStream = new AudioStreamPlayer();
		PopStream.Stream = sound;
		PopStream.VolumeDb = 5;
		PopStream.MaxPolyphony = 100;

		AddChild(PopStream);
    }

    public void EmitPopSound() 
	{
		PopStream.Play(0.2f);
	}

}