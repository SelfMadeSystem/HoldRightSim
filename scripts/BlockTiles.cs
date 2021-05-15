using Godot;
using System;
using Utils;

public class BlockTiles : TileMap, Collidable
{
	public void EntityHit(Entity entity, KinematicCollision2D collision)
	{
		// var box = player.GetNode<CollisionShape2D>("Box");
		// var shape = (box.Shape as RectangleShape2D);
		var w2m = WorldToMap(collision.Position - collision.Normal);
		var cell = GetCellv(w2m);

		if (collision.Normal.y < -0.1)
		{
			switch (cell)
			{
				case -1: break;
				case 24:
					{
						entity.ResetJump();
						entity.ResetSpeed();
						entity.Accel = (int)(entity.DefAccel * 0.05f);
						entity.RunAccel = (int)(entity.DefRunAccel * 0.25f);
						entity.GroundFriction = entity.DefGroundFriction * 0.05f;
						entity.MaxRunSpeed = (int)(entity.DefMaxRunSpeed * 1.25f);
						entity.ResetAccelAir = entity.ResetSpeedAir = entity.ResetGroundFrictionAir = true;
						break;
					}
				case 25:
					{
						entity.ResetSpeed();
						entity.StartJump = 8400;
						entity.EndJump = 1000;
						entity.MaxJumpTime = 0.2f;
						entity.MaxSpeed = (int)(entity.DefMaxSpeed * 0.5f);
						entity.MaxRunSpeed = (int)(entity.DefMaxRunSpeed * 0.35f);
						break;
					}
				default:
					{
						entity.ResetSpeed();
						entity.ResetJump();
						break;
					}
			}
		}
	}

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
