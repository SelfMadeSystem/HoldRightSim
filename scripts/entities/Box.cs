using Godot;
using System;

public class Box : Entity
{
	public override void _Ready()
	{
		base._Ready();
		AddCollisionExceptionWith(GetNode<KinematicBody2D>("Collision"));
	}
}
