using Godot;
using System;

public partial class WheelDroid : CharacterBody2D
{
	public const float Speed = 300.0f;
	public CharacterBody2D Enemy = null;

	public float AwareDistance = 500;

	bool Awake = false;

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
		GD.Print(Position.DistanceTo(Enemy.Position));
        if (!Awake && IsOnFloor() && Position.DistanceTo(Enemy.Position) < AwareDistance)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wake");
			Awake = true;
		}

		if (Awake) 
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = (Enemy.Position.X - Position.X) < 0;
		}

    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();
		
	}
}
