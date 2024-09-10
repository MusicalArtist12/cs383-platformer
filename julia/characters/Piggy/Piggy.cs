// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : CharacterBody2D
{
	private AnimatedSprite2D Sprite;
	private Timer RedFlashTimer;

	[Export]
	private float Accel = 20.0f;
	[Export]
	private float Decel = 10.0f;
	[Export]
	private float WalkSpeed = 200.0f;
	[Export]
	private float SprintSpeed = 400.0f;
	[Export]
	private float JumpVelocity = -200.0f;
	
	[Export]
	public int MaxHealth = 100;
	
	private int Health;
	private float Speed;

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
		Speed = WalkSpeed;
		Health = MaxHealth;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		RedFlashTimer = GetNode<Timer>("RedFlashTimer");
		RedFlashTimer.SetPaused(false);
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Add the gravity.
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

			
			if (Mathf.Abs(velocity.X) > 0)
			{
				Sprite.Play("walk");
				
				Sprite.SpeedScale = Mathf.Abs(velocity.X) / WalkSpeed;
			}
			else
			{	
				Sprite.SpeedScale = 1.0f;
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
