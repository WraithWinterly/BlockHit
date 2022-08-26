using Godot;
using System;

public class HealthBar : ProgressBar
{
  private Player _player;

  public override void _Ready()
  {
    base._Ready();
    _player = (Owner as Main).GetPlayer();
  }
  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);
    Value = Mathf.Lerp((float)Value, _player.Health, 0.1f);
  }
}
