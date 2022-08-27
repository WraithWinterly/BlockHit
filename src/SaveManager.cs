using Godot;
using Godot.Collections;
using System;

public class SaveManager : Node
{
  public Dictionary<string, int> Save { get => _save; }

  private Dictionary<string, int> _save = new Dictionary<string, int>() {
      {"LastLevel", 0},
    };

  private Events _events;

  public override void _Ready()
  {
    base._Ready();
    _events = GetTree().Root.GetNode<Events>("Main/Events");

    _events.Connect(nameof(Events.LevelCompleted), this, nameof(OnLevelCompleted));
    _events.Connect(nameof(Events.LevelSelectorChanged), this, nameof(OnLevelSelectorChanged));
    LoadGame();
  }

  private void SaveGame()
  {
    var saveGame = new File();
    saveGame.Open("user://blockhit.save", File.ModeFlags.Write);
    saveGame.StoreLine(JSON.Print(_save));
    saveGame.Close();
    GD.Print("Saved: ", _save);
  }

  private void LoadGame()
  {
    var saveGame = new File();
    if (!saveGame.FileExists("user://blockhit.save"))
      return; // Error! We don't have a save to load.

    saveGame.Open("user://blockhit.save", File.ModeFlags.Read);

    _save = new Godot.Collections.Dictionary<string, int>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);
    GD.Print("Loaded: ", _save);
    saveGame.Close();
  }

  private void OnLevelCompleted()
  {
    var player = (Owner as Main).GetPlayer();
    var levelManager = (Owner as Main).GetNode<LevelManager>("LevelManager");
    int currLevel = (int)levelManager.CurrentLevel;

    var keyName = $"HighScore{levelManager.CurrentLevel.ToString()}";
    if (_save.ContainsKey(keyName))
    {
      if (player.Score > (int)_save[keyName])
        _save[keyName] = player.Score;
    }
    else
    {
      _save.Add(keyName, player.Score);
    }

    SaveGame();
  }

  private void OnLevelSelectorChanged(LevelManager.Levels level)
  {
    _save["LastLevel"] = (int)level;
    SaveGame();
  }
}
