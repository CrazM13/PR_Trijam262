using Godot;
using System;

public partial class Projectile : AnimatableBody2D {

	[Export] private float speed;
	[Export] private float inertModifier;
	[Export] private AnimatedSprite2D sprite;

	public bool Active { get; private set; } = false;

	public Vector2 Direction { get; set; }

	public override void _PhysicsProcess(double delta) {
		KinematicCollision2D collision = MoveAndCollide(Direction * GetSpeed() * (float) delta);
		if (collision != null) {
			if (!Active) {
				this.Active = true;
				sprite.Scale = new Vector2(0.5f, 0.5f);
				sprite.Animation = "active";
				CollisionMask += 1;
			}

			if (collision.GetCollider() is Damageable damagableObj) {
				if (damagableObj.OnDamage(this)) {
					Direction = -Direction.Reflect(collision.GetNormal());
					AudioManager.Instance.PlayGlobal(0); // Bounce
				}
			} else {
				Direction = -Direction.Reflect(collision.GetNormal());
				AudioManager.Instance.PlayGlobal(0); // Bounce
			}
		}



	}

	private float GetSpeed() {
		return Active ? speed : speed * inertModifier;
	}

}
