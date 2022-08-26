using Godot;
using System;

public class Score : Label
{
  private Player _player;
  private ScoreLight _scoreLight;

  public override void _Ready()
  {
    _player = (Owner as Main).GetPlayer();
    _scoreLight = GetNode<ScoreLight>("Control/ScoreLight");
    _player.Connect(nameof(Player.Scored), this, nameof(OnScored));
    Text = 0.ToString();
  }

  private void OnScored()
  {
    Text = _player.Score.ToString();
    _scoreLight.Play();
  }
}
