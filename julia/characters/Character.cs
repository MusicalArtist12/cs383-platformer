using Godot;
using System;

public partial class Character : CharacterBody2D {
    [ExportGroup("Health")]
	[Export]
	public int MaxHealth = 100;
	protected int Health; 

    virtual public int GetHealth() { return Health; }

    virtual public void TakeDamage(int loss) 
	{
		Health -= loss;
	}

}