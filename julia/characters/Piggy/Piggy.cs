// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : CharacterBody2D
{
	// References to commonly used child nodes
	private AnimatedSprite2D Sprite;
	private Timer RedFlashTimer;

	[ExportGroup("Movement")]
	[Export]
	private float Accel = 20.0f; // The rate at which the character increases speed
	[Export]
	private float Decel = 10.0f; // The rate at which the character stops moving
	[Export]
	private float WalkSpeed = 200.0f; 
	[Export]
	private float SprintSpeed = 400.0f;
	[Export]
	private float JumpVelocity = -200.0f;
	private float Speed;
	
	[ExportGroup("Health")]
	[Export]
	public int MaxHealth = 100;
	[Export]
	public float RedFlashTime = 0.5f;
	private int Health;

	public int GetHealth() { return Health; }

	public void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.Play("damaged");
	}

    public override void _Ready()
    {	
		Speed = WalkSpeed;
		Health = MaxHealth;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		if (Input.IsActionJustPressed("MainCharacterJump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionPressed("MainCharacterSprint") && IsOnFloor()) 
		{
			Speed = SprintSpeed;
		}
		else if (IsOnFloor())
		{
			Speed = WalkSpeed;
		}
		
		Vector2 direction = Input.GetVector(
			"MainCharacterLeft", 
			"MainCharacterRight", 
			"NullInput", 
			"NullInput"
		);
		if (IsOnFloor()) 
		{
			if (direction != Vector2.Zero)
			{
				Sprite.FlipH = direction.X < 0;
				velocity.X = Mathf.MoveToward(velocity.X, direction.X * Speed, Accel);
			}
			else 
			{
				velocity.X = Mathf.MoveToward(velocity.X, 0, Decel);
			}

			if (Mathf.Abs(velocity.X) > 0)
			{
				if (!(Sprite.IsPlaying() && Sprite.GetAnimation() == "damaged"))
				{
					Sprite.Play("walk");
				}
				
				
				Sprite.SpeedScale = Mathf.Abs(velocity.X) / WalkSpeed;
			}
			else
			{	
				Sprite.SpeedScale = 1.0f;
				if (!(Sprite.IsPlaying() && Sprite.GetAnimation() == "damaged")) 
				{
					Sprite.Play("idle");
				}
				
			}
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

}
