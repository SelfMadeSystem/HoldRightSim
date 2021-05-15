using Godot;
using System;
using Utils;

public class SpecialTiles : TileMap, Collidable
{
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


	public void EntityHit(Entity entity, KinematicCollision2D collision)
	{
		var box = entity.GetNode<CollisionShape2D>("Box");
		var shape = (box.Shape as RectangleShape2D);

		var tiles = TileUtils.GetTileTypes(this, new Rect2(box.GlobalPosition, shape.Extents));
		foreach (var tile in tiles)
		{
			switch (tile)
			{
				case 0:
					{
						entity.AllowJump();
						break;
					}
				case 1:
					{
						entity.Gravity = entity.DefGravity / 4;
						entity.GravityReset = 0.1f;
						if (entity.MaxJumpTime == entity.DefMaxJumpTime) entity.MaxJumpTime = entity.DefMaxJumpTime * 2;
						entity.JumpReset = 0.1f;
						break;
					}
			}
		}
	}


	public void _on_Area2D_area_entered(Area2D area)
	{
		GD.Print(area);
	}
}
