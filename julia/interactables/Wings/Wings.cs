using Godot;
using System;

public partial class Wings : AnimatedSprite2D
{

	private float Speed = 50.0f;
	private float DeltaSpeed = 1.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (GetParent() is Character)
		{
			Character character = (Character)GetParent();
			character.Velocity = new Vector2(character.Velocity.X, character.Velocity.Y - Speed);

			Speed -= DeltaSpeed;

			if (Speed <= 0)
			{
				QueueFree();
			}
		}
	}

}
