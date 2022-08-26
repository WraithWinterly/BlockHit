using Godot;
using System;

public class DeathScreen : Control
{
  private Events _events;
  private AnimationPlayer _anim;


  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _anim = GetNode<AnimationPlayer>("AnimationPlayer");

    _events.Connect(nameof(Events.PlayerDied), this, nameof(OnPlayerDied));
  }

  private async void OnPlayerDied()
  {
    _anim.Play("Fade");
    await ToSignal(_anim, "animation_finished");
    _events.EmitSignal(nameof(Events.LevelReset));
  }

}
