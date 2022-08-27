using Godot;
using System;

public class Main : Node
{
  private Events _events;
  private LevelManager _levelManager;

  public static bool InTransition;
  public static bool InGame;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _levelManager = GetNode<LevelManager>("LevelManager");

    _events.Connect(nameof(Events.LevelStarted), this, nameof(OnLevelStarted));
    VisualServer.SetDefaultClearColor(Colors.Black);
  }

  public override void _Input(InputEvent @event)
  {
    base._Input(@event);
    if (@event.IsActionPressed("Fullscreen"))
    {
      OS.WindowFullscreen = !OS.WindowFullscreen;
      GetTree().SetInputAsHandled();
    }
  }

  public Timer GetLevelTimer()
  {
    return GetNode<Timer>($"LevelManager/{_levelManager.CurrentLevel.ToString()}/LevelTimer");
  }

  public Player GetPlayer()
  {
    return GetNode<Player>($"LevelManager/{_levelManager.CurrentLevel.ToString()}/Player");
  }

  private void OnLevelStarted()
  {
    InGame = true;
  }

}
