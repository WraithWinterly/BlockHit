using Godot;
using System;

public class Events : Node
{
  [Signal] public delegate void Start();
  [Signal] public delegate void PlayerDied();
  [Signal] public delegate void ResetGame();
}
