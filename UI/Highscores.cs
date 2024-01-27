using Godot;
using System;

public partial class Highscores : Control
{
	private VBoxContainer ScoresContainer;
	private HBoxContainer ScoresPrototype;
	private GameState GlobalState = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalState = GetNode<GameState>("/root/GameState");
		GlobalState.LoadResult();
		ScoresContainer = GetNode<VBoxContainer>("ScoresAlign/VBoxContainer");
		ScoresPrototype = GetNode<HBoxContainer>("ScoresAlign/VBoxContainer/ScorePrototype");
		foreach(var result in GlobalState.Highscore)
		{
			HBoxContainer resultScore = ScoresPrototype;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
