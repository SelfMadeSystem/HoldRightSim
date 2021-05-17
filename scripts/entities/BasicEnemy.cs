using Godot;
using Utils;
using System;

public class BasicEnemy : Entity
{
	protected int _hDir = 1;
	public override void UpdateEntityInputs(float delta)
	{
		if (Velocity.x < 0 && _hDir > 0) _hDir = _ground ? -1 : 0;
		if (Velocity.x > 0 && _hDir < 0) _hDir = _ground ? 1 : 0;
		_inputs = new EntityInputs(false, false, _hDir);
	}

	public override Vector2 DoJump(bool forceJump, Vector2 velocity, float delta)
	{
		if (_jumpTime > MaxJumpTime) _jumpExpired = true;
		if (forceJump || !_jumpExpired)
		{
			_jumpTime += delta;
			velocity.y = -MathUtils.Interpolate(StartJump, EndJump, _jumpTime / MaxJumpTime) * delta;
			_jumpExpired = false;
		}
		return velocity;
	}

	public override void DoCollisionStuff(KinematicCollision2D touched)
	{
		base.DoCollisionStuff(touched);
		if (touched == null) return;
		if (_hDir < 0)
		{
			if (touched.Normal.x > 0.7)
			{
				_hDir = 1;
			}
		}
		else if (_hDir > 0)
		{
			if (touched.Normal.x < -0.7)
			{
				_hDir = -1;
			}
		}
	}

	public override void JumpedOn(Entity entity)
	{
		GetParent().RemoveChild(this);
	}
	public override void UpdateAnimation(float delta)
	{
		var animatedSprite = GetNode<AnimatedSprite>("Sprite");
		animatedSprite.SpeedScale = Math.Abs(Velocity.x / (DefMaxSpeed * delta));
		animatedSprite.FlipH = _hDir < 0;
	}
}
