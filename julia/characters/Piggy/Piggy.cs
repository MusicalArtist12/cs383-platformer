// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : CharacterBody2D
{
	private AnimatedSprite2D Sprite;
	private Timer RedFlashTimer;

	public const float Accel = 20.0f;
	public const float Decel = 10.0f;
	public const float WalkSpeed = 300.0f;
	public const float SprintSpeed = 800.0f;
	public const float JumpVelocity = -200.0f;
	
	float Speed = WalkSpeed;

	// As a percentage
	public const int MaxHealth = 100;
	private int Health;

	public int getHealth() 
	{
		return Health;
	}

	public void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.Modulate = new Color(1.0f, 0.0f, 0.0f);
		RedFlashTimer.Start();
	}

    public override void _Ready()
    {
		Health = MaxHealth;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		RedFlashTimer = GetNode<Timer>("RedFlashTimer");
		RedFlashTimer.SetPaused(false);
    }

    public override void _PhysicsProcess(double delta)
	{
		GD.Print(Health);
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
			Speed = SprintSpeed;
		}
		else if (IsOnFloor())
		{
			Speed = WalkSpeed;
		}
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("MainCharacterLeft", "MainCharacterRight", "NullInput", "NullInput");
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

			
			if (velocity.X > 0)
			{
				Sprite.Play("walk");
				
				Sprite.SpeedScale = velocity.X / WalkSpeed;
			}
			else
			{
				Sprite.Play("idle");
			}


		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public void OnRedFlashTimerTimeout()
	{
		Sprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
	}
}
