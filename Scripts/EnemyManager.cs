using Godot;
using System;

public partial class EnemyManager : Node {

	[Export] public Node2D Target { get; set; }
	[Export] public SceneTransition sceneManager { get; set; }
	[Export] public ProjectilePool explosionPool { get; set; }

	private int enemyCount;

	public override void _Ready() {

		foreach (Node child in GetChildren()) {
			if (child is Enemy enemy) {
				enemyCount++;
				enemy.Initialize(this);
			}
		}

	}

	public void RegisterEnemyDeath(Enemy enemy) {
		enemyCount--;

		if (enemyCount <= 0) sceneManager.ReloadScene(); // Move to new scene

		Node2D newExplosion = explosionPool.Spawn() as Node2D;
		if (newExplosion != null) {
			GetTree().CreateTimer(1f).Timeout += () => {
				explosionPool?.Despawn(newExplosion);
			};
		}
		explosionPool.AddChild(newExplosion);
		newExplosion.GlobalPosition = enemy.GlobalPosition;
	}

}
