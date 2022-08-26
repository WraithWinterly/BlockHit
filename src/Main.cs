using Godot;
using System;

public class Main : Node
{
  public override void _Ready()
  {
    VisualServer.SetDefaultClearColor(Colors.Black);
  }

  public Player GetPlayer()
  {
    return GetNode<Player>("Basket/Player");
  }

}
