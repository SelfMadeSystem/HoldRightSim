using Godot;
using Utils;
using System;
using System.Collections.Generic;
public class Entity : KinematicBody2D, Collidable
{
    [Export]
    public int DefMaxSpeed = 6000;
    [Export]
    public int DefMaxRunSpeed = 10000;
    [Export]
    public int DefAccel = 600;
    [Export]
    public int DefRunAccel = 800;
    [Export]
    public float DefGroundFriction = 20f;
    [Export]
    public float AirFriction = 1f;
    [Export]
    public int DefStartJump = 14000;
    [Export]
    public int DefEndJump = 9000;
    [Export]
    public float DefMaxJumpTime = 0.4f;
    [Export]
    public int DefGravity = 1400;

    public Vector2 Velocity = new Vector2();
    public int StartJump;
    public int EndJump;
    public float MaxJumpTime;
    public bool ResetJumpGround;
    public bool ResetJumpAir;
    public bool ResetJumpFinishJump;
    public float JumpReset;
    public int Gravity;
    public float GravityReset;
    public int MaxSpeed;
    public int MaxRunSpeed;
    public bool ResetSpeedGround;
    public bool ResetSpeedAir;
    public float SpeedReset;
    public int Accel;
    public int RunAccel;
    public bool ResetAccelGround;
    public bool ResetAccelAir;
    public float AccelReset;
    public float GroundFriction;
    public bool ResetGroundFrictionGround;
    public bool ResetGroundFrictionAir;
    public float GroundFrictionReset;

    protected bool _lastFacing = false;
    protected float _stopped = 10000;
    protected bool _ground = false;
    protected bool _jumpExpired = true;
    protected bool _canJump = false;
    protected float _forceJump = 0;
    protected float _jumpTime = 0;
    protected float _floatTime = 0;
    protected Vector2 _startPos;
    protected List<Area2D> _collisions = new List<Area2D>();
    protected EntityInputs _prevInputs = new EntityInputs();
    protected EntityInputs _inputs = new EntityInputs();
    protected ColTest _colTest;

    public override void _Ready()
    {
        _startPos = Position;
        _colTest = GetNode<ColTest>("ColTest");
        ResetJump();
        ResetSpeed();
        Gravity = DefGravity;
    }
    public virtual void JumpedOn(Entity entity)
    {

    }
    public virtual void UpdateEntityInputs(float delta)
    {
    }

    public void ResetStuff(float delta)
    {
        if (GravityReset > 0 && (GravityReset -= delta) <= 0) Gravity = DefGravity;
        if (JumpReset > 0 && (JumpReset -= delta) == 0) ResetJump();
        if (SpeedReset > 0 && (SpeedReset -= delta) == 0)
        {
            MaxSpeed = DefMaxSpeed;
            MaxRunSpeed = DefMaxRunSpeed;
        }
        if (AccelReset > 0 && (AccelReset -= delta) == 0)
        {

            Accel = DefAccel;
            RunAccel = DefRunAccel;
        }
        if (GroundFrictionReset > 0 && (GroundFrictionReset -= delta) == 0) GroundFriction = DefGroundFriction;
        if (_ground)
        {
            if (ResetJumpGround) ResetJump();
            if (ResetSpeedGround)
            {
                MaxSpeed = DefMaxSpeed;
                MaxRunSpeed = DefMaxRunSpeed;
            }
            if (ResetAccelGround)
            {
                Accel = DefAccel;
                RunAccel = DefRunAccel;
            }
            if (ResetGroundFrictionGround) GroundFriction = DefGroundFriction;
            ResetJumpGround = false;
            ResetSpeedGround = false;
            ResetAccelGround = false;
            ResetGroundFrictionGround = false;
        }
        else if (!_ground)
        {
            if (ResetJumpAir) ResetJump();
            if (ResetSpeedAir)
            {
                MaxSpeed = DefMaxSpeed;
                MaxRunSpeed = DefMaxRunSpeed;
            }
            if (ResetAccelAir)
            {
                Accel = DefAccel;
                RunAccel = DefRunAccel;
            }
            if (ResetGroundFrictionAir) GroundFriction = DefGroundFriction;
            ResetJumpAir = false;
            ResetSpeedAir = false;
            ResetAccelAir = false;
            ResetGroundFrictionAir = false;
        }
    }

    public virtual void ResetJump()
    {
        StartJump = DefStartJump;
        EndJump = DefEndJump;
        MaxJumpTime = DefMaxJumpTime;
    }

    public virtual void ResetSpeed()
    {
        if (SpeedReset <= 0)
        {
            MaxSpeed = DefMaxSpeed;
            MaxRunSpeed = DefMaxRunSpeed;
        }
        if (AccelReset <= 0)
        {
            Accel = DefAccel;
            RunAccel = DefRunAccel;
        }
        if (GroundFrictionReset <= 0)
            GroundFriction = DefGroundFriction;
    }

    public void AllowJump()
    {
        _canJump = true;
    }

    public void ForceJump(float time)
    {
        _forceJump = time;
        _jumpTime = 0;
    }

