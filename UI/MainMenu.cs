using Godot;
using System;

public partial class MainMenu : Control
{
	
	private GameState GlobalState = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalState = GetNode<GameState>("/root/GameState");
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnStartButtonPressd()
	{
		GetTree().ChangeSceneToFile(GlobalState.LevelPath);
	}

	public void OnLeaderboardButtonPressd()
	{
		GetTree().ChangeSceneToFile("res://UI/highscores.tscn");
	}

	public void OnQuitButtonPressd()
	{
		GetTree().Quit();
	}



}
