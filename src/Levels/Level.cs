using Godot;
using System;

public class Level : Node2D
{
  [Export] public AudioStream musicStream;
  [Export] public int Seed = 10;

  public Vector2 SpawnTopLeft = new Vector2();
  public Vector2 SpawnBottomRight = new Vector2();

  public override void _Ready()
  {
    base._Ready();
    SpawnTopLeft = GetNode<Position2D>("SpawnTopLeft").GlobalPosition;
    SpawnBottomRight = GetNode<Position2D>("SpawnTopRight").GlobalPosition;
  }
}
