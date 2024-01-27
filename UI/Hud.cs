using Godot;
using System;

public partial class Hud : Control
{
	private Label ScoreCountLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScoreCountLabel = GetNode<Label>("ScoreContainer/ScoreCount");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnScoreCountChanged(int scoreCount)
	{
		ScoreCountLabel.Text = scoreCount.ToString();
	}
}
