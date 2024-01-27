using Godot;
using System;

public partial class Patient : StaticBody3D
{
	
	private Node3D IdleMesh;
	private Node3D TickledMesh;
	private Node3D LaughMesh;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IdleMesh = GetNode<Node3D>("Pivot/MeshBox_Idle");
		TickledMesh = GetNode<Node3D>("Pivot/MeshBox_Tickled");
		LaughMesh = GetNode<Node3D>("Pivot/MeshBox_Laugh");

		TickledMesh.Hide();
		LaughMesh.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
