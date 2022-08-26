using Godot;
using System;

public class DeathScreen : Control
{
  private AnimationPlayer _anim;


  public override void _Ready()
  {
    _anim = GetNode<AnimationPlayer>("AnimationPlayer");
    GetTree().Root.GetNode<Events>("Main/Events").Connect(nameof(Events.PlayerDied), this, nameof(OnPlayerDied));
  }

  private async void OnPlayerDied()
  {
    _anim.Play("Fade");
    await ToSignal(_anim, "animation_finished");
    GetTree().Root.GetNode<Events>("Main/Events").EmitSignal(nameof(Events.GameResetTriggered));
  }

}
