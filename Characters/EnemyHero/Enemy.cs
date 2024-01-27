using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export]
	public Vector3 _movementTargetPosition = new Vector3(-3.0f, 0.0f, 2.0f);

	
	private NodePath ExclamationMarkPath = new NodePath("Pivot/Exclamation_Mark");

	private NavigationAgent3D _navigationAgent;

	private float _movementSpeed = 10.0f;
	
	public Vector3 MovementTarget
	{
		get { return _navigationAgent.TargetPosition; }
		set { _navigationAgent.TargetPosition = value; }
	}

	public override void _Ready()
	{
		base._Ready();

		_navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		// These values need to be adjusted for the actor's speed
		// and the navigation layout.
		_navigationAgent.PathDesiredDistance = 0.5f;
		_navigationAgent.TargetDesiredDistance = 0.5f;

		// Make sure to not await during _Ready.
		Callable.From(ActorSetup).CallDeferred();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (Input.IsActionJustReleased("ShowExclamationMark"))
		{
			var exclamationMark = GetNode<Node3D>(ExclamationMarkPath);
			exclamationMark.Visible = !exclamationMark.Visible;
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (_navigationAgent.IsNavigationFinished())
		{
			return;
		}

		Vector3 currentAgentPosition = GlobalTransform.Origin;
		Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

		Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * _movementSpeed;
		MoveAndSlide();
	}

	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		// Now that the navigation map is no longer empty, set the movement target.
		MovementTarget = _movementTargetPosition;
	}
}
