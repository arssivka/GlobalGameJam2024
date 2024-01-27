using Godot;
using System;
using System.Collections.Generic;

public partial class GameState : Node
{
    public int SCORE { get; set; } = 0;
    public List<int> Highscore = new List<int>();
    public EnemySpawner EnemySpawner;
    
    public void SaveResult()
    {
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

        foreach(int score in Highscore)
        {
            saveGame.StoreLine(score.ToString());
        }
    }

    public void LoadResult()
    {
        if (!FileAccess.FileExists("user://savegame.save"))
        {
            return; // Error! We don't have a save to load.
        }
        Highscore.Clear();
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
        while (!saveGame.EofReached())
        {
            String line = saveGame.GetLine();
            int res = 0;
            if(int.TryParse(line, out res))
            {
                Highscore.Add(res);
            }
        }
    }

    public void UpdateHighscore()
    {
        LoadResult();
        Highscore.Add(SCORE);
        Highscore.Sort((a, b) => b.CompareTo(a));
        if (Highscore.Count > 5)
        {
            Highscore.RemoveAt(Highscore.Count - 1);
        }
        SaveResult();
    }
}
