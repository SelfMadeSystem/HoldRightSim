using Godot;
using System;

public class ColTest : KinematicBody2D
{
	public KinematicCollision2D Test(int maskBit) {
		SetCollisionMaskBit(maskBit, true);
		var collision = MoveAndCollide(new Vector2(0, 0), testOnly: true);
		SetCollisionMaskBit(maskBit, false);
		return collision;
	}
	public KinematicCollision2D Test(int maskBit, Vector2 relVec) {
		SetCollisionMaskBit(maskBit, true);
		var collision = MoveAndCollide(relVec, testOnly: true);
		SetCollisionMaskBit(maskBit, false);
		return collision;
	}
}
