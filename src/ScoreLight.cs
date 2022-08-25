using Godot;
using System;

public class ScoreLight : Node2D
{
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
    base._Ready();
    _animationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
  }

  public void Play()
  {
    _animationPlayer.Stop();
    _animationPlayer.Play("Flash");
  }
}
