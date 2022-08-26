using Godot;
using System;
using System.Collections.Generic;
public class Spawner : Node2D
{
  [Export] private PackedScene _enemy1;
  [Export] private PackedScene _enemy2;
  [Export] private PackedScene _powerup1;
  [Export] private PackedScene _powerup2;

  private RandomNumberGenerator _rng = new RandomNumberGenerator();

  private Timer _timer;

  private readonly List<PackedScene> _spawners = new List<PackedScene>();

  private Node2D _spawnNode;

  private enum LevelSpeed
  {
    Slow,
    Normal,
    Fast,
  }

  [Export] private LevelSpeed _levelSpeed = LevelSpeed.Normal;

  private float _waitTimeMin;
  private float _waitTimeMax;

  public override void _Ready()
  {
    base._Ready();
    _spawnNode = Owner.GetNode<Node2D>("%Objects");

    _timer = GetNode<Timer>("Timer");
    _timer.Connect("timeout", this, nameof(OnTimerTimeout));
    _timer.Start(0);

    switch (_levelSpeed)
    {
      case LevelSpeed.Slow:
        _waitTimeMin = 0.4f;
        _waitTimeMax = 1f;
        break;
      case LevelSpeed.Normal:
        _waitTimeMin = 0.2f;
        _waitTimeMax = 0.8f;
        break;
      case LevelSpeed.Fast:
        _waitTimeMin = 0.1f;
        _waitTimeMax = 0.6f;
        break;
    }

    if (_enemy1 != null)
    {
      _spawners.Add(_enemy1);
    }
    if (_enemy2 != null)
    {
      _spawners.Add(_enemy2);
    }
    if (_powerup1 != null)
    {
      _spawners.Add(_powerup1);
    }
    if (_powerup2 != null)
    {
      _spawners.Add(_powerup2);
    }
  }

  private void SpawnObjects()
  {
    foreach (PackedScene spawner in _spawners)
    {
      int electedSpawner = (int)(GD.Randi() % _spawners.Count);
      Node2D _spawnObject = (Node2D)_spawners[electedSpawner].Instance();
      _spawnNode.AddChild(_spawnObject);

      int dir = (int)(GD.Randi() % 2);

      ObjectBase rigidBody = _spawnObject.GetNode<ObjectBase>("ObjectBase");

      Level level = Owner as Level;

      if (dir == 0)
      {
        rigidBody.GlobalPosition = new Vector2(
          (float)GD.RandRange(level.SpawnTopLeft.x, level.SpawnTopLeft.x + (float)GD.RandRange(-15f, 15f)),
          (float)GD.RandRange(level.SpawnTopLeft.y, level.SpawnBottomRight.y)
        );
        rigidBody.FacingRight = true;
      }
      else
      {
        rigidBody.GlobalPosition = new Vector2(
          (float)GD.RandRange(level.SpawnBottomRight.x, level.SpawnBottomRight.x + (float)GD.RandRange(-15f, 15f)),
          (float)GD.RandRange(level.SpawnTopLeft.y, level.SpawnBottomRight.y)
        );
        rigidBody.FacingRight = false;
      }
    }
  }

  private void OnTimerTimeout()
  {
    _timer.Start((float)GD.RandRange(_waitTimeMin, _waitTimeMax));
    SpawnObjects();
  }
}
