using Godot;
using System;

public partial class GameOver : Control
{
	private Label ScoreCountLabel;
	private GameState GlobalState = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalState = GetNode<GameState>("/root/GameState");
		ScoreCountLabel = GetNode<Label>("Control/VBoxContainer/HBoxContainer/ScoreCount");
		ScoreCountLabel.Text = GlobalState.SCORE.ToString();
		GlobalState.UpdateHighscore();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMenuButtonPressd()
	{
		GetTree().ChangeSceneToFile("res://UI/MainMenu.tscn");
	}

	public void OnNewGameButtonPressd()
	{
		GetTree().ChangeSceneToFile(GlobalState.LevelPath);
	}
}
