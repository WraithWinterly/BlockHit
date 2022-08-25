using Godot;
using System;

public class Score : Label
{
  private Player _player;
  private ScoreLight _scoreLight;

  private int _count = 0;

  public override void _Ready()
  {
    _player = GetNode<Player>("%Player");
    _scoreLight = GetNode<ScoreLight>("Control/ScoreLight");
    _player.Connect(nameof(Player.Scored), this, nameof(OnScored));
    Text = 0.ToString();
  }

  private void OnScored()
  {
    _count++;
    Text = _count.ToString();
    _scoreLight.Play();
  }
}
