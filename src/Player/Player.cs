using Godot;
using System;

public class Player : Node2D
{
  public int Health { get => _health; }
  public int Score { get => _score; }

  private const int MaxHealth = 100;

  [Signal] public delegate void Scored();

  private AudioStreamPlayer _scoreSound;
  private AudioStreamPlayer _deathSound;
  private AudioStreamPlayer _hitSound;

  private ScoreLight _scoreLight;

  private Area2D _area;

  private int _health;
  private int _score;

  public override void _Ready()
  {
    _scoreLight = GetNode<ScoreLight>("%ScoreLight");
    Connect(nameof(Scored), this, nameof(OnScored));

    _scoreSound = GetNode<AudioStreamPlayer>("ScoreSound");
    _deathSound = GetNode<AudioStreamPlayer>("DeathSound");
    _hitSound = GetNode<AudioStreamPlayer>("HurtSound");

    _area = GetNode<Area2D>("KinematicBody2D/Area2D");
    _area.Connect("body_entered", this, nameof(OnAreaBodyEntered));

    _health = MaxHealth;
  }

  public void RemoveHealth(int amount)
  {
    _health -= amount;
    _health = _health < 0 ? 0 : _health;
    if (_health == 0)
    {
      GetTree().Root.GetNode<Events>("Main/Events").EmitSignal(nameof(Events.PlayerDied));
      var k = GetNode<KinematicBody2D>("KinematicBody2D");
      k.SetPhysicsProcess(false);
      k.SetProcess(false);
      k.SetProcessInput(false);
      k.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
      _area.SetDeferred("monitoring", false);
      _area.SetDeferred("monitorable", false);
      GetNode<AnimationPlayer>("KinematicBody2D/AnimationPlayer").Play("Death");
      _deathSound.Play();
    }
  }

  public void AddHealth(int amount)
  {
    _health += amount;
    _health = _health > MaxHealth ? MaxHealth : _health;
  }

  private void OnScored()
  {
    _score += 1;
    _scoreSound.PitchScale = (float)GD.RandRange(0.95f, 1.1f);
    _scoreSound.Play();
    _scoreLight.Play();
  }

  private void OnAreaBodyEntered(Node body)
  {
    if (body.IsInGroup("Enemy"))
    {
      var objBody = body as ObjectBase;
      RemoveHealth(objBody.EnemyDamage);
      _hitSound.PitchScale = 1f + (float)GD.RandRange(-0.15f, 0.15f);
      _hitSound.Play();
    }
    else if (body.IsInGroup("Powerup"))
    {
      var objBody = body as ObjectBase;
      AddHealth(objBody.PowerBoost);
    }
  }
}
