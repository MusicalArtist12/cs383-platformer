// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -200.0f;

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
		

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("MainCharacterLeft", "MainCharacterRight", "NullInput", "NullInput");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walk");
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = velocity.X < 0;

		}
		else
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
