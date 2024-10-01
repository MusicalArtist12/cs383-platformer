// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : Character
{
	// References to commonly used child nodes
	private AnimatedSprite2D Sprite;
	private Timer GunReloadTimer;

	private Sprite2D WingGun;

	[Signal]
    public delegate void PiggyKilledEventHandler();

	// all units: distance: cm, time: s
	[ExportGroup("Movement")]
	[Export]
	private float Accel = 10.0f; // The rate at which the character increases speed
	[Export]
	private float Decel = 50.0f; // The rate at which the character stops moving
	[Export]
	private float WalkSpeed = 250.0f; 
	[Export]
	private float SprintSpeed = 500.0f;
	[Export]
	private float JumpVelocity = -100.0f;
	private float Speed;
	
	[Export]
	private float AirSpeedMultiplier = 0.2f;

	[ExportGroup("Weapon")]
	[Export]
	private float BulletSpeed = 980.0f;
	[Export]
	private float ShotRecoil = 500.0f;
	[Export]
	private float ImpactPushSpeed = 500.0f;
	[Export]
	private int BulletDamage = 10;

	private int XDirection = 1;
	private bool reloading = false;
	
	public override void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.SpeedScale = 1.0f;
		Sprite.Play("damaged");

		if (Health <= 0)
		{
			EmitSignal(SignalName.PiggyKilled);
		}
	}	

    public override void _Ready()
    {	
		Speed = WalkSpeed;
		Health = MaxHealth;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		WingGun = GetNode<Sprite2D>("WingGun");
		GunReloadTimer = GetNode<Timer>("GunReloadTimer");

		base._Ready();
    }


    public override void _PhysicsProcess(double delta)
	{
		GravityFallDamage(delta);
		Vector2 velocity = Velocity;


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
		
		Vector2 MousePosition = GetLocalMousePosition();
		WingGun.Rotation = MousePosition.Angle();

		if (MousePosition.X < 0)
		{
			Scale = new Vector2(-Mathf.Abs(Scale.X), Scale.Y);
			XDirection = XDirection * -1;
		}

		if (Input.IsActionJustPressed("MainCharacterShoot"))
		{
			ShootWeapon(MousePosition.Normalized());
			velocity.X -= XDirection * ShotRecoil;

		}

		Vector2 direction = Input.GetVector(
			"MainCharacterLeft", 
			"MainCharacterRight", 
			"NullInput", 
			"NullInput"
		);
		if (IsOnFloor()) 
		{
			if (direction != Vector2.Zero)
			{
				
				velocity.X = Mathf.MoveToward(velocity.X, direction.X * Speed, Accel);
			}
			else 
			{
				velocity.X = Mathf.MoveToward(velocity.X, 0, Decel);
			}
		}
		else {
			if (direction != Vector2.Zero)
			{
				velocity.X = Mathf.MoveToward(velocity.X, direction.X * Speed, Accel * AirSpeedMultiplier);
			}
		}
		
		if (Mathf.Abs(velocity.X) > 0)
		{
			if (!(Sprite.IsPlaying() && Sprite.GetAnimation() == "damaged"))
			{
				Sprite.Play("walk");
			}
			
			Sprite.SpeedScale = Mathf.Abs(velocity.X) / WalkSpeed;
		}
		else
		{	
			Sprite.SpeedScale = 1.0f;
			if (!(Sprite.IsPlaying() && Sprite.GetAnimation() == "damaged")) 
			{
				Sprite.Play("idle");
			}	
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	void ShootWeapon(Vector2 Direction)
	{
		if (reloading) {
			return;
		}
		
		WingBullet bullet = (WingBullet)ResourceLoader.Load<PackedScene>(
			"res:///julia/interactables/WingBullet/WingBullet.tscn"
		).Instantiate();

		base.EmitPopSound();
		
		Vector2 AbsDirection = new Vector2(Direction.X * XDirection, Direction.Y);

		bullet.Position = new Vector2(Position.X + (AbsDirection.X), (Position.Y + 8)+ (AbsDirection.Y) );
		bullet.Rotation = AbsDirection.Angle();
		bullet.Velocity = BulletSpeed * AbsDirection;
		bullet.ImpactPushSpeed = ImpactPushSpeed;
		bullet.BulletDamage = BulletDamage;
		bullet.AddCollisionExceptionWith(this);
		GetParent().AddChild(bullet);
		GunReloadTimer.Start();
		reloading = true;
	}

	void OnGunReloadTimerTimeout() 
	{
		reloading = false;
	}
}
