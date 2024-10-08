// Julia Abdel-Monem

using Godot;
using System;

public partial class WheelDroid : Character
{
	private Timer ChargeTimer;
	private Timer SleepTimer;
	private Timer AwareTimer;
	private AnimatedSprite2D Sprite;
	private Piggy Enemy = null;

	// all units: distance: cm, time: s
	[ExportGroup("Movement")]
	[Export]
	private float Accel = 25.0f;
	[Export]
	private float Decel = 50.0f;
	[Export]
	private float Speed = 250.0f;

	[ExportGroup("Weapon")]
	[Export]
	private float ChargeTime = 1.0f;
	[Export]
	private float BulletSpeed = 980.0f;
	[Export]
	private float ShotRecoil = 500.0f;
	[Export]
	private float ImpactPushSpeed = 500.0f;
	[Export]
	private int BulletDamage = 10;


	[ExportGroup("AI")]
	[Export]
	private float AwareDistance = 800;
	[Export]
	private float ShotDistance = 400;	
	[Export]

	private bool Awake = false;
	private bool Aware = false;

	private bool HasDied = false;

	public void WakeUp()
	{
		if (CanChangeAnimation())
		{
			Sprite.Play("wake");
		}


	}

	public void Sleep()
	{
		if (CanChangeAnimation())
		{
			Sprite.Play("sleep");
			Awake = false;
		}
	}

	public void Die() 
	{
		Sprite.Play("death");
		Node global = GetTree().Root.GetNode<Node>("Global");
		
		if (!HasDied) {
			global.Call("add_coin", 10);
		}

		HasDied = true;
	}

	public override void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.Play("damaged");

		if (Health <= 0) 
		{
			Die();
		}
	}

    public override void _Ready()
    {
		ChargeTimer = GetNode<Timer>("ChargeTimer");
		SleepTimer = GetNode<Timer>("SleepTimer");
		AwareTimer = GetNode<Timer>("AwareTimer");
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Health = MaxHealth;
		Sprite.Play("sleepIdle");
		ChargeTimer.SetPaused(false);
		SleepTimer.SetPaused(false);
		
		// Bind to the first possible enemy. Assume singleplayer and that there will only be one Piggy.
		foreach (Node2D child in GetParent().GetChildren()) {
			if (child is Piggy) {
				Enemy = (Piggy)child;
				break;
			}
		}

		base._Ready();
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
			(Sprite.GetAnimation() == "shoot" || Sprite.GetAnimation() == "damaged" || Sprite.GetAnimation() == "death") &&
			!HasDied
		);
	}

    public override void _PhysicsProcess(double delta)
	{
		GravityFallDamage(delta);
		Vector2 velocity = Velocity;

		// make sure that things stay dead.
		if (HasDied) {
			Die();
		}

		if (IsOnFloor() && Position.DistanceTo(Enemy.Position) < AwareDistance)
		{
			if (!Awake)
			{
				WakeUp();
			}

		}

		if (Awake && IsOnFloor() && Aware)
		{
			SetDirection(Enemy.Position.X - Position.X);

			// Move towards enemy & Shoot when close enough
			if (Math.Abs(Enemy.Position.X - Position.X) >= ShotDistance && CanChangeAnimation()) 
			{
				velocity.X = Mathf.MoveToward(
					velocity.X, 
					Speed * Math.Sign(Enemy.Position.X - Position.X), 
					Accel
				);
				Sprite.Play("move");
				
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
		else 
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Decel);
		}

		
		Velocity = velocity;
		MoveAndSlide();
	}

	public void OnAnimationChange() 
	{
		if (Sprite.GetAnimation() == "shoot")
		{
			Velocity = new Vector2(Velocity.X + ShotRecoil * Math.Sign(Position.X - Enemy.Position.X), Velocity.Y);
		}
	}

	public void OnAnimationFinish() 
	{
		if (Sprite.GetAnimation() == "wake") 
		{
			Sprite.Play("wakeIdle");
			AwareTimer.Start();
			Aware = true;
			Awake = true;
		}
		else if (Sprite.GetAnimation() == "sleep") 
		{
			Sprite.Play("sleepIdle");
		}
		else if (Sprite.GetAnimation() == "death")
		{
			QueueFree();
		}
		
		else if (Sprite.GetAnimation() == "damaged" && !Awake)
		{
			Sprite.Play("wakeIdle");
			AwareTimer.Start();
			Aware = true;
			Awake = true;
		}

	}

	public void ShootWeapon()
	{
		Sprite.Play("shoot");
		Bullet bullet = (Bullet)ResourceLoader.Load<PackedScene>(
			"res:///julia/interactables/Bullet/Bullet.tscn"
		).Instantiate();
		
		bullet.Position = new Vector2(Position.X, Position.Y - 8);
		bullet.Velocity = new Vector2(Sprite.FlipH ? -1.0f * BulletSpeed : BulletSpeed, 0.0f);
		bullet.Damage = BulletDamage;
		bullet.ImpactPushSpeed = ImpactPushSpeed;

		bullet.AddCollisionExceptionWith(this);
		GetParent().AddChild(bullet);

		base.EmitPopSound();
	}

	public void OnChargeTimerTimeout() 
	{	
		ShootWeapon();
	}

	public void OnSleepTimerTimeout()
	{
		Sleep();
	}

	public void OnAwareTimerTimeout()
	{
		AwareTimer.Stop();
		Aware = false;
		if (SleepTimer.IsStopped())
		{
			SleepTimer.Start();
		}
		if (CanChangeAnimation())
		{
			Sprite.Play("wakeIdle");
		}

	}
}
