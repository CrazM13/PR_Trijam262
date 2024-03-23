using Godot;
using System;

public partial class Player : Damageable {
	[Export] private float Speed = 300.0f;

	[Export] private SceneTransition sceneManager;
	[Export] public ProjectilePool explosionPool { get; set; }

	[Export] private Sprite2D tankBase;
	[Export] private Sprite2D tankGun;
	[Export] private ProjectilePool ammo;

	private bool controllable = true;

	public override void _Ready() {
		Engine.TimeScale = 1;
	}

	public override void _PhysicsProcess(double delta) {
		if (!controllable) return;

		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero) {
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float) delta);
		if (collision != null) {
			OnDamage(null);
		}

		UpdateRotation();
		if (Input.IsActionJustPressed("fire")) {
			Fire();
		}
	}

	private void UpdateRotation() {
		tankBase.LookAt(tankBase.GlobalPosition + Velocity);
		tankGun.LookAt(GetViewport().GetMousePosition());
	}

	private void Fire() {
		AudioManager.Instance.PlayGlobal(3);

		if (ammo.Spawn() is Projectile newProjectile) {
			Vector2 mousePos = GetViewport().GetMousePosition();

			newProjectile.GlobalPosition = tankGun.GlobalPosition;
			newProjectile.Direction = (mousePos - tankGun.GlobalPosition).Normalized();

			AddSibling(newProjectile);
		}
	}

	public override bool OnDamage(Projectile projectile) {
		sceneManager.ReloadScene(0.1f);

		if (projectile != null) projectile.CollisionMask -= 1;

		AudioStreamPlayer audio = AudioManager.Instance.PlayGlobal(2); // PlayerDeath

		Engine.TimeScale = 0.1f;
		sceneManager.SpeedScale = 1f / (float)Engine.TimeScale;

		controllable = false;

		Node2D newExplosion = explosionPool.Spawn() as Node2D;
		if (newExplosion != null) {
			GetTree().CreateTimer(1f).Timeout += () => {
				if (IsInstanceValid(newExplosion)) explosionPool?.Despawn(newExplosion);
			};
		}
		explosionPool.AddChild(newExplosion);
		newExplosion.GlobalPosition = GlobalPosition;

		return false;
	}
}
