using Godot;
using System;

public abstract partial class Damageable : CharacterBody2D {

	/// <summary>
	/// The action taken when his by active projectile
	/// </summary>
	/// <param name="projectile">The colliding projectile</param>
	/// <returns>Should bounce proceed</returns>
	public abstract bool OnDamage(Projectile projectile);


}
