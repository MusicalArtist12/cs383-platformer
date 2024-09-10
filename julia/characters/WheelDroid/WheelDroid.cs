// Julia Abdel-Monem

using Godot;
using System;

public partial class WheelDroid : CharacterBody2D
{
	private Timer ChargeTimer;
	private AnimatedSprite2D Sprite;
	private Piggy Enemy = null;

	[Export]
	private float BulletSpeed = 1000.0f;
	[Export]
	private float Accel = 5.0f;
	[Export]
	private float Decel = 5.0f;
	[Export]
	private float ShotRecoil = 20.0f;
	[Export]
	private float Speed = 150.0f;
	[Export]
	// The distance at which the bot wakes up
	private float AwareDistance = 800;
	[Export]
	// The distance at which to shoot
	private float ShotDistance = 400;
	[Export]
	public int MaxHealth = 100;

	bool Awake = false;

	private int Health;

	public int getHealth() 
	{
		return Health;
	}

	public void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.Play("damaged");
	}

    public override void _Ready()
    {
		ChargeTimer = GetNode<Timer>("ChargeTimer");
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		Sprite.Play("idle");
		ChargeTimer.SetPaused(false);
		
		// Bind to the first possible enemy. Assuming singleplayer and that there will only be one Piggy.
		foreach (Node2D child in GetParent().GetChildren()) {
			if (child is Piggy) {
				Enemy = (Piggy)child;
				break;
			}
		}
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

					if (!(Sprite.IsPlaying() && (Sprite.GetAnimation() == "shoot" || Sprite.GetAnimation() == "damaged")))
					{	
						Sprite.Play("charge");

						if (ChargeTimer.IsStopped()) {
							ChargeTimer.Start();
						}
					}

					if (Sprite.GetAnimation() == "shoot" && Sprite.GetFrame() == 0)
					{
						velocity.X -= (ShotRecoil * Math.Sign(Enemy.Position.X - Position.X));
					}

				}
			}
		}
		else if (IsOnFloor() && Position.DistanceTo(Enemy.Position) > AwareDistance) 
		{	
			if (Awake)
			{	
				velocity.X = 0;
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
		Bullet bullet = (Bullet)ResourceLoader.Load<PackedScene>("res:///julia/characters/WheelDroid/Bullet.tscn").Instantiate();
		Vector2 position = Position;
		position.X += Sprite.FlipH ? -100 : 100;

		bullet.Position = position;

		bullet.Velocity = new Vector2(Sprite.FlipH ? -1.0f * BulletSpeed : BulletSpeed, 0.0f);
		GetParent().AddChild(bullet);
	}
}
