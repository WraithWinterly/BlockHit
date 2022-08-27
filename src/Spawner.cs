using Godot;
using System;
using System.Collections.Generic;
public class Spawner : Node2D
{
  [Export] private PackedScene _enemy1;
  [Export] private PackedScene _enemy2;
  [Export] private PackedScene _powerup1;
  [Export] private PackedScene _powerup2;

  private readonly List<PackedScene> _spawners = new List<PackedScene>();

  private RandomNumberGenerator _rng = new RandomNumberGenerator();

  private Events _events;

  private Timer _timer;

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
    _events = GetTree().Root.GetNode<Events>("Main/Events");

    _spawnNode = Owner.GetNode<Node2D>("%Objects");

    _timer = GetNode<Timer>("Timer");
    _timer.Connect("timeout", this, nameof(OnTimerTimeout));

    _rng.Seed = (Owner as Level).Seed;

    switch (_levelSpeed)
    {
      case LevelSpeed.Slow:
        _waitTimeMin = 0.8f;
        _waitTimeMax = 1.6f;
        break;
      case LevelSpeed.Normal:
        _waitTimeMin = 0.4f;
        _waitTimeMax = 1.2f;
        break;
      case LevelSpeed.Fast:
        _waitTimeMin = 0.2f;
        _waitTimeMax = 0.8f;
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

    _events.Connect(nameof(Events.LevelStarted), this, nameof(LevelStarted));
  }

  private void LevelStarted()
  {
    _timer.Start(0.1f);
  }

  private void SpawnObjects()
  {
    foreach (PackedScene spawner in _spawners)
    {
      int electedSpawner;

      if (_spawners.Count == 2)
      {
        GD.Print("double chance algo");
        int chance = _rng.RandiRange(0, 100);
        if (chance <= 80)
        {
          electedSpawner = 0;
        }
        else
        {
          electedSpawner = 1;
        }
      }
      else if (_spawners.Count == 4)
      {
        int chance = _rng.RandiRange(0, 100);
        if (chance <= 40)
        {
          electedSpawner = 0;
        }
        else if (chance <= 60)
        {
          electedSpawner = 1;
        }
        else if (chance <= 80)
        {
          electedSpawner = 2;
        }
        else
        {
          electedSpawner = 3;
        }
      }
      else
      {
        electedSpawner = _rng.RandiRange(0, _spawners.Count - 1);
      }

      Node2D _spawnObject = (Node2D)_spawners[electedSpawner].Instance();
      _spawnNode.AddChild(_spawnObject);

      int dir = _rng.RandiRange(0, 1);

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
