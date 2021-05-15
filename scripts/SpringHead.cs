using Godot;
using System;

public class SpringHead : KinematicBody2D, Collidable
{
	public void EntityHit(Entity entity, KinematicCollision2D collision)
	{
		GetParent<Collidable>().EntityHit(entity, collision);
	}
}
