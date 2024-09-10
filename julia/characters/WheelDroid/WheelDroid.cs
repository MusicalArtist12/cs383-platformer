using Godot;
using System;

public partial class WheelDroid : CharacterBody2D
{
	private Timer ChargeTimer;
	private AnimatedSprite2D Sprite;

	public const float Speed = 150.0f;
	public CharacterBody2D Enemy = null;

	public float AwareDistance = 800;
	public float ShotDistance = 400;

	bool Awake = false;

    public override void _Ready()
    {
		ChargeTimer = GetNode<Timer>("ChargeTimer");
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		Sprite.Play("idle");
		ChargeTimer.SetPaused(false);
		
    }

	public void Sleep()
	{
		Sprite.PlayBackwards("wake");
		
	}

	public void WakeUp()
	{
		Sprite.Play("wake");
	}

	public void SetDirection(float dir)
	{
		Vector2 position;
		position.Y = 0;
		if (dir > 0)
		{
			Sprite.FlipH = false;
			position.X = 36;
			Sprite.Position = position;
		}
		else if (dir < 0)
		{
			Sprite.FlipH = true;
			position.X = -36;
			Sprite.Position = position;
		}
	}

    public override void _Process(double delta) {}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

       	if (IsOnFloor() && Position.DistanceTo(Enemy.Position) < AwareDistance)
		{
			if (!Awake)
			{
				WakeUp();
			}
			else 
			{
				SetDirection(Enemy.Position.X - Position.X);
					

				if (Math.Abs(Enemy.Position.X - Position.X) >= ShotDistance) 
				{
					velocity.X = Speed * Math.Sign(Enemy.Position.X - Position.X);
					Sprite.Play("move");
					ChargeTimer.Stop();
				}
				else 
				{
					velocity.X = 0;
					if (!(Sprite.IsPlaying() && Sprite.GetAnimation() == "shoot"))
					{	
						Sprite.Play("charge");
						GD.Print(ChargeTimer.GetTimeLeft());

						if (ChargeTimer.IsStopped()) {
							ChargeTimer.Start();
						}
					}
				}
			}
		}
		else if (IsOnFloor() && Position.DistanceTo(Enemy.Position) > AwareDistance) 
		{	
			velocity.X = 0;
			if (Awake)
			{
				Sleep();
			}
			else 
			{
				Sprite.Play("idle");
			}
		}
	
		Velocity = velocity;
		MoveAndSlide();
	}


	public void OnAnimationFinish() 
	{
		if (Sprite.GetAnimation() == "wake" && !Awake) 
		{
			Awake = true;
		}
		else if (Sprite.GetAnimation() == "wake" && Awake) 
		{
			Awake = false;
		}

	}

	public void OnChargeTimerTimeout() 
	{	
		Sprite.Play("shoot");
	}
}
