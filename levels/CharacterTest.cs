// Julia Abdel-Monem

using Godot;
using System;

public partial class CharacterTest : Node2D
{
	
    public override void _Ready()
    {
		CharacterBody2D character = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://characters/piggy.tscn").Instantiate();
		character.Position = new Vector2(500, 0);


		AddChild(character);
    }
}
