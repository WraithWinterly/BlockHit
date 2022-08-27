using Godot;
using System;

public class Events : Node
{
  [Signal] public delegate void LevelStarted();

  [Signal] public delegate void PlayerDied();

  [Signal] public delegate void LevelCompleted();

  [Signal] public delegate void ReturnedToMenu();

  [Signal] public delegate void LevelReset();

  [Signal] public delegate void FadePlayerFaded();

  [Signal] public delegate void LevelSelectorChanged(LevelManager.Levels level);
}
