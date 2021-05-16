using Godot;
using System;

public class CamObj : Node2D
{
	[Export]
	public Vector2 MultiplyVector = new Vector2(0.6f, 0.2f);
	[Export]
	public Vector2 MaxDist = new Vector2(100, 70);
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent<Player>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var v = _player.Velocity * MultiplyVector;
		v.x = Math.Max(-MaxDist.x, Math.Min(MaxDist.x, v.x));
		v.y = Math.Max(-MaxDist.y, Math.Min(MaxDist.y, v.y));
		GlobalPosition = _player.GlobalPosition + v;
	}
}
