using Godot;
using System;

public class PlayerController : KinematicBody2D
{
  const int MaxJumpBufferFrames = 6;
  const int Speed = 2;
  const float Gravity = 9.8f * 4;

  private Events _events;

  private Player _player;
  private Vector2 _linearVelocity;
  private AnimationPlayer _anim;

  private CollisionShape2D _coll;
  private Area2D _area;

  private float _currentGravity;
  private float _jumpBufferFrames;

  private int _gravityDir = 1;

  private bool _isGrounded;
  private bool _hasJumpBuffered;
  private bool _canChangeDir;

  private bool _resetPlayer = true;

  public override void _Ready()
  {
    base._Ready();
    _player = Owner as Player;
    _events = GetTree().Root.GetNode<Events>("Main/Events");

    _anim = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");

    _coll = GetNode<CollisionShape2D>("CollisionShape2D");
    _area = GetNode<Area2D>("Area2D");

    _events.Connect(nameof(Events.LevelStarted), this, nameof(Start));

    _jumpBufferFrames = MaxJumpBufferFrames;
    _currentGravity = Gravity;

    SetPhysicsProcess(false);
  }

  private void Start()
  {
    SetPhysicsProcess(true);
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);

    _currentGravity = Mathf.Abs(_currentGravity);
    _currentGravity *= _gravityDir;

    _linearVelocity.x = 0;
    _linearVelocity.y += _currentGravity;

    _linearVelocity = MoveAndSlide(_linearVelocity, _gravityDir == 1 ? Vector2.Up : Vector2.Down);

    if (IsOnFloor())
    {
      if (!_canChangeDir)
      {
        _canChangeDir = true;
        if (_resetPlayer) return;
        _player.EmitSignal(nameof(Player.Scored));
        _anim.Play(_gravityDir == 1 ? "ScoreDown" : "ScoreUp");
      }
    }

    if (_hasJumpBuffered)
    {
      _jumpBufferFrames -= 1;
      if (_jumpBufferFrames <= 0)
      {
        _hasJumpBuffered = false;
        _jumpBufferFrames = MaxJumpBufferFrames;
      }
    }

    if (_hasJumpBuffered && IsOnFloor())
    {
      Jump();
      _hasJumpBuffered = false;
      _jumpBufferFrames = MaxJumpBufferFrames;
    }
  }

  public override void _Input(InputEvent @event)
  {
    base._Input(@event);
    if (@event.IsActionPressed("Jump"))
    {
      if (_canChangeDir)
      {
        Jump();
      }
      else
      {
        _hasJumpBuffered = true;
      }
    }
  }

  private void EndCharacterCommon()
  {
    SetPhysicsProcess(false);
    SetProcess(false);
    SetProcessInput(false);
    _coll.SetDeferred("disabled", true);
    _area.SetDeferred("monitoring", false);
    _area.SetDeferred("monitorable", false);
  }

  public void Die()
  {
    EndCharacterCommon();
    _anim.Play("Death");
  }

  public void OnLevelComplete()
  {
    EndCharacterCommon();
    _anim.Play("Complete");
  }

  private void Jump()
  {
    if (!_canChangeDir) return;

    _resetPlayer = false;
    _linearVelocity.y = Speed;
    _gravityDir *= -1;
    _canChangeDir = false;
  }


}

