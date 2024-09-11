// Julia Abdel-Monem

using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	public int Damage;
	public const float HitBack = 200.0f;
	public override void _PhysicsProcess(double delta)
	{

		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			if (collision.GetCollider() is WheelDroid)
			{
				WheelDroid droid = (WheelDroid)collision.GetCollider();
				Vector2 velocity = droid.Velocity;
				velocity.X -= (HitBack * Math.Sign(droid.Position.X - Position.X));
				droid.Velocity = velocity;
				droid.TakeDamage(Damage);
			}
			else if (collision.GetCollider() is Piggy)
			{
				Piggy piggy = (Piggy)collision.GetCollider();
				Vector2 velocity = piggy.Velocity;
				velocity.X += (HitBack * Math.Sign(piggy.Position.X - Position.X));
				piggy.Velocity = velocity;
				piggy.TakeDamage(Damage);
			}

			QueueFree();
		}
	}

	public void OnDeathTimerTimeout()
	{
		QueueFree();
	}
}
