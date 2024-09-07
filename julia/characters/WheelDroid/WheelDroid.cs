using Godot;
using System;

public partial class WheelDroid : CharacterBody2D
{
	public const float Speed = 150.0f;
	public CharacterBody2D Enemy = null;

	public float AwareDistance = 800;
	public float ShotDistance = 400;

	bool Awake = false;

    public override void _Ready()
    {
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");

    }

    public override void _Process(double delta)
    {
        if (!Awake && IsOnFloor() && Position.DistanceTo(Enemy.Position) < AwareDistance)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wake");
		}
		if (Awake && IsOnFloor() && Position.DistanceTo(Enemy.Position) > AwareDistance ) 
		{
			Awake = false;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").PlayBackwards("wake");
		}

		if (Awake) 
		{
				Vector2 position;
				position.Y = 0;
				

			if ((Enemy.Position.X - Position.X) < 0)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
				position.X = -36;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Position = position;
			}
			else 
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
				position.X = 36;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Position = position;
			}
			

		}

    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (Awake)
		{			
			if (Math.Abs(Enemy.Position.X - Position.X) >= ShotDistance) 
			{
				velocity.X = Speed * Math.Sign(Enemy.Position.X - Position.X);
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("move");
			}
			else 
			{
				velocity.X = 0;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("charge");

			}

		}
		else 
		{
			velocity.X = 0;
		}



		Velocity = velocity;
		MoveAndSlide();
		
	}


	public void OnAnimationFinish() 
	{
		
		if (GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetAnimation() == "wake" && !Awake) 
		{
			Awake = true;
		}

	}
}
