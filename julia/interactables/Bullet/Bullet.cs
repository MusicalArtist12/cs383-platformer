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
			if (collision.GetCollider() is Character)
			{
				Character character = (Character)collision.GetCollider();
				Vector2 velocity = character.Velocity;
				velocity.X -= (HitBack * Math.Sign(character.Position.X - Position.X));
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
