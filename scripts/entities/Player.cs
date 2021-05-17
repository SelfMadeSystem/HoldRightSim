using Godot;
using Utils;
using System;
using System.Collections.Generic;

public class Player : Entity
{ // Todo: Set friction, accel, min & max speed, start & end jump, max jump time, etc.
	[Export]
	public int Mass = 32;
	[Export]
	public float FloatDelay = 0.1f;
	[Export]
	public float LookStopTime = 5;
	[Export]
	public bool NoControl = false;

	//  public override void _Process(float delta)
	// public override void _PhysicsProcess(float delta)
	// {
	// 	UpdateEntityInputs(delta);
	// 	Vector2 velocity = Velocity;
	// 	var groundTest = MoveAndCollide(new Vector2(0, 0.1f), testOnly: true);
	// 	if (!_ground || groundTest == null)
	// 	{
	// 		velocity.y += Gravity * delta;
	// 	}
	// 	float speedUp = (_inputs.r ? RunAccel : Accel) * delta;
	// 	float maxSpeed = (_inputs.r ? MaxRunSpeed : MaxSpeed) * delta;
	// 	switch (_inputs.h)
	// 	{
	// 		case -1:
	// 			{
	// 				if (velocity.x >= -maxSpeed)
	// 				{
	// 					if (velocity.x > 0) velocity = ApplyFriction(velocity, delta);
	// 					velocity.x = Math.Max(-maxSpeed, velocity.x - speedUp);
	// 				}
	// 				else if (_ground) velocity = ApplyFriction(velocity, delta);
	// 				break;
	// 			}
	// 		case 1:
	// 			{
	// 				if (velocity.x <= maxSpeed)
	// 				{
	// 					if (velocity.x < 0) velocity = ApplyFriction(velocity, delta);
	// 					velocity.x = Math.Min(maxSpeed, velocity.x + speedUp);
	// 				}
	// 				else if (_ground) velocity = ApplyFriction(velocity, delta);
	// 				break;
	// 			}
	// 	}

	// 	if (_ground)
	// 	{
	// 		_jumpExpired = false;

	// 	}
	// 	if (_inputs.h == 0)
	// 	{
	// 		velocity = ApplyFriction(velocity, delta);
	// 	}

	// 	var forceJump = _forceJump > 0;

	// 	if (forceJump || _inputs.j)
	// 	{
	// 		if (_jumpTime > MaxJumpTime) _jumpExpired = true;
	// 		if (forceJump || _canJump || !_jumpExpired)
	// 		{
	// 			_jumpTime += delta;
	// 			if (_canJump) _jumpTime = 0;
	// 			velocity.y = -MathUtils.Interpolate(StartJump, EndJump, _jumpTime / MaxJumpTime) * delta;
	// 			_jumpExpired = false;
	// 		}
	// 		else if (!_ground)
	// 		{
	// 			if (!_prevInputs.j && _floatTime >= FloatDelay)
	// 			{
	// 				if (velocity.y > 0)
	// 				{
	// 					velocity.y = float.Epsilon;
	// 					_floatTime = 0;
	// 					_jumpTime = MaxJumpTime + 1;
	// 				}
	// 			}
	// 		}
	// 	}
	// 	else
	// 	{
	// 		_jumpTime = 0;
	// 		_jumpExpired = true;
	// 	}

	// 	_forceJump -= delta;

	// 	_floatTime += delta;

	// 	foreach (var area in _collisions)
	// 	{
	// 		var g = GlobalPosition.x - area.GlobalPosition.x;

	// 		velocity.x += Math.Min(20, Math.Max(-20, 1 / g * delta * 4000));
	// 		if (Math.Abs(g) <= 3)
	// 		{
	// 			if (g < 0)
	// 			{
	// 				velocity.x = Math.Min(velocity.x, 0);
	// 			}
	// 			else if (g > 0)
	// 			{
	// 				velocity.x = Math.Max(velocity.x, 0);
	// 			}
	// 			velocity.x += g * delta * 500;
	// 		}
	// 	}

