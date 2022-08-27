using Godot;
using System;

public class Level : Node2D
{
  [Export] public AudioStream musicStream;
  [Export] public ulong Seed = 10;

  public AudioStreamPlayer _levelTrack;
  public Vector2 SpawnTopLeft = new Vector2();
  public Vector2 SpawnBottomRight = new Vector2();

  private Events _events;
  private Timer _levelTimer;

  public override void _Ready()
  {
    base._Ready();
    _levelTrack = GetNode<AudioStreamPlayer>("LevelTrack");
    SpawnTopLeft = GetNode<Position2D>("SpawnTopLeft").GlobalPosition;
    SpawnBottomRight = GetNode<Position2D>("SpawnTopRight").GlobalPosition;
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _levelTimer = GetNode<Timer>("LevelTimer");

    _events.Connect(nameof(Events.LevelStarted), this, nameof(Start));
    _levelTimer.Connect("timeout", this, nameof(OnTimerTimeout));
  }

  private void Start()
  {
    _levelTrack.Play();
    _levelTimer.Start();
  }

  private void OnTimerTimeout()
  {
    _events.EmitSignal(nameof(Events.LevelCompleted));
  }
}
