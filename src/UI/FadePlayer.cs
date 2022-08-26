using Godot;
using System;

public class FadePlayer : CanvasLayer
{
  [Signal] public delegate void Faded();

  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
    base._Ready();
    _animationPlayer = GetNode<AnimationPlayer>("TextureRect/AnimationPlayer");
    _animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
    InitialTransition();
    Show();
  }

  public async void Transition()
  {
    GetTree().Paused = true;
    _animationPlayer.PlayBackwards("Fade");
    await ToSignal(this, nameof(Faded));
    _animationPlayer.Play("Fade");
    await ToSignal(this, nameof(Faded));
    GetTree().Paused = false;
  }

  private async void InitialTransition()
  {
    GetTree().Paused = true;
    _animationPlayer.Play("Fade");
    await ToSignal(this, nameof(Faded));
    GetTree().Paused = false;
  }

  private void OnAnimationFinished(String animName)
  {
    EmitSignal(nameof(Faded));
  }
}
