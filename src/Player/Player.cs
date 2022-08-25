using Godot;
using System;

public class Player : Node2D
{
  [Signal] public delegate void Scored();
  [Signal] public delegate void Died();

  private AudioStreamPlayer _scoreSound;
  private ScoreLight _scoreLight;

  public override void _Ready()
  {
    _scoreLight = GetNode<ScoreLight>("%ScoreLight");

    Connect(nameof(Scored), this, nameof(OnScored));
    _scoreSound = GetNode<AudioStreamPlayer>("ScoreSound");
  }

  private void OnScored()
  {
    _scoreSound.Play();
    _scoreLight.Play();
  }
}
