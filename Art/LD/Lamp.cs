using Godot;
using System;

public partial class Lamp : Node3D
{
	[Export] public Color LampColor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<OmniLight3D>("OmniLight3D").LightColor = LampColor;
		var m = GetNode<MeshInstance3D>("MeshInstance3D").GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		m.Emission = LampColor;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
