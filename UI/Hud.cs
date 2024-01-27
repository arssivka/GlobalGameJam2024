using Godot;
using System;

public partial class Hud : Control
{
	private Label ScoreCountLabel;
	private Control Hint;
	private bool HintHidden = false;
	private float TimeElapsed = 0.0f; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScoreCountLabel = GetNode<Label>("ScoreContainer/ScoreCount");
		Hint = GetNode<Control>("Hint");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (HintHidden)
		{
			return;
		}
		if (TimeElapsed > 5)
		{
			Hint.Hide();
			HintHidden = true;
		}
		else
		{
			TimeElapsed += (float)delta;
		}
	}

	public void OnScoreCountChanged(int scoreCount)
	{
		ScoreCountLabel.Text = scoreCount.ToString();
	}
}
