using Godot;
using System;

public class FadePlayer : CanvasLayer
{
  private Events _events;

  private AnimationPlayer _anim;

  private Label blockHitLabel;
  private Label loadingLabel;

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

    blockHitLabel = GetNode<Label>("TextureRect/BlockHitLabel");
    loadingLabel = GetNode<Label>("TextureRect/LoadingLabel");

    _events.Connect(nameof(Events.LevelReset), this, nameof(OnLevelReset));
    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(OnReturnedToMenu));
    InitialTransition();
  }

  public async void Transition(bool showText = true)
  {
    blockHitLabel.Visible = showText;
    loadingLabel.Visible = showText;

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
    blockHitLabel.Visible = false;
    loadingLabel.Visible = false;

    GetTree().Paused = true;
    _anim.Play("Fade");
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    GetTree().Paused = false;
  }

  private void OnReturnedToMenu()
  {
    Transition();
  }

  private void OnLevelReset()
  {
    Transition(showText: false);
  }

  private void OnAnimationFinished(String animName)
  {
    _events.EmitSignal(nameof(Events.FadePlayerFaded));
  }
}
