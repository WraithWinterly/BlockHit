using Godot;
using System;

public class LevelManager : Node
{
  public enum Levels
  {
    Basket,
    Solid,
    Block,
  }

  public Levels CurrentLevel { get => _level; }

  private Events _events;
  private SaveManager _saveManager;

  private Levels _level = Levels.Basket;

  public override async void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _saveManager = GetTree().Root.GetNode<SaveManager>("Main/SaveManager");
    _events.Connect(nameof(Events.LevelReset), this, nameof(LevelReset));
    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(OnReturnedToMenu));

    _events.Connect(nameof(Events.LevelSelectorChanged), this, nameof(ChangeLevel));

    LoadLevels();

    await ToSignal(GetTree(), "physics_frame");

    ChangeLevel((Levels)_saveManager.Save["LastLevel"]);
  }

  private async void ChangeLevel(Levels level)
  {
    var levelLoad = GD.Load($"res://src/Levels/{level.ToString()}.tscn") as PackedScene;
    if (GetChildCount() > 0)
    {
      foreach (Node2D child in GetChildren())
      {
        child.QueueFree();
      }
    }
    await ToSignal(GetTree(), "physics_frame");

    AddChild(levelLoad.Instance());
    _level = level;
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

  private void LoadLevels()
  {
    foreach (Levels level in Enum.GetValues(typeof(Levels)))
    {
      GD.Load($"res://src/levels/{level.ToString()}.tscn");
    }
  }
}
