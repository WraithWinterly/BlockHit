using Godot;
using System;

public class Main : Node
{
  private LevelManager _levelManager;

  public override void _Ready()
  {
    _levelManager = GetNode<LevelManager>("LevelManager");
    VisualServer.SetDefaultClearColor(Colors.Black);
  }

  public Timer GetLevelTimer()
  {
    return GetNode<Timer>($"LevelManager/{_levelManager.CurrentLevel.ToString()}/LevelTimer");
  }

  public Player GetPlayer()
  {
    return GetNode<Player>($"LevelManager/{_levelManager.CurrentLevel.ToString()}/Player");
  }

}
