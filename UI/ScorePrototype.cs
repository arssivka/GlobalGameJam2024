using Godot;
using System;

public partial class ScorePrototype : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Init(int place, int score)
	{
		GetNode<Label>("Label").Text = place.ToString() + ": ";
		GetNode<Label>("ScoreCount").Text = score.ToString();
	}
}
