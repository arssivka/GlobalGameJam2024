using Godot;
using System;

public partial class Hero : CharacterBody3D
{
	[Export]
	public const float Speed = 500.0f;
	[Export]
	public const float JumpVelocity = 4f;
	[Export]
	public const float Gravity = 10f;
	[Export]
	public const float GroundedHeight = 0.5f;
	[Export]
	public const float JumpStartHeight = 0.1f;
	private float VerticalSpeed = 0.0f;

	private Vector3 MoveDirection = new Vector3(0, 0, 0);
	private NodePath RunMeshPath = new NodePath("Pivot/Hero_Run");
	private NodePath IdleMeshPath = new NodePath("Pivot/Hero_Idle");
	private NodePath TickleMeshPath = new NodePath("Pivot/Hero_Tickle");
	private NodePath CameraPath = new NodePath("CameraPivot/Camera");

	private void UpdateDirection()
	{
		if (!MoveDirection.IsZeroApprox())
		{
			Vector3 direction = -Velocity;
			direction.Y = 0;
			if (!direction.IsZeroApprox())
			{
				GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
			}
		}
	}

	private void UpdateMovementMeshes(double delta)
	{
		if (MoveDirection.IsZeroApprox())
		{
			GetNode<Node3D>(RunMeshPath).Hide();
			GetNode<Node3D>(IdleMeshPath).Show();
		}
		else
		{
			Vector3 pivotPosition = GetNode<Node3D>("Pivot").Position; 
			if (pivotPosition.Y < GroundedHeight)
			{
				GetNode<Node3D>(RunMeshPath).Hide();
				GetNode<Node3D>(IdleMeshPath).Show();
			}
			else
			{
				GetNode<Node3D>(IdleMeshPath).Hide();
				GetNode<Node3D>(RunMeshPath).Show();
			}
		}
	}

	private void UpdateJumping(double delta)
	{
		Vector3 pivotPosition = GetNode<Node3D>("Pivot").Position; 
		if (!MoveDirection.IsZeroApprox())
		{
			if (pivotPosition.Y < JumpStartHeight)
			{
				VerticalSpeed = JumpVelocity;
			}
		}
		VerticalSpeed = Mathf.Max(-JumpVelocity, VerticalSpeed - Gravity * (float)delta);

		pivotPosition.Y = Mathf.Max(0, pivotPosition.Y + VerticalSpeed * (float)delta);

		GetNode<Node3D>("Pivot").Position = pivotPosition;
	}

	public override void _Ready()
	{
		base._Ready();
		GetNode<Node3D>(TickleMeshPath).Hide();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

	}

	private void ListenInput()
	{
		if (Input.IsActionPressed("RunUp"))
		{
			MoveDirection.Z = -1;
		}
		else if (Input.IsActionPressed("RunDown"))
		{
			MoveDirection.Z = 1;
		}
		else
		{
			MoveDirection.Z = 0;			
		}

		if (Input.IsActionPressed("RunRight"))
		{
			MoveDirection.X = 1;
		}
		else if (Input.IsActionPressed("RunLeft"))
		{
			MoveDirection.X = -1;
		}
		else
		{
			MoveDirection.X = 0;			
		}

		MoveDirection = MoveDirection.Normalized();
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		ListenInput();
		UpdateMovementMeshes(delta);
		UpdateDirection();
		UpdateJumping(delta);
		Vector3 moveVelocity = MoveDirection * Speed * (float)delta;
		Velocity = moveVelocity;
		MoveAndSlide();
	}
}
