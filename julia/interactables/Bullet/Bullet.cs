// Julia Abdel-Monem

using Godot;
using System;


public partial class Bullet : CharacterBody2D
{
	public int Damage;
	public float ImpactPushSpeed;

	public override void _PhysicsProcess(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			if (collision.GetCollider() is Character character)
			{
				Vector2 velocity = character.Velocity;
				velocity.X -= ImpactPushSpeed * Math.Sign(Position.X - character.Position.X);
				character.Velocity = velocity;
				character.TakeDamage(Damage);
			}

			QueueFree();
		}
	}

	public void OnDeathTimerTimeout()
	{
		QueueFree();
	}
}
