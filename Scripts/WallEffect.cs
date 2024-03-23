using Godot;
using System;

public partial class WallEffect : Sprite2D {

	RandomNumberGenerator rng;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		rng = new RandomNumberGenerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

		this.RegionRect = new Rect2(0, rng.RandiRange(0, 100), 30, 2000);
	}
}