    public virtual void UpdateAnimation(float delta)
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        UpdateEntityInputs(delta);
        Vector2 velocity = Velocity;
        var groundTest = MoveAndCollide(new Vector2(0, 0.1f), testOnly: true);
        if (!_ground || groundTest == null)
        {
            velocity.y += Gravity * delta;
        }
        float speedUp = (_inputs.r ? RunAccel : Accel) * delta;
        float maxSpeed = (_inputs.r ? MaxRunSpeed : MaxSpeed) * delta;
        switch (_inputs.h)
        {
            case -1:
                {
                    if (velocity.x >= -maxSpeed)
                    {
                        if (velocity.x > 0) velocity = ApplyFriction(velocity, delta);
                        velocity.x = Math.Max(-maxSpeed, velocity.x - speedUp);
                    }
                    else if (_ground) velocity = ApplyFriction(velocity, delta);
                    break;
                }
            case 1:
                {
                    if (velocity.x <= maxSpeed)
                    {
                        if (velocity.x < 0) velocity = ApplyFriction(velocity, delta);
                        velocity.x = Math.Min(maxSpeed, velocity.x + speedUp);
                    }
                    else if (_ground) velocity = ApplyFriction(velocity, delta);
                    break;
                }
        }

        if (_ground)
        {
            _jumpExpired = false;

        }
        if (_inputs.h == 0)
        {
            velocity = ApplyFriction(velocity, delta);
        }

        var forceJump = _forceJump > 0;

        if (forceJump || _inputs.j)
        {
            velocity = DoJump(forceJump, velocity, delta);
        }
        else
        {
            _jumpTime = 0;
            _jumpExpired = true;
            if (ResetJumpFinishJump) ResetJump();
        }

        _forceJump -= delta;

        _floatTime += delta;

        foreach (var area in _collisions)
        {
            var g = GlobalPosition.x - area.GlobalPosition.x;

            velocity.x += Math.Min(20, Math.Max(-20, 1 / g * delta * 4000));
            if (Math.Abs(g) <= 3)
            {
                if (g < 0)
                {
                    velocity.x = Math.Min(velocity.x, 0);
                }
                else if (g > 0)
                {
                    velocity.x = Math.Max(velocity.x, 0);
                }
                velocity.x += g * delta * 500;
            }
        }

        PreMove(velocity, delta);

        var newVel = MoveAndSlide(velocity, Vector2.Up, false, 2, 0.785398f, false);

        if (newVel.y != 0 && newVel.y != velocity.y)
        {
            if (velocity.y < 0) _jumpTime = MaxJumpTime + 1;
            newVel.x = velocity.x;
        }

        if (Velocity.y < 0 && newVel.y == 0 && _inputs.j) _jumpTime = MaxJumpTime + 1;
        _ground = (_ground && newVel.y == 0) || (!_ground && Velocity.y > 0 && newVel.y == 0);
        Velocity = newVel;

        PostMove(newVel, delta);

        UpdateAnimation(delta);

        _prevInputs = _inputs;
        ResetStuff(delta);
    }

    public virtual Vector2 DoJump(bool forceJump, Vector2 velocity, float delta)
    {
        if (_jumpTime > MaxJumpTime) _jumpExpired = true;
        if (forceJump || _canJump || !_jumpExpired)
        {
            _jumpTime += delta;
            if (_canJump) _jumpTime = 0;
            velocity.y = -MathUtils.Interpolate(StartJump, EndJump, _jumpTime / MaxJumpTime) * delta;
            _jumpExpired = false;
        }
        return velocity;
    }

    public virtual void EntityHit(Entity entity, KinematicCollision2D collision)
    {
    }

    public virtual Vector2 ApplyFriction(Vector2 velocity, float delta)
    {
        float friction = _ground ? GroundFriction : AirFriction;
        float minus = velocity.x * friction;
        velocity.x -= minus * delta;
        return velocity;
    }
    public virtual void PreMove(Vector2 v, float delta)
    {
    }

    public virtual void PostMove(Vector2 v, float delta)
    {
        _canJump = false;
        var collision = _colTest.Test(1);

        if (collision != null) if (collision.Collider is Collidable) (collision.Collider as Collidable).EntityHit(this, collision);


        collision = _colTest.Test(2);

        if (collision != null) if (collision.Collider is Collidable) (collision.Collider as Collidable).EntityHit(this, collision);


        if (Math.Abs(Velocity.x) < 0.0003) Velocity.x = 0;

        if (Velocity.x == 0 || (_ground && _inputs.h == 0))
        {
            _stopped += delta;
        }
        else _stopped = 0;

        if (_inputs.h < 0)
        {
            _lastFacing = false;
        }
        else if (_inputs.h > 0)
        {
            _lastFacing = true;
        }
        var count = GetSlideCount();
        for (int i = 0; i < count; i++)
        {
            DoCollisionStuff(GetSlideCollision(i));
        }
        var groundTest = MoveAndCollide(new Vector2(0, 1f), testOnly: true);
        DoCollisionStuff(groundTest);
        if (!_ground)
        {
            if (groundTest != null) { _ground = groundTest.Normal.y < -0.7; }
        }
    }

    public virtual void DoCollisionStuff(KinematicCollision2D touched)
    {
        if (touched == null) return;
        if (touched.Normal.y < -0.7) _ground = true;
        if (touched.Collider is Collidable) (touched.Collider as Collidable).EntityHit(this, touched);
    }

    public bool Ground {get => _ground;}

    public class EntityInputs
    {
        public readonly bool j; // Jump
        public readonly bool r; // Run
        public readonly int h; // Horizontal
        public readonly int v; // Vertical

        public EntityInputs(bool jump = false, bool run = false, int horizontal = 0, int vertical = 0)
        {
            j = jump;
            r = run;
            h = horizontal;
            v = vertical;
        }
    }
}
