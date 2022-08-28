using Godot;
using System;

public class LevelSelector : Control
{
  private Events _events;
  private LevelManager _levelManager;
  private SaveManager _saveManager;
  private Button _basketButton;
  private Button _solidButton;
  private Button _blockButton;

  private Label _basketLabel;
  private Label _solidLabel;
  private Label _blockLabel;

  public override async void _Ready()
  {
    _events = GetTree().Root.GetNode<Events>("Main/Events");
    _levelManager = GetTree().Root.GetNode<LevelManager>("Main/LevelManager");
    _saveManager = GetTree().Root.GetNode<SaveManager>("Main/SaveManager");

    _basketButton = GetNode<Button>("VBoxContainer/Basket/Button");
    _solidButton = GetNode<Button>("VBoxContainer/Solid/Button");
    _blockButton = GetNode<Button>("VBoxContainer/Block/Button");

    _basketLabel = GetNode<Label>("VBoxContainer/Basket/Button/HBoxContainer/Label");
    _solidLabel = GetNode<Label>("VBoxContainer/Solid/Button/HBoxContainer/Label");
    _blockLabel = GetNode<Label>("VBoxContainer/Block/Button/HBoxContainer/Label");

    _events.Connect(nameof(Events.ReturnedToMenu), this, nameof(UpdateText));

    _basketButton.Connect("pressed", this, nameof(OnBasketButtonPressed));
    _solidButton.Connect("pressed", this, nameof(OnSolidButtonPressed));
    _blockButton.Connect("pressed", this, nameof(OnBlockButtonPressed));

    await ToSignal(GetTree(), "physics_frame");
    await ToSignal(GetTree(), "physics_frame");

    UpdateText();
  }

  private void UpdateText()
  {
    if (_saveManager.Save.ContainsKey("HighScoreBasket"))
    {
      _basketLabel.Text = $"Basket: {_saveManager.Save["HighScoreBasket"]}";
    }
    if (_saveManager.Save.ContainsKey("HighScoreSolid"))
    {
      _solidLabel.Text = $"Solid: {_saveManager.Save["HighScoreSolid"]}";
    }
    if (_saveManager.Save.ContainsKey("HighScoreBlock"))
    {
      _blockLabel.Text = $"Block: {_saveManager.Save["HighScoreBlock"]}";
    }
    _basketLabel.Modulate = _levelManager.CurrentLevel == LevelManager.Levels.Basket ? Colors.Purple : Colors.White;
    _solidLabel.Modulate = _levelManager.CurrentLevel == LevelManager.Levels.Solid ? Colors.Purple : Colors.White;
    _blockLabel.Modulate = _levelManager.CurrentLevel == LevelManager.Levels.Block ? Colors.Purple : Colors.White;
  }

  private async void OnBasketButtonPressed()
  {
    if (_levelManager.CurrentLevel != LevelManager.Levels.Basket)
    {
      _events.EmitSignal(nameof(Events.LevelSelectorChanged), LevelManager.Levels.Basket);
    }
    await ToSignal(GetTree(), "physics_frame");
    await ToSignal(GetTree(), "physics_frame");
    UpdateText();
  }

  private async void OnSolidButtonPressed()
  {
    if (_levelManager.CurrentLevel != LevelManager.Levels.Solid)
    {
      _events.EmitSignal(nameof(Events.LevelSelectorChanged), LevelManager.Levels.Solid);
    }
    await ToSignal(GetTree(), "physics_frame");
    await ToSignal(GetTree(), "physics_frame");
    UpdateText();
  }

  private async void OnBlockButtonPressed()
  {
    if (_levelManager.CurrentLevel != LevelManager.Levels.Block)
    {
      _events.EmitSignal(nameof(Events.LevelSelectorChanged), LevelManager.Levels.Block);
    }
    await ToSignal(GetTree(), "physics_frame");
    await ToSignal(GetTree(), "physics_frame");
    UpdateText();
  }


}