	// 	PreMove(velocity, delta);

	// 	var newVel = MoveAndSlide(velocity, Vector2.Up, false, 2, 0.785398f, false);

	// 	if (newVel.y != 0 && newVel.y != velocity.y)
	// 	{
	// 		if (velocity.y < 0) _jumpTime = MaxJumpTime + 1;
	// 		newVel.x = velocity.x;
	// 	}

	// 	if (Velocity.y < 0 && newVel.y == 0 && _inputs.j) _jumpTime = MaxJumpTime + 1;
	// 	_ground = (_ground && newVel.y == 0) || (!_ground && Velocity.y > 0 && newVel.y == 0);
	// 	Velocity = newVel;

	// 	PostMove(newVel, delta);
	// 	_canJump = false;

	// 	var collision = _colTest.Test(1);

	// 	if (collision != null) EmitSignal("HitCollectable", this, collision.Collider);


	// 	collision = _colTest.Test(2);

	// 	if (collision != null) EmitSignal("HitSpecial", this, collision.Collider);


	// 	if (Math.Abs(Velocity.x) < 0.0003) Velocity.x = 0;

	// 	if (Velocity.x == 0 || (_ground && _inputs.h == 0))
	// 	{
	// 		_stopped += delta;
	// 	}
	// 	else _stopped = 0;

	// 	if (_inputs.h < 0)
	// 	{
	// 		_lastFacing = false;
	// 	}
	// 	else if (_inputs.h > 0)
	// 	{
	// 		_lastFacing = true;
	// 	}

	// 	UpdateAnimation(delta);

	// 	if (Position.y >= 1000)
	// 	{
	// 		Position = _startPos;
	// 	}

	// 	_prevInputs = _inputs;
	// 	ResetStuff();
	// }


	private float _damageTime = 0;
	public override void _Process(float delta)
	{
		_damageTime -= delta;
		if (_damageTime > 0)
		{
			var animatedSprite = GetNode<AnimatedSprite>("Sprite");
			animatedSprite.Visible = _damageTime % 0.2f < 0.1f;
		}
	}

	private void _on_Center_area_entered(Area2D area)
	{
		_collisions.Add(area);
	}

	private void _on_Center_area_exited(Area2D area)
	{
		_collisions.Remove(area);
	}

	public override Vector2 DoJump(bool forceJump, Vector2 velocity, float delta)
	{
		if (_jumpTime > MaxJumpTime) _jumpExpired = true;
		if (forceJump || _canJump || !_jumpExpired)
		{
			_jumpTime += delta;
			if (_canJump) _jumpTime = 0;
			velocity.y = -MathUtils.Interpolate(StartJump, EndJump, _jumpTime / MaxJumpTime) * delta;
			_jumpExpired = false;
		}
		else if (!_ground)
		{
			if (!_prevInputs.j && _floatTime >= FloatDelay)
			{
				if (velocity.y > 0)
				{
					velocity.y = float.Epsilon;
					_floatTime = 0;
					_jumpTime = MaxJumpTime + 1;
				}
			}
		}
		return velocity;
	}



	public override void PreMove(Vector2 v, float delta)
	{
		var feet = GetNode<KinematicBody2D>("Feet");
		var body = GetNode<KinematicBody2D>("Body");
		var head = GetNode<KinematicBody2D>("Head");
		head.SetCollisionLayerBit(12, false);
		feet.SetCollisionMaskBit(12, true);
		body.SetCollisionMaskBit(12, true);
		var contact = feet.MoveAndCollide(v * delta, testOnly: true) ?? body.MoveAndCollide(v * delta, testOnly: true);
		// GD.Print((contact?.Collider as Node)?.Name, (contact?.Collider as Node)?.GetParent()?.Name, Name);
		feet.SetCollisionMaskBit(12, false);
		body.SetCollisionMaskBit(12, false);
		head.SetCollisionLayerBit(12, true);
		if (contact != null)
		{
			if (contact.Position.y > Position.y)
			{
				ForceJump(0.05F);
				if ((contact.Collider as Node)?.GetParent() is Entity)
				{
					((contact.Collider as Node)?.GetParent() as Entity).JumpedOn(this);
				}
			}
		}
	}

