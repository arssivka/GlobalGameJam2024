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
		var ScoreScene = GD.Load<PackedScene>("res://UI/ScorePrototype.tscn");
		for(int i = 0; i < GlobalState.Highscore.Count; ++i)
		{
			var ScoreInstance = ScoreScene.Instantiate() as ScorePrototype;
			
			ScoreInstance.Init(i+1, GlobalState.Highscore[i]);
			ScoresContainer.AddChild(ScoreInstance);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMenuButtonPressd()
	{
		GetTree().ChangeSceneToFile("res://UI/MainMenu.tscn");
	}
}
