using Godot;
using System;

public class PauseMenu : Control
{
  private Events _events;
  private VBoxContainer _vBox;
  private Button _continueButton;
  private Button _returnToMenuButton;

  public override void _Ready()
  {
    base._Ready();
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _vBox = GetNode<VBoxContainer>("CenterContainer/VBoxContainer");
    _continueButton = GetNode<Button>("CenterContainer/VBoxContainer/Continue");
    _returnToMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/ReturnToMenu");

    _continueButton.Connect("pressed", this, nameof(OnContinuePressed));
    _returnToMenuButton.Connect("pressed", this, nameof(OnReturnToMenuPressed));
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
        EnableButtons();
        _continueButton.GrabFocus();
      }
    }
  }

  private void DisableButtons()
  {
    foreach (Button button in _vBox.GetChildren())
    {
      button.Disabled = true;
    }
  }

  private void EnableButtons()
  {
    foreach (Button button in _vBox.GetChildren())
    {
      button.Disabled = false;
    }
  }

  private void OnContinuePressed()
  {
    GetTree().Paused = false;
    Hide();
  }

  private async void OnReturnToMenuPressed()
  {
    DisableButtons();
    _events.EmitSignal(nameof(Events.ReturnedToMenu));
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    Hide();
  }
}
