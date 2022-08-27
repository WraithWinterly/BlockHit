using Godot;
using System;

public class HUD : Control
{
  private Events _events;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");

    _events.Connect(nameof(Events.LevelStarted), this, nameof(OnLevelStarted));
    Hide();
  }

  private void OnLevelStarted()
  {
    Show();
  }



  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
