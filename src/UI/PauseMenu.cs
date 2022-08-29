using Godot;
using System;

public class PauseMenu : Control
{
  private Events _events;
  private VBoxContainer _vBox;
  private Control _pauseMenu;
  private TextureButton _pauseButton;
  private Button _continueButton;
  private Button _returnToMenuButton;

  public override void _Ready()
  {
    base._Ready();
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _pauseMenu = GetNode<Control>("PauseMenu");
    _pauseButton = GetNode<TextureButton>("PauseButton");
    _vBox = GetNode<VBoxContainer>("PauseMenu/CenterContainer/VBoxContainer");
    _continueButton = GetNode<Button>("PauseMenu/CenterContainer/VBoxContainer/Continue");
    _returnToMenuButton = GetNode<Button>("PauseMenu/CenterContainer/VBoxContainer/ReturnToMenu");

    _events.Connect(nameof(Events.LevelStarted), this, nameof(OnLevelStarted));
    _events.Connect(nameof(Events.LevelCompleted), this, nameof(OnLevelCompleted));
    _pauseButton.Connect("pressed", this, nameof(OnPauseButtonPressed));
    _continueButton.Connect("pressed", this, nameof(OnContinuePressed));
    _returnToMenuButton.Connect("pressed", this, nameof(OnReturnToMenuPressed));
    _pauseMenu.Hide();
    _pauseButton.Hide();
  }

  public override void _Input(InputEvent @event)
  {
    base._Input(@event);
    if (Main.InTransition || !Main.InGame) return;
    if (@event.IsActionPressed("Pause"))
    {
      TogglePause();
    }
  }

  private void TogglePause()
  {

    if (GetTree().Paused)
    {
      GetTree().Paused = false;
      _pauseMenu.Hide();
    }
    else
    {
      GetTree().Paused = true;
      _pauseMenu.Show();
      EnableButtons();
      _continueButton.GrabFocus();
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

  private void OnLevelStarted()
  {
    if (OS.GetName() != "Windows")
    {
      _pauseButton.Show();
    }
  }

  private async void OnLevelCompleted()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    _pauseButton.Hide();
  }

  private void OnPauseButtonPressed()
  {
    TogglePause();
  }

  private void OnContinuePressed()
  {
    GetTree().Paused = false;
    _pauseMenu.Hide();
  }

  private async void OnReturnToMenuPressed()
  {
    DisableButtons();
    _events.EmitSignal(nameof(Events.ReturnedToMenu));
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    _pauseMenu.Hide();
    _pauseButton.Hide();
  }
}
