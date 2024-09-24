using Godot;
using System;

public partial class ScoreCounter : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = GetTree().Root.GetNode<Piggy>("Level1/Critters/Piggy").GetHealth().ToString();
	}
}
