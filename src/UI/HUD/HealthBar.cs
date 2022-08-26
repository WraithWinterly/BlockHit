using Godot;
using System;

public class HealthBar : ProgressBar
{
  private Player _player;

  public override void _Ready()
  {
    base._Ready();
    GetTree().Root.GetNode<Events>("Main/Events").Connect(nameof(Events.Start), this, nameof(Start));
  }

  private void Start()
  {
    _player = (Owner as Main).GetPlayer();
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);
    if (_player == null) return;
    Value = Mathf.Lerp((float)Value, _player.Health, 0.1f);
  }
}
