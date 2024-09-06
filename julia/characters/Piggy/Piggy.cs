// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : CharacterBody2D
{
	public const float WalkSpeed = 300.0f;
	public const float SprintSpeed = 800.0f;
	public const float JumpVelocity = -600.0f;
	
	// As a percentage
	public const int MaxHealth = 100;
	private int Health;

	float Speed = WalkSpeed;

	public int getHealth() 
	{
		return Health;
	}

	public void takeDamage(int loss) 
	{
		Health -= loss;
	}

    public override void _Ready()
    {
		Health = MaxHealth;
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// This will be removed/modified later
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}


		if (Input.IsActionPressed("MainCharacterSprint") && IsOnFloor()) 
		{
			Speed = Mathf.MoveToward(Speed, SprintSpeed, Speed);
		}
		else if (IsOnFloor())
		{
			Speed = Mathf.MoveToward(Speed, WalkSpeed, Speed);
		}
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("MainCharacterLeft", "MainCharacterRight", "NullInput", "NullInput");
		if (IsOnFloor()) 
		{
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walk");
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = velocity.X < 0;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").SpeedScale = Speed / WalkSpeed;

			}
			else
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
