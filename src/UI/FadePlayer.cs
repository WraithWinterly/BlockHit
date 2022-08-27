using Godot;
using System;

public class FadePlayer : CanvasLayer
{
  private Events _events;

  private AnimationPlayer _anim;

  public override void _EnterTree()
  {
    base._EnterTree();
    Show();
  }

  public override void _Ready()
  {
    base._Ready();
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _anim = GetNode<AnimationPlayer>("TextureRect/AnimationPlayer");
    _anim.Connect("animation_finished", this, nameof(OnAnimationFinished));

    _events.Connect(nameof(Events.LevelReset), this, nameof(Transition));
    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(Transition));
    InitialTransition();
  }

  public async void Transition()
  {
    Main.InTransition = true;
    GetTree().Paused = true;
    _anim.PlayBackwards("Fade");
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    _anim.Play("Fade");
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    GetTree().Paused = false;
    Main.InTransition = false;
  }

  private async void InitialTransition()
  {
    GetTree().Paused = true;
    _anim.Play("Fade");
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    GetTree().Paused = false;
  }

  private void OnAnimationFinished(String animName)
  {
    _events.EmitSignal(nameof(Events.FadePlayerFaded));
  }
}
