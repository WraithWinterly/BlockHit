using Godot;
using System;

public class PauseMenu : Control
{
  private Events _events;

  private Button _continueButton;
  private Button _returnToMenuButton;
  private Button _quitButton;

  public override void _Ready()
  {
    base._Ready();
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _continueButton = GetNode<Button>("CenterContainer/VBoxContainer/Continue");
    _returnToMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/ReturnToMenu");
    _quitButton = GetNode<Button>("CenterContainer/VBoxContainer/Quit");

    _continueButton.Connect("pressed", this, nameof(OnContinuePressed));
    _returnToMenuButton.Connect("pressed", this, nameof(OnReturnToMenuPressed));
    _quitButton.Connect("pressed", this, nameof(OnQuitPressed));
    Hide();
  }

  public override void _Input(InputEvent @event)
  {
    base._Input(@event);
    if (@event.IsActionPressed("Pause"))
    {
      if (Main.InTransition || !Main.InGame) return;
      if (GetTree().Paused)
      {
        GetTree().Paused = false;
        Hide();
      }
      else
      {
        GetTree().Paused = true;
        Show();
      }
    }
  }

  private void OnContinuePressed()
  {
    GetTree().Paused = false;
    Hide();
  }

  private void OnReturnToMenuPressed()
  {

  }

  private void OnQuitPressed()
  {
    GetTree().Quit();
  }
}
