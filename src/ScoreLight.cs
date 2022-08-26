using Godot;
using System;

public class ScoreLight : Node2D
{
  private AnimationPlayer _anim;

  public override void _Ready()
  {
    base._Ready();
    _anim = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
  }

  public void Play()
  {
    _anim.Stop();
    _anim.Play("Flash");
  }
}
