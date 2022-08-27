using Godot;
using System;

public class MainMenu : Control
{
  private Events _events;
  private AudioStreamPlayer _mainMenuTrack;
  private AnimationPlayer _anim;
  private HBoxContainer _hBox;
  private Button _playButton;
  private Button _quitButton;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _mainMenuTrack = GetNode<AudioStreamPlayer>("MainMenuTrack");
    _anim = GetNode<AnimationPlayer>("AnimationPlayer");

    _hBox = GetNode<HBoxContainer>("CenterContainer/HBoxContainer");
    _playButton = GetNode<Button>("CenterContainer/HBoxContainer/Play");
    _quitButton = GetNode<Button>("CenterContainer/HBoxContainer/Quit");

    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(OnReturnedToMenu));
    _playButton.Connect("pressed", this, nameof(OnPlayPressed));
    _quitButton.Connect("pressed", this, nameof(OnQuitPressed));

    _playButton.GrabFocus();

    _mainMenuTrack.Play();
  }

  private void OnPlayPressed()
  {
    _anim.Play("Fade");
    DisableButtons();
    _events.EmitSignal(nameof(Events.LevelStarted));
    _mainMenuTrack.Playing = false;
  }

  private void DisableButtons()
  {
    foreach (Button button in _hBox.GetChildren())
    {
      button.Disabled = true;
    }
  }

  private void EnableButtons()
  {
    foreach (Button button in _hBox.GetChildren())
    {
      button.Disabled = false;
    }
  }

  private void OnQuitPressed()
  {
    GetTree().Quit();
  }

  private async void OnReturnedToMenu()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    Main.InGame = false;
    Modulate = new Color(1, 1, 1, 1);
    Show();
    EnableButtons();
    _mainMenuTrack.Play();
    _playButton.GrabFocus();
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
