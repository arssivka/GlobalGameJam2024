using Godot;
using System;

public partial class Hero : CharacterBody3D
{
	[Export]
	public const float Speed = 500.0f;
	[Export]
	public const float JumpVelocity = 4.5f;

	private Vector3 MoveVelocity = new Vector3(0, 0, 0);
	private NodePath RunMeshPath = new NodePath("Pivot/Hero_Run");
	private NodePath IdleMeshPath = new NodePath("Pivot/Hero_Idle");
	private NodePath TickleMeshPath = new NodePath("Pivot/Hero_Tickle");
	private NodePath CameraPath = new NodePath("CameraPivot/Camera");

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		GetNode<Node3D>(TickleMeshPath).Hide();
		if (MoveVelocity.IsZeroApprox())
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

	private void ListenInput()
	{
		if (Input.IsActionPressed("RunUp"))
		{
			MoveVelocity.Z = -1;
		}
		else if (Input.IsActionPressed("RunDown"))
		{
			MoveVelocity.Z = 1;
		}
		else
		{
			MoveVelocity.Z = 0;			
		}

		if (Input.IsActionPressed("RunRight"))
		{
			MoveVelocity.X = 1;
		}
		else if (Input.IsActionPressed("RunLeft"))
		{
			MoveVelocity.X = -1;
		}
		else
		{
			MoveVelocity.X = 0;			
		}
		MoveVelocity = MoveVelocity.Normalized();
	}


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		ListenInput();
		Velocity = MoveVelocity * Speed * (float)delta;
        if (!MoveVelocity.IsZeroApprox())
        {
//            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(Position - MoveVelocity);
        }

		// Leave it to better time
		/*
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		*/
		MoveAndSlide();
	}
}
