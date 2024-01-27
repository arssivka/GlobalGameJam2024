using Godot;
using System;

public partial class Patient : StaticBody3D
{
	public enum State
	{
		Idle,
		Tickled,
		Laugh,
	};

	public State CurrentState = State.Idle;
	[Export]
	public float TicklingTime = 0.0f;
	public const float TicklingTimeLimit = 5.0f;


	private Node3D IdleMesh;
	private Node3D TickledMesh;
	private Node3D LaughMesh;

	private Hero TicklingHero;
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
		if (CurrentState == State.Tickled && TicklingHero != null)
		{
			TicklingTime += (float)delta;
			if (TicklingTime >= TicklingTimeLimit)
			{
				OnStateSchanged(State.Laugh);
				TicklingHero.OnTicklingSucceed();
			}
			else
			{
				// paste update visual ticking progress here
			}
		}
	}

	private void OnBodyTickingZoneEntered(Node3D body)
	{
		TicklingHero = body as Hero;
		if (TicklingHero != null)
		{
			OnStateSchanged(State.Tickled);
			TicklingHero.StartTickling(this);
		}
	}

	private void OnBodyTickingZoneLeft(Node3D body)
	{
		TicklingHero = body as Hero;
		if (TicklingHero != null)
		{
			TicklingHero.StopTickling();
		}
		TicklingHero = null;
	}

	private void OnStateSchanged(State newState)
	{
		if (CurrentState == newState)
		{
			return;
		}
		CurrentState = newState;
		if (newState == State.Laugh)
		{
			IdleMesh.Hide();
			TickledMesh.Hide();
			LaughMesh.Show();
		}
		else if (newState == State.Tickled)
		{
			IdleMesh.Hide();
			TickledMesh.Show();
			LaughMesh.Hide();
		}
		else if (newState == State.Idle)
		{
			IdleMesh.Show();
			TickledMesh.Hide();
			LaughMesh.Hide();
		}
	}

}
