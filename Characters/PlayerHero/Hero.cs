using Godot;
using System;

public partial class Hero : CharacterBody3D
{
	public enum State
	{
		Normal,
		Tickling,
	};

	[Export]
	public const float Speed = 500.0f;
	[Export]
	public const float DashSpeed = 2000.0f;
	[Export]
	public const float JumpVelocity = 4f;
	[Export]
	public const float Gravity = 10f;
	[Export]
	public const float TickleJumpVelocity = 3.5f;
	[Export]
	public const float TickleGravity = 40f;
	public const float GroundedHeight = 0.5f;
	[Export]
	public const float JumpStartHeight = 0.1f;
	private float VerticalSpeed = 0.0f;
	private AudioStreamPlayer3D StepsSound = null;
	private State CurrentState = State.Normal;
	private Vector3 MoveDirection = new Vector3(0, 0, 0);
	private NodePath RunMeshPath = new NodePath("Pivot/Hero_Run");
	private NodePath IdleMeshPath = new NodePath("Pivot/Hero_Idle");
	private NodePath TickleMeshPath = new NodePath("Pivot/Hero_Tickle");
	private Node3D Victim = null;
	private Hud PlayerHud = null;
	private PauseMenu PauseMenu = null;
	private int ScoreCount = 0;
	private GameState GlobalState = null;
	private bool dashEnabled = false;
	private bool dashCooldown = false;
	private int NumPatients = 0;
	private int NumTicledPatients = 0;

	public void StartTickling(Node3D body)
	{
		ChangeState(State.Tickling);
		Victim = body;
	}

	public void StopTickling()
	{
		ChangeState(State.Normal);
		Victim = null;
	}

	public void OnTicklingSucceed(int scores)
	{
		ChangeState(State.Normal);
		Victim = null;
		ScoreCount += scores;
		GlobalState.SCORE = ScoreCount;
		PlayerHud.OnScoreCountChanged(ScoreCount);
		NumTicledPatients++;
	}

	private void ChangeState(State newState)
	{
		CurrentState = newState;
		if (CurrentState == State.Tickling)
		{
			GetNode<Node3D>(RunMeshPath).Hide();
			GetNode<Node3D>(IdleMeshPath).Hide();
			GetNode<Node3D>(TickleMeshPath).Show();
		}
		else
		{
			GetNode<Node3D>(RunMeshPath).Hide();
			GetNode<Node3D>(IdleMeshPath).Show();
			GetNode<Node3D>(TickleMeshPath).Hide();
		}
	}

	private void UpdateDirection()
	{
		if (!MoveDirection.IsZeroApprox())
		{
			Vector3 direction = -Velocity;
			direction.Y = 0;
			if(Victim != null)
			{
				direction = GlobalPosition - Victim.GlobalPosition;
				if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Z))
				{
					direction.Z = 0;
				}
				else
				{
					direction.X = 0;
				}
			}

			if (!direction.IsZeroApprox())
			{
				GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
			}
		}
	}

	private void UpdateMovementMeshes(double delta)
	{
		if (CurrentState != State.Normal)
		{
			return;
		}
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
		if (CurrentState == State.Normal)
		{
			if (!MoveDirection.IsZeroApprox())
			{
				if (pivotPosition.Y < JumpStartHeight)
				{
					Random random = new Random();
					VerticalSpeed = JumpVelocity;
					StepsSound.Play();
					StepsSound.PitchScale = 0.8f - (float)(random.NextDouble()) * 0.2f;
				}
			}
			VerticalSpeed = Mathf.Max(-JumpVelocity, VerticalSpeed - Gravity * (float)delta);
		}
		else if (CurrentState == State.Tickling)
		{
			if (pivotPosition.Y < JumpStartHeight)
			{
				VerticalSpeed = TickleJumpVelocity;
			}
			VerticalSpeed = Mathf.Max(-JumpVelocity, VerticalSpeed - TickleGravity * (float)delta);
		}
	
		pivotPosition.Y = Mathf.Max(0, pivotPosition.Y + VerticalSpeed * (float)delta);

		GetNode<Node3D>("Pivot").Position = pivotPosition;
	}

	public override void _Ready()
	{
		base._Ready();
		GetNode<Node3D>(TickleMeshPath).Hide();
		PlayerHud = GetNode<Hud>("HUD");
		PlayerHud.OnScoreCountChanged(ScoreCount);
		PauseMenu = GetNode<PauseMenu>("PauseMenu");
		StepsSound = GetNode<AudioStreamPlayer3D>("StepsSound");
		GlobalState = GetNode<GameState>("/root/GameState");
		GlobalState.SCORE = 0;
		ScoreCount = 0;

		var patientsRoot = GetTree().CurrentScene.FindChild("Patients");
		if (patientsRoot != null)
		{
			NumPatients = patientsRoot.GetChildCount();
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (NumTicledPatients == NumPatients)
		{
			GD.Print("Player ticled all patients, emitting AllPatientsTicled signal");
			GlobalState.EmitSignal(GameState.SignalName.AllPatientsTickled);
		}
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
		
		if (Input.IsActionPressed("Dash") && !dashCooldown && GlobalState.SCORE > 0)
		{
			ScoreCount -= 1;
			GlobalState.SCORE -= 1;
			PlayerHud.OnScoreCountChanged(ScoreCount);
			
			dashEnabled = true;
			dashCooldown = true;
			GetNode<Timer>("DashTimer").Start();
			GetNode<Timer>("CooldownTimer").Start();
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
		var speed = (dashEnabled) ? DashSpeed : Speed;
		Vector3 moveVelocity = MoveDirection * speed * (float)delta;
		Velocity = moveVelocity;
		MoveAndSlide();
	}
	
	private void OnHitAreaBodyEntered(Node3D body)
	{
		GetTree().ChangeSceneToFile("res://UI/GameOver.tscn");
	}

	private void OnDashTimeout()
	{
		dashEnabled = false;
	}

	private void OnColldownTimer()
	{
		dashCooldown = false;
	}
}
