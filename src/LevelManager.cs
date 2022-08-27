using Godot;
using System;

public class LevelManager : Node
{
  public enum Levels
  {
    Basket,
  }

  public Levels CurrentLevel { get => _level; }

  private Events _events;

  private Levels _level = Levels.Basket;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _events.Connect(nameof(Events.LevelReset), this, nameof(LevelReset));
    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(OnReturnedToMenu));
  }

  private async void ChangeLevel(Levels level)
  {
    if (GetChildCount() > 0)
    {
      foreach (Node2D child in GetChildren())
      {
        child.QueueFree();
      }
    }
    await ToSignal(GetTree(), "physics_frame");

    var levelLoad = GD.Load($"res://src/levels/{level.ToString()}.tscn") as PackedScene;
    AddChild(levelLoad.Instance());



  }

  private async void LevelReset()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    ChangeLevel(_level);
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    _events.EmitSignal(nameof(Events.LevelStarted));
  }

  private async void OnReturnedToMenu()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    ChangeLevel(_level);
  }
}
