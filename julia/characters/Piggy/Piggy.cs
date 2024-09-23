// Julia Abdel-Monem

using Godot;
using System;

public partial class Piggy : Character
{
	// References to commonly used child nodes
	private AnimatedSprite2D Sprite;
	private Timer RedFlashTimer;
	private Sprite2D WingGun;

	[ExportGroup("Movement")]
	[Export]
	private float Accel = 20.0f; // The rate at which the character increases speed
	[Export]
	private float Decel = 10.0f; // The rate at which the character stops moving
	[Export]
	private float WalkSpeed = 200.0f; 
	[Export]
	private float SprintSpeed = 400.0f;
	[Export]
	private float JumpVelocity = -200.0f;
	private float Speed;

	[ExportGroup("Weapon")]
	[Export]
	private float BulletSpeed = 1000.0f;
	[Export]
	private float ShotRecoil = 100.0f;
	[Export]
	private float ImpactPushSpeed = 100.0f;
	[Export]
	private int BulletDamage = 10;

	private int XDirection = 1;
	
	public override void TakeDamage(int loss) 
	{
		Health -= loss;
		Sprite.SpeedScale = 1.0f;
		Sprite.Play("damaged");

	}

    public override void _Ready()
    {	
		Speed = WalkSpeed;
		Health = MaxHealth;
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		WingGun = GetNode<Sprite2D>("WingGun");
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
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	void ShootWeapon(Vector2 Direction)
	{
		WingBullet bullet = (WingBullet)ResourceLoader.Load<PackedScene>(
			"res:///julia/interactables/WingBullet/WingBullet.tscn"
		).Instantiate();
		
		Vector2 AbsDirection = new Vector2(Direction.X * XDirection, Direction.Y);

		bullet.Position = new Vector2(Position.X + (300.0f * AbsDirection.X), Position.Y + (300.0f * AbsDirection.Y));
		bullet.Rotation = AbsDirection.Angle();
		bullet.Velocity = BulletSpeed * AbsDirection;
		bullet.ImpactPushSpeed = ImpactPushSpeed;
		bullet.BulletDamage = BulletDamage;
		GetParent().AddChild(bullet);
	}

}
