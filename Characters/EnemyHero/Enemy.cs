using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	private NodePath ExclamationMarkPath = new NodePath("Pivot/Exclamation_Mark");

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (Input.IsActionJustReleased("ShowExclamationMark"))
		{
			var exclamationMark = GetNode<Node3D>(ExclamationMarkPath);
			exclamationMark.Visible = !exclamationMark.Visible;
		}
	}
}
