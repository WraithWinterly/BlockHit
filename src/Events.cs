using Godot;
using System;

public class Events : Node
{
  [Signal] public delegate void LevelStarted();

  [Signal] public delegate void PlayerDied();

  [Signal] public delegate void LevelReset();

  [Signal] public delegate void FadePlayerFaded();
}
