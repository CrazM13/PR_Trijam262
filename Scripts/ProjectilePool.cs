using Godot;
using System;
using System.Collections.Generic;

public partial class ProjectilePool : Node {

	[Export] private PackedScene projectilePrefab;
	[Export] private int maxObjects = 10;

	private Queue<Node> inactiveObjects;

	public override void _Ready() {
		base._Ready();

		inactiveObjects = new Queue<Node>();

		for (int i = 0; i < maxObjects; i++) {
			Node newObject = projectilePrefab.Instantiate<Node>();
			inactiveObjects.Enqueue(newObject);
		}
	}

	public Node Spawn() {
		if (inactiveObjects.Count > 0) {
			Node newObject = inactiveObjects.Dequeue();

			return newObject;
		} else return null;
	}

	public void Despawn(Node obj) {
		obj.GetParent().RemoveChild(obj);

		inactiveObjects.Enqueue(obj);
	}

}
