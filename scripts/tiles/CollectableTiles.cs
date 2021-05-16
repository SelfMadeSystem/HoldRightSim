using Godot;
using System;
using Utils;

public class CollectableTiles : TileMap, Collidable
{
	public void EntityHit(Entity entity, KinematicCollision2D collision)
	{
		var box = entity.GetNode<CollisionShape2D>("Box");
		var shape = (box.Shape as RectangleShape2D);

		var tiles = TileUtils.GetTileVectors(this, new Rect2(box.GlobalPosition, shape.Extents));
		foreach (var tile in tiles) SetCellv(tile, -1);
	}
}
