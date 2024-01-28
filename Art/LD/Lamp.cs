using Godot;
using System;
using System.Drawing;

public partial class Lamp : Node3D
{
	[Export] public bool IsBlue = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (IsBlue)
		{
			GetNode<OmniLight3D>("OmniLight3D").LightColor = new Godot.Color(Godot.Colors.Blue);

			GetNode<MeshInstance3D>("MeshInstanceRed").Hide();
			GetNode<MeshInstance3D>("MeshInstanceBlue").Show();
		}
		else
		{
			GetNode<MeshInstance3D>("MeshInstanceRed").Show();
			GetNode<MeshInstance3D>("MeshInstanceBlue").Hide();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
