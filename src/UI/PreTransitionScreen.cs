using Godot;
using System;

public class PreTransitionScreen : Control
{
  private Events _events;
  private AnimationPlayer _anim;
  private Label _label;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _anim = GetNode<AnimationPlayer>("AnimationPlayer");
    _label = GetNode<Label>("Label");

    _events.Connect(nameof(Events.PlayerDied), this, nameof(OnPlayerDied));
    _events.Connect(nameof(Events.LevelCompleted), this, nameof(OnLevelComplete));
  }

  private async void OnPlayerDied()
  {
    _label.Text = "---Failed---";
    _anim.Play("Fade");
    await ToSignal(_anim, "animation_finished");
    _events.EmitSignal(nameof(Events.LevelReset));
  }

  private async void OnLevelComplete()
  {
    int score = (Owner as Main).GetPlayer().Score;
    _label.Text = $"---Completed ({score}!)---";
    _anim.Play("FadeComplete");
    await ToSignal(_anim, "animation_finished");
    _events.EmitSignal(nameof(Events.ReturnedToMenu));
  }

}