	public override void PostMove(Vector2 v, float delta)
	{
		base.PostMove(v, delta);

		var collision = _colTest.Test(14);

		if (collision != null)
		{
			Damage();
			if ((collision.Collider as Node) is Entity)
			{
				((collision.Collider as Node) as Entity).EntityHit(this, collision);
			}
			if ((collision.Collider as Node)?.GetParent() is Entity)
			{
				((collision.Collider as Node)?.GetParent() as Entity).EntityHit(this, collision);
			}
		}

		if (Position.y >= 1000)
		{
			Position = _startPos;
		}
	}

	public override void DoCollisionStuff(KinematicCollision2D touched)
	{
		base.DoCollisionStuff(touched);
		if (touched == null) return;
		if (_inputs.r)
		{
			Entity collided = (touched.Collider as Entity);
			if (collided == null) collided = ((touched.Collider as Node)?.GetParent() as Entity);
			if (collided == null) return;
			collided.Velocity.x += (-touched.Normal.x * Mass);
		}
	}

	public void Damage()
	{
		if (_damageTime <= 0)
		{
			_damageTime = 1.5f;
		}
	}



	private Vector2 _squish = Vector2.One;
	private Vector2 _squishAdd = Vector2.Zero;

	public override void UpdateAnimation(float delta)
	{
		var animatedSprite = GetNode<AnimatedSprite>("Sprite");
		if (_ground)
		{
			if (_inputs.j && _jumpTime <= 0)
			{
				animatedSprite.SpeedScale = 1;
				animatedSprite.Animation = "jump";
				animatedSprite.Frame = 0;
			}
			else
			if (_inputs.h == 0)
			{
				animatedSprite.SpeedScale = _stopped < LookStopTime ? 2 : 1;
				animatedSprite.Animation = "default";
			}
			else
			{
				animatedSprite.FlipH = _inputs.h < 0;
				animatedSprite.Animation = "walk";
				animatedSprite.SpeedScale = _inputs.r && _inputs.h != 0 ? 2 : 1;
			}
		}
		else
		{
			if (Math.Abs(Velocity.x) > 0.1) animatedSprite.FlipH = Velocity.x < 0;
			animatedSprite.Animation = Velocity.y >= 0 ? "down" : "up";
		}
		animatedSprite.Scale = _squish;
		animatedSprite.Offset = new Vector2(0, (1 - _squish.y) * animatedSprite.Frames.GetFrame(animatedSprite.Animation, animatedSprite.Frame).GetHeight());
		if (_squish != Vector2.One)
		{
			_squish += _squishAdd * delta;
			if (_squishAdd.x > 0) _squish.x = Math.Min(_squish.x, 1);
			else if (_squishAdd.x < 0) _squish.x = Math.Max(_squish.x, 1);
			if (_squishAdd.y > 0) _squish.y = Math.Min(_squish.y, 1);
			else if (_squishAdd.y < 0) _squish.y = Math.Max(_squish.y, 1);
		}
	}

	public override void JumpedOn(Entity entity)
	{
		_squish = new Vector2(1.3f, 0.75f);
		_squishAdd = new Vector2(-1.2f, 1f);
	}

	public override void UpdateEntityInputs(float delta)
	{
		if (NoControl) return;
		var h = 0;
		var jump = false;
		var run = false;
		if (Input.IsActionPressed("ui_left")) h--;
		if (Input.IsActionPressed("ui_right")) h++;
		jump = Input.IsActionPressed("pl_jump");
		run = Input.IsActionPressed("pl_run");
		_inputs = new EntityInputs(jump, run, h);
	}
}
