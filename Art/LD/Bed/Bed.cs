using Godot;
using System;

public partial class Bed : Node3D
{

	private bool isHeroInArea = false;
	private string heroName = "";

	public override void _Ready()
	{
		base._Ready();

		GetNode<Area3D>("TickleArea").BodyEntered += OnBodyEntered;
		GetNode<Area3D>("TickleArea").BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	private void OnBodyEntered(Node3D body)
	{
		GD.Print(body.Name, " entered PickleArea");
	}

	private void OnBodyExited(Node3D body)
	{
		GD.Print(body.Name, " exited PickleArea");
	}
}
