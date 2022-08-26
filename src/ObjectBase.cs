using Godot;
using System;

public class ObjectBase : RigidBody2D
{
  public Types ObjType { get => type; }
  public float Speed { get => _currSpeed; }
  public bool Dying { get => _dying; }
  public int EnemyDamage { get => _enemyDamage; }
  public int PowerBoost { get => _powerupBoost; }
  public bool FacingRight;

  public enum Types
  {
    Enemy,
    Powerup,
  }

  [Export] private Types type = Types.Enemy;
  [Export] private Color color = new Color(1, 1, 1, 1);

  [Export] private float _speedMax = 1.2f;
  [Export] private float _speedMin = 0.8f;

  [Export] private int _enemyDamage = 20;
  [Export] private int _powerupBoost = 20;

  [Export] private bool _impulseY;
  [Export] private bool _bounces;

  private Sprite _sprite;
  private Area2D _area;
  private AnimationPlayer _anim;
  private CollisionShape2D _coll;

  private AudioStreamPlayer2D _sound;

  private float _currSpeed;
  private bool _dying;

  public override async void _Ready()
  {
    base._Ready();
    _sprite = GetNode<Sprite>("Sprite");
    _area = GetNode<Area2D>("Area2D");
    _anim = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
    _coll = GetNode<CollisionShape2D>("CollisionShape2D");

    _sound = GetNode<AudioStreamPlayer2D>("Sound");

    _currSpeed = (float)GD.RandRange(_speedMin, _speedMax);
    await ToSignal(GetTree(), "physics_frame");

    _sprite.FlipH = !FacingRight;

    if (type == Types.Enemy)
    {
      AddToGroup("Enemy");
    }

    if (type == Types.Powerup)
    {
      AddToGroup("Powerup");
    }

    if (_bounces)
    {
      PhysicsMaterial physicsMaterial = new PhysicsMaterial();
      physicsMaterial.Bounce = 1;
      physicsMaterial.Friction = 0.2f;
      PhysicsMaterialOverride = physicsMaterial;
    }

    _sprite.SelfModulate = color;
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);

    foreach (Node body in _area.GetOverlappingBodies())
    {
      if (body.IsInGroup("Object") && body != this)
      {
        var objBase = body as ObjectBase;

        if (!objBase.Dying && objBase.ObjType == Types.Enemy)
        {
          if (objBase.Speed < _currSpeed)
          {
            return;
          }
          QueueFree();
        }
      }

      if (body.IsInGroup("Player"))
      {
        Die();
      }
    }

    if (GlobalPosition.x < -120 || GlobalPosition.x > 512)
    {
      Owner.QueueFree();
    }

    if (GlobalPosition.y < -16 || GlobalPosition.y > 240)
    {
      Owner.QueueFree();
    }
  }

  public override void _IntegrateForces(Physics2DDirectBodyState state)
  {
    base._IntegrateForces(state);
    if (FacingRight)
    {
      state.ApplyCentralImpulse(new Vector2(Speed, _impulseY ? _currSpeed : 0));
    }
    else
    {
      state.ApplyCentralImpulse(new Vector2(-Speed, _impulseY ? -_currSpeed : 0));
    }
  }

  private async void Die()
  {
    _dying = true;
    _coll.SetDeferred("disabled", true);
    SetPhysicsProcess(false);
    _sound.PitchScale = 1f + (float)GD.RandRange(-0.15f, 0.15f);
    _sound.Play();
    _anim.Play("Death");
    await ToSignal(_anim, "animation_finished");
    QueueFree();
  }
}
