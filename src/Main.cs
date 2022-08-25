using Godot;
using System;

public class Main : Node
{
  public override void _Ready()
  {
    VisualServer.SetDefaultClearColor(Colors.Black);
  }

}
