using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export]
	public Vector3 MovementTargetPosition = new Vector3(-3.0f, 0.0f, 2.0f);
	[Export]
	private float MovementSpeed { get; set; } = 400.0f;
	[Export]
	public int JumpImpulse { get; set; } = 30;
	[Export]
	public int FallAcceleration { get; set; } = 200;
	
	private Vector3 _targetVelocity = Vector3.Zero;
	
	private NodePath _exclamationMarkPath = new NodePath("Pivot/Exclamation_Mark");

	private NavigationAgent3D _navigationAgent;
	private bool _navigationLocked = false;
	private Node3D _playerNode;
	
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
			var exclamationMark = GetNode<Node3D>(_exclamationMarkPath);
			exclamationMark.Visible = !exclamationMark.Visible;
		}
		
		if (!_navigationLocked && _playerNode is not null && MovementTarget != _playerNode!.Position)
		{
			MovementTarget = _playerNode!.Position;
			_navigationLocked = true;
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (!_navigationAgent.IsNavigationFinished())
		{
			Vector3 currentAgentPosition = GlobalTransform.Origin;
			Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();

			Vector3 horisontalSpeed = Vector3.Zero;
			horisontalSpeed = currentAgentPosition.DirectionTo(nextPathPosition) * MovementSpeed * (float)delta;
			_targetVelocity.X = horisontalSpeed.X;
			_targetVelocity.Z = horisontalSpeed.Z;
			
			if (IsOnFloor() && !horisontalSpeed.IsZeroApprox())
			{
				_targetVelocity.Y = JumpImpulse;
			}
		}
		else
		{
			_targetVelocity.X = 0;
			_targetVelocity.Z = 0;
		}

		_targetVelocity.Y -= FallAcceleration * (float)delta;
	
		Vector3 direction = -_targetVelocity;
		direction.Y = 0;
		if (!direction.IsZeroApprox())
		{
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}
		Velocity = _targetVelocity;
		MoveAndSlide();
	}

	private async void ActorSetup()
	{
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

		// Now that the navigation map is no longer empty, set the movement target.
		MovementTarget = MovementTargetPosition;
	}
	
	private void OnVisionAreaBodyEntered(Node3D body)
	{
		_playerNode = body;
	}
	
	private void OnVisionAreaBodyExited(Node3D body)
	{
		_playerNode = null;
	}

	private void OnNavigationLockTimerTimeout()
	{
		_navigationLocked = false;
	}
}
