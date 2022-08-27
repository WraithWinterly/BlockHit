using Godot;
using System;

public class HUD : Control
{
  private Events _events;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");

    _events.Connect(nameof(Events.LevelStarted), this, nameof(OnLevelStarted));
    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(OnReturnedToMenu));
    Hide();
  }

  private void OnLevelStarted()
  {
    Show();
  }

  private async void OnReturnedToMenu()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    Hide();
  }



  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
