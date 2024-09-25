// Julia Abdel-Monem

using Godot;
using System;

public partial class WingBullet : CharacterBody2D
{

	public float ImpactPushSpeed;
	public int BulletDamage;

	public override void _PhysicsProcess(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		
		if (collision != null && collision.GetCollider() is Character character)
		{
			character.Velocity = new Vector2(
				character.Velocity.X + ImpactPushSpeed * Mathf.Sign(Velocity.X), 
				character.Velocity.Y + ImpactPushSpeed * Mathf.Sign(Velocity.X)
			);

			character.TakeDamage(BulletDamage);
			
			if (!character.HasWings) {
				Wings wings = (Wings)ResourceLoader.Load<PackedScene>(
					"res:///julia/interactables/Wings/Wings.tscn"
				).Instantiate();
				character.AddChild(wings);
				character.HasWings = true;
			}

			QueueFree();
		}
	
	}

	public void OnDeathTimerTimeout()
	{
		QueueFree();
	}
}

