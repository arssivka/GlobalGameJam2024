using Godot;

public partial class PauseMenu : Control
{
    private NodePath ResumeButtonPath = "Control/ResumeButton";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause"))
    	{
			SwitchPause();
		}
	}

	public void SwitchPause()
	{
		if (!GetTree().Paused)
		{
			GetTree().Paused = true;
			this.Show();
            GetNode<Button>(ResumeButtonPath).CallDeferred("grab_focus");
		}
		else
		{
			GetTree().Paused = false;
			this.Hide();
		}
	}
	public void OnMenuButtonPressd()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://UI/MainMenu.tscn");
	}

	public void OnResumeButtonPressd()
	{
		SwitchPause();
	}
}
