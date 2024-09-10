using Godot;
using System;

public partial class WheelDroid : CharacterBody2D
{
	private Timer ChargeTimer;
	private AnimatedSprite2D Sprite;

	public const float Accel = 5.0f;
	public const float Decel = 5.0f;
	public const float ShotRecoil = 20.0f;
	public const float Speed = 150.0f;
	public Piggy Enemy = null;

	// The distance at which the bot wakes up
	public float AwareDistance = 800;
	
	// The distance at which to shoot
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

				if (Math.Abs(Enemy.Position.X - Position.X) >= ShotDistance && !(Sprite.IsPlaying() && Sprite.GetAnimation() == "shoot")) 
				{
					velocity.X = Mathf.MoveToward(velocity.X, Speed * Math.Sign(Enemy.Position.X - Position.X), Accel);

					Sprite.Play("move");
					ChargeTimer.Stop();
				}
				else 
				{
					velocity.X = Mathf.MoveToward(velocity.X, 0, Decel);

					if (!(Sprite.IsPlaying() && Sprite.GetAnimation() == "shoot"))
					{	
						Sprite.Play("charge");

						if (ChargeTimer.IsStopped()) {
							ChargeTimer.Start();
						}
					}

					if (Sprite.GetAnimation() == "shoot" && Sprite.GetFrame() == 0)
					{
						velocity.X = velocity.X - (ShotRecoil * Math.Sign(Enemy.Position.X - Position.X));
					}

				}
			}
		}
		else if (IsOnFloor() && Position.DistanceTo(Enemy.Position) > AwareDistance) 
		{	
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
		if (Enemy.Position.X - Position.X < AwareDistance && Enemy.Position.Y - Position.Y < 50) 
		{
			Enemy.TakeDamage(20);

		}
	}
}
