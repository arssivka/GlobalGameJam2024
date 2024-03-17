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
	public const float TicklingTimeLimit = 3.0f;
	[Export]
	public int BaseScoreCounnt = 5;

	private Node3D IdleMesh;
	private Node3D TickledMesh;
	private Node3D LaughMesh;
	
	private bool _enemySpawned = false;	

	private AudioStreamPlayer3D HihiPlayer = null;
	private AudioStreamPlayer3D HysteriaPlayer = null;
	
	private Hero TicklingHero;
	private GameState GlobalState = null;
	private Node3D TearsPivot = null;
	private TextureProgressBar ProgressBar = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IdleMesh = GetNode<Node3D>("Pivot/Dude_Sleep");
		TickledMesh = GetNode<Node3D>("Pivot/Dude_Tickled");
		LaughMesh = GetNode<Node3D>("Pivot/Dude_Laugh");
		HihiPlayer = GetNode<AudioStreamPlayer3D>("HihiPlayer");
		HysteriaPlayer = GetNode<AudioStreamPlayer3D>("HysteriaPlayer");
		TearsPivot = GetNode<Node3D>("ParticlesPivot");
		const string path = "ProgressBar/SubViewport/TextureProgressBar";
		ProgressBar = GetNode<TextureProgressBar>(path);

		TickledMesh.Hide();
		LaughMesh.Hide();

		GlobalState = GetNode<GameState>("/root/GameState");
		// Listen for AllPatientsTickled event and reset state when all patients are tickled
		GlobalState.AllPatientsTickled += ResetState;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (CurrentState == State.Tickled)
		{
			if (TicklingHero != null)
			{
				HihiPlayer.VolumeDb = -7;
				TicklingTime += (float)delta;
				if (TicklingTime >= TicklingTimeLimit)
				{
					OnStateChanged(State.Laugh);
					TicklingHero.OnTicklingSucceed(BaseScoreCounnt);
				}
				UpdateProgressBar();
			}
			else
			{
				HihiPlayer.VolumeDb = -17;
			}
		}
		if (CurrentState == State.Laugh)
		{
			TicklingTime += (float)delta;
			if (TicklingTime >= 3.0)
			{
				HysteriaPlayer.VolumeDb = -15;
			}
			else
			{
				HysteriaPlayer.VolumeDb = 0;
			}
		}
	}

	private void OnBodyTickingZoneEntered(Node3D body)
	{
		if (CurrentState == State.Laugh)
		{
			return;
		}
		TicklingHero = body as Hero;
		if (TicklingHero != null)
		{
			OnStateChanged(State.Tickled);
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

	private void OnStateChanged(State newState)
	{
		if (CurrentState == newState)
		{
			return;
		}
		CurrentState = newState;
		if (newState == State.Laugh)
		{
			TicklingTime = 0;
			IdleMesh.Hide();
			TickledMesh.Hide();
			LaughMesh.Show();
			HihiPlayer.Stop();
			HysteriaPlayer.Play();
			if (!_enemySpawned && GlobalState.EnemySpawner is not null)
			{
				_enemySpawned = true;
				GlobalState.EnemySpawner.SpawnEnemy(Position);
			}
			TearsPivot.Show();
		}
		else if (newState == State.Tickled)
		{
			IdleMesh.Hide();
			TickledMesh.Show();
			LaughMesh.Hide();
			HihiPlayer.Play();
			HysteriaPlayer.Stop();
		}
		else if (newState == State.Idle)
		{
			IdleMesh.Show();
			TickledMesh.Hide();
			LaughMesh.Hide();
			HihiPlayer.Stop();
			HysteriaPlayer.Stop();
			TearsPivot.Hide();
		}
	}

	private void OnTickledFlipTimerTimeout()
	{
		var scaleTickled = TickledMesh.Scale;
		scaleTickled.X = -scaleTickled.X;
		TickledMesh.Scale = scaleTickled;
	}

	private void OnLaughFlipTimerTimeout()
	{
		var scaleLaugh = LaughMesh.Scale;
		scaleLaugh.X = -scaleLaugh.X;
		LaughMesh.Scale = scaleLaugh;
	}

	private void UpdateProgressBar()
	{
		switch (CurrentState)
		{
			case State.Idle:
				ProgressBar.Visible = false;
				break;
			case State.Tickled:
				ProgressBar.Value = TicklingTime / TicklingTimeLimit * 100;
				ProgressBar.Visible = true;
				break;
			case State.Laugh:
				ProgressBar.Value = 100;
				ProgressBar.Visible = true;
				ProgressBar.TintProgress = new Color(0.858824f, 0.0196078f, 0.207843f, 1f);
				break;
		}
	}

	public void ResetState()
	{
		OnStateChanged(State.Idle);
		ProgressBar.Hide();
		ProgressBar.TintProgress = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		ProgressBar.Value = 0;
		TicklingTime = 0;
	}
}
