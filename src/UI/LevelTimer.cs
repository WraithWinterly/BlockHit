using Godot;
using System;

public class LevelTimer : Label
{
  private Events _events;
  private Player _player;
  private Timer _timer;

  private Timer _levelTimer;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _events.Connect(nameof(Events.LevelStarted), this, nameof(LevelStarted));
    _events.Connect(nameof(Events.LevelReset), this, nameof(LevelReset));
  }

  public override void _PhysicsProcess(float delta)
  {
    if (_levelTimer != null)
    {
      Text = $"Timer: {Convert.ToInt32(_levelTimer.TimeLeft).ToString()}";
    }
    else
    {
      Text = "Timer: 0";
    }
  }

  private void LevelStarted()
  {
    _levelTimer = (Owner as Main).GetLevelTimer();
  }

  private async void LevelReset()
  {
    await ToSignal(_events, nameof(Events.FadePlayerFaded));
    Text = "Timer: 0";
    _levelTimer = null;
  }
}
