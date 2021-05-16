using Godot;
using System;

public class Spring : Entity, Collidable
{
	private float _sproingTime;
	public override void UpdateAnimation(float delta)
	{
		var sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		sprite.Animation = _sproingTime < 0 ? "default" : _sproingTime < 0.15f || _sproingTime > 0.2f ? "half" : "full";
		_sproingTime -= delta;
	}

	public override void EntityHit(Entity entity, KinematicCollision2D collision)
	{
		entity.ResetJumpFinishJump = true;
		entity.MaxJumpTime = entity.DefMaxJumpTime * 1.25f;
		entity.StartJump = (int)(entity.DefStartJump * 1.25f);
		_sproingTime = 0.25f;
		entity.ForceJump(entity is Spring ? 0.1f : 0.05F);
	}
}
