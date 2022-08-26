using Godot;
using System;

public class MainMenu : Control
{
  private Events _events;
  private AnimationPlayer _anim;
  private Button _button;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _anim = GetNode<AnimationPlayer>("AnimationPlayer");
    _button = GetNode<Button>("Play");
    _button.Connect("pressed", this, nameof(OnPlayPressed));
  }

  private void OnPlayPressed()
  {
    _anim.Play("Fade");
    _button.Disabled = true;
    _events.EmitSignal(nameof(Events.Start));
    GetNode<AudioStreamPlayer>("MainMenuTrack").Playing = false;
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
