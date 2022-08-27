using Godot;
using System;

public class Player : Node2D
{
  public int Health { get => _health; }
  public int Score { get => _score; }

  private const int MaxHealth = 100;

  [Signal] public delegate void Scored();

  private Events _events;
  private PlayerController _playerController;

  private AudioStreamPlayer _scoreSound;
  private AudioStreamPlayer _deathSound;
  private AudioStreamPlayer _hitSound;
  private AudioStreamPlayer _completeSound;

  private ScoreLight _scoreLight;

  private Area2D _area;

  private int _health;
  private int _score;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _playerController = GetNode<PlayerController>("KinematicBody2D");

    _scoreLight = GetNode<ScoreLight>("%ScoreLight");
    _scoreSound = GetNode<AudioStreamPlayer>("ScoreSound");
    _deathSound = GetNode<AudioStreamPlayer>("DeathSound");
    _hitSound = GetNode<AudioStreamPlayer>("HurtSound");
    _completeSound = GetNode<AudioStreamPlayer>("CompleteSound");

    _area = GetNode<Area2D>("KinematicBody2D/Area2D");

    _events.Connect(nameof(Events.LevelComplete), this, nameof(OnLevelComplete));
    Connect(nameof(Scored), this, nameof(OnScored));
    _area.Connect("body_entered", this, nameof(OnAreaBodyEntered));

    _health = MaxHealth;
  }

  public void RemoveHealth(int amount)
  {
    _health -= amount;
    _health = _health < 0 ? 0 : _health;

    if (_health == 0)
    {
      Die();
    }
  }

  private void Die()
  {
    _events.EmitSignal(nameof(Events.PlayerDied));

    _playerController.Die();

    _deathSound.Play();
  }

  private void OnLevelComplete()
  {
    _completeSound.Play();
    _playerController.OnLevelComplete();
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
