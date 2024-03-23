using Godot;
using System;

public partial class Enemy : Damageable {

	[Export] private float speed = 300.0f;
	[Export] private float turnSpeed = 0.75f;

	[Export] private Node2D shield;
	[Export] private Sprite2D sprite;

	private Vector2 direction = Vector2.Right;

	public EnemyManager EnemyManager { get; private set; }

	public void Initialize(EnemyManager manager) {
		EnemyManager = manager;
		direction = (EnemyManager.Target.GlobalPosition - this.GlobalPosition).Normalized();
	}

	public override bool OnDamage(Projectile projectile) {
		EnemyManager.RegisterEnemyDeath(this);
		this.GetParent().RemoveChild(this);

		AudioManager.Instance.PlayGlobal(1); // EnemyDeath

		return false;
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 targetPositon = EnemyManager.Target.GlobalPosition;

		Vector2 targetDirection = (targetPositon - this.GlobalPosition).Normalized();

		direction = direction.Lerp(targetDirection, turnSpeed * (float) delta);

		KinematicCollision2D collision = MoveAndCollide(direction * speed * (float) delta);
		if (collision?.GetCollider() is Player player) {
			player.OnDamage(null);
		}

		UpdateRotation();
	}

	private void UpdateRotation() {
		sprite.LookAt(sprite.GlobalPosition + direction);
		shield.LookAt(EnemyManager.Target.GlobalPosition);
	}

}
