// Julia Abdel-Monem

using Godot;
using System;

public partial class CharacterTest : Node2D
{
	
    public override void _Ready()
    {
        GetNode<WheelDroid>("WheelDroid").Enemy = GetNode<Piggy>("Piggy");
    }
}
