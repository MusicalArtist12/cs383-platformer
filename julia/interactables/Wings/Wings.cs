using Godot;
using System;

public partial class Wings : AnimatedSprite2D
{

	private float Speed = 75.0f;
	private float DeltaSpeed = 75.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (GetParent() is Character character)
		{
			character.Velocity = new Vector2(character.Velocity.X, character.Velocity.Y - Speed);

			Speed -= DeltaSpeed * (float)delta;

			if (Speed <= 0 || character.GetHealth() == 0)
			{
				QueueFree();
				character.HasWings = false;
			}
		}
	}

}
