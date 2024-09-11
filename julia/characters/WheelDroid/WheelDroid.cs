// Julia Abdel-Monem

using Godot;
using System;

public partial class WheelDroid : Character
{
	private Timer ChargeTimer;
	private AnimatedSprite2D Sprite;
	private Piggy Enemy = null;

	[ExportGroup("Movement")]
	[Export]
	private float Accel = 5.0f;
	[Export]
	private float Decel = 5.0f;
	[Export]
	private float Speed = 150.0f;

	[ExportGroup("Weapon")]
	[Export]
	private float ChargeTime = 1.0f;
	[Export]
	private float BulletSpeed = 1000.0f;
	[Export]
	private float ShotRecoil = 20.0f;
	[Export]
	private int BulletDamage = 10;


	[ExportGroup("AI")]
	[Export]
	private float AwareDistance = 800;
	[Export]
	private float ShotDistance = 400;	
	private bool Awake = false;


	public override void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.Play("damaged");

		if (Health <= 0) 
		{
			Sprite.Play("death");
		}
	}

    public override void _Ready()
    {
		ChargeTimer = GetNode<Timer>("ChargeTimer");
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Health = MaxHealth;
		Sprite.Play("idle");
		ChargeTimer.SetPaused(false);
		
		// Bind to the first possible enemy. Assume singleplayer and that there will only be one Piggy.
		foreach (Node2D child in GetParent().GetChildren()) {
			if (child is Piggy) {
				Enemy = (Piggy)child;
				break;
			}
		}
    }

	public void SetDirection(float dir)
	{
		// this is necessary since the sprite isn't centered in its container
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

	// don't interrupt "shoot" or "damaged"
	private bool CanChangeAnimation()
	{
		return !(
			Sprite.IsPlaying() && 
			(Sprite.GetAnimation() == "shoot" || Sprite.GetAnimation() == "damaged") || Sprite.GetAnimation() == "death"
		);
	}

    public override void _PhysicsProcess(double delta)
	{
		GravityFallDamage(delta);
		Vector2 velocity = Velocity;


       	if (IsOnFloor() && Position.DistanceTo(Enemy.Position) < AwareDistance)
		{
			if (!Awake)
			{
				if (CanChangeAnimation())
				{
					Sprite.Play("wake");
				}
			}
			else 
			{
				SetDirection(Enemy.Position.X - Position.X);

				if (Math.Abs(Enemy.Position.X - Position.X) >= ShotDistance && CanChangeAnimation()) 
				{
					velocity.X = Mathf.MoveToward(
						velocity.X, 
						Speed * Math.Sign(Enemy.Position.X - Position.X), 
						Accel
					);
					if (CanChangeAnimation()) 
					{
						Sprite.Play("move");
					}
					
					ChargeTimer.Stop();
				}
				else 
				{
					velocity.X = Mathf.MoveToward(velocity.X, 0, Decel);

					if (CanChangeAnimation())
					{	
						Sprite.Play("charge");

						if (ChargeTimer.IsStopped()) {
							ChargeTimer.Start(ChargeTime);
						}
					}

				}
			}
		}
		else if (IsOnFloor() && Position.DistanceTo(Enemy.Position) > AwareDistance) 
		{	
			if (Awake)
			{	
				velocity.X = 0;
				if (CanChangeAnimation())
				{
					Sprite.PlayBackwards("wake");
				}
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
		else if (Sprite.GetAnimation() == "death")
		{
			QueueFree();
		}
		else if (Sprite.GetAnimation() == "shoot")
		{
			Velocity = new Vector2(Velocity.X + ShotRecoil * Math.Sign(Position.X - Enemy.Position.X), Velocity.Y);
		}

	}

	public void OnChargeTimerTimeout() 
	{	
		Sprite.Play("shoot");
		Bullet bullet = (Bullet)ResourceLoader.Load<PackedScene>(
			"res:///julia/interactables/Bullet/Bullet.tscn"
		).Instantiate();
		bullet.Position = new Vector2(Position.X + (Sprite.FlipH ? -100 : 100), Position.Y);
		bullet.Velocity = new Vector2(Sprite.FlipH ? -1.0f * BulletSpeed : BulletSpeed, 0.0f);
		bullet.Damage = BulletDamage;
		GetParent().AddChild(bullet);
	}
}
