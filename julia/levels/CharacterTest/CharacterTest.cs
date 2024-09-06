// Julia Abdel-Monem

using Godot;
using System;

public partial class CharacterTest : Node2D
{
	
    public override void _Ready()
    {
		CharacterBody2D Character = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://julia/characters/Piggy/Piggy.tscn").Instantiate();
		Character.Position = new Vector2(500, 0);

    WheelDroid Droid = (WheelDroid)ResourceLoader.Load<PackedScene>("res://julia/characters/WheelDroid/WheelDroid.tscn").Instantiate();
    Droid.Position = new Vector2(1200, 0);

    Droid.Enemy = Character;

		AddChild(Character);
    AddChild(Droid);
    }
}
