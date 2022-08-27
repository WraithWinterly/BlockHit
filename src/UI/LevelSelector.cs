using Godot;
using System;

public class LevelSelector : Control
{
  private Events _events;

  private Button _basketButton;
  private Button _solidButton;
  private Button _blockButton;

  public override void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");

    _basketButton = GetNode<Button>("VBoxContainer/Basket/Button");
    _solidButton = GetNode<Button>("VBoxContainer/Solid/Button");
    _blockButton = GetNode<Button>("VBoxContainer/Block/Button");

    _basketButton.Connect("pressed", this, nameof(OnBasketButtonPressed));
    _solidButton.Connect("pressed", this, nameof(OnSolidButtonPressed));
    _blockButton.Connect("pressed", this, nameof(OnBlockButtonPressed));
  }

  private void OnBasketButtonPressed()
  {
    _events.EmitSignal(nameof(Events.LevelSelectorChanged), LevelManager.Levels.Basket);
  }

  private void OnSolidButtonPressed()
  {
    _events.EmitSignal(nameof(Events.LevelSelectorChanged), LevelManager.Levels.Solid);
  }

  private void OnBlockButtonPressed()
  {
    _events.EmitSignal(nameof(Events.LevelSelectorChanged), LevelManager.Levels.Block);
  }


}
