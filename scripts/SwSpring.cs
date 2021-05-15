using Godot;
using Godot.Collections;

public class SwSpring : Entity
{
	[Export]
	public Vector2 Force = new Vector2(300, 150);
	[Export]
	public int MaxY = 300;
	[Export]
	public bool VertGroundOnly = true;
	private float _sproingTime;
	public override void _Ready()
	{
		base._Ready();
		AddCollisionExceptionWith(GetNode<KinematicBody2D>("Solid"));
		Force.y *= -1;
		// GetNode<KinematicBody2D>("RSide").Connect("body_entered", this, "RBoing");
		// GetNode<KinematicBody2D>("LSide").Connect("body_entered", this, "LBoing");
	}

	public override void UpdateAnimation(float delta)
	{
		var sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		sprite.Animation = _sproingTime < 0 ? "default" : _sproingTime < 0.15f || _sproingTime > 0.2f ? "half" : "full";
		_sproingTime -= delta;
	}

	public void LBoing(Node body)
	{
		Boing(body, -1);
	}
	public void RBoing(Node body)
	{
		Boing(body, 1);
	}

	public void Boing(Node body, int side)
	{
		if (body == this) return;
		Entity entity = body as Entity ?? body?.GetParent<Entity>();
		if (entity == null || (entity is Player && entity != body)) return;
		_sproingTime = 0.25f;
		entity.Velocity = new Vector2(side * Force.x, entity.Velocity.y + (entity.Velocity.y >= -MaxY && (!VertGroundOnly || entity.Ground)? Force.y : 0));
		entity.GroundFriction = entity.AirFriction;
		entity.GroundFrictionReset = 0.25f;
	}
}
