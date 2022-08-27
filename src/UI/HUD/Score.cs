using Godot;
using System;

public class Score : Label
{
  private Events _events;
  private Player _player;
  private ScoreLight _scoreLight;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _events.Connect(nameof(Events.LevelStarted), this, nameof(LevelStarted));
    _events.Connect(nameof(Events.LevelReset), this, nameof(LevelReset));
  }

  private void LevelStarted()
  {
    _player = (Owner as Main).GetPlayer();
    _scoreLight = GetNode<ScoreLight>("Control/ScoreLight");
    _player.Connect(nameof(Player.Scored), this, nameof(OnScored));
  }

  private async void LevelReset()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    Text = 0.ToString();
  }

  private async void OnScored()
  {
    await ToSignal(GetTree(), "physics_frame");
    Text = _player.Score.ToString();
    _scoreLight.Play();
  }
}
