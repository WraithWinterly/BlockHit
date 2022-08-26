using Godot;
using System;

public class LevelManager : Node
{
  enum Levels
  {
    Basket,
  }

  private Levels _level = Levels.Basket;

  public override void _Ready()
  {
    GetTree().Root.GetNode<Events>("Main/Events").Connect(nameof(Events.GameResetTriggered), this, nameof(GameResetTriggered));
  }

  private async void ChangeLevel(Levels level)
  {
    await ToSignal(GetNode<FadePlayer>("%FadePlayer"), nameof(FadePlayer.Faded));
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
    await ToSignal(GetNode<FadePlayer>("%FadePlayer"), nameof(FadePlayer.Faded));
    GetTree().Root.GetNode<Events>("Main/Events").EmitSignal(nameof(Events.Start));

  }

  private void GameResetTriggered()
  {
    ChangeLevel(_level);
  }
}
