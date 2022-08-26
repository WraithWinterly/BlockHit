using Godot;
using System;

public class LevelManager : Node
{
  private enum Levels
  {
    Basket,
  }

  private Events _events;

  private Levels _level = Levels.Basket;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _events.Connect(nameof(Events.LevelReset), this, nameof(LevelReset));
  }

  private async void ChangeLevel(Levels level)
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));

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

    await ToSignal(_events, nameof(Events.FadePlayerFaded));

    _events.EmitSignal(nameof(Events.LevelStarted));

  }

  private void LevelReset()
  {
    ChangeLevel(_level);
  }
}
