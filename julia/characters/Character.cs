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
	private float FallingVelocity = 0.0f;

    public void GravityFallDamage(double delta) 
    {
        if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
			FallingVelocity = Velocity.Y;
		}
		else
		{
			if (FallingVelocity > 500.0f)
			{
				TakeDamage((int)FallingVelocity / 100);
				FallingVelocity = 0.0f;
			}
		}

    }

}