using Godot;
using System;

public class MainMenu : Control
{
  private Events _events;
  private AudioStreamPlayer _mainMenuTrack;
  private AnimationPlayer _anim;
  private Button _playButton;
  private Button _quitButton;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _mainMenuTrack = GetNode<AudioStreamPlayer>("MainMenuTrack");
    _anim = GetNode<AnimationPlayer>("AnimationPlayer");

    _playButton = GetNode<Button>("CenterContainer/VBoxContainer/Play");
    _quitButton = GetNode<Button>("CenterContainer/VBoxContainer/Quit");

    _playButton.Connect("pressed", this, nameof(OnPlayPressed));
    _quitButton.Connect("pressed", this, nameof(OnQuitPressed));
  }

  private void OnPlayPressed()
  {
    _anim.Play("Fade");
    _playButton.Disabled = true;
    _events.EmitSignal(nameof(Events.LevelStarted));
    _mainMenuTrack.Playing = false;
  }

  private void OnQuitPressed()
  {
    GetTree().Quit();
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
