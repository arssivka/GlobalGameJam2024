using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class EnemySpawner : Node3D
{
	[Export] public float MinSpawnDistance = 25;
	[Export] public NodePath RoutesNodePath;
	[Export] public NodePath SpawnPointsNodePath;
	[Export] public PackedScene EnemyScene { get; set; }

	private Array<PathFollow3D> _routes = new Array<PathFollow3D>();
	private Array<Node3D> _spawnPoints = new Array<Node3D>();
	
	public override void _Ready()
	{
		var mappedRoutes = GetNode<Node3D>(RoutesNodePath).GetChildren()
			.Where(child => (child.GetChildCount() > 0 && child.GetChild(0) is PathFollow3D))
			.Select(child => child.GetChild(0))
			.Cast<PathFollow3D>();
		
		foreach(var route in mappedRoutes)
		{
			_routes.Add(route);
		}
		
		var mappedSpawnPoints = GetNode<Node3D>(SpawnPointsNodePath).GetChildren()
			.Where(child => child is Node3D)
			.Select(child => child)
			.Cast<Node3D>();
		
		foreach(var spawnPoint in mappedSpawnPoints)
		{
			_spawnPoints.Add(spawnPoint);
		}
		
		GetNode<GameState>("/root/GameState").EnemySpawner = this;
	}
	
	public void SpawnEnemy(Vector3 currentPosition)
	{
		var enemy = EnemyScene.Instantiate<Enemy>();
		do
		{
			enemy.GlobalPosition = _spawnPoints[(int)(GD.Randi() % _spawnPoints.Count)].GlobalPosition;
		} while (enemy.Position.DistanceSquaredTo(currentPosition) < MinSpawnDistance * MinSpawnDistance);

		var i = (int)(GD.Randi() % _routes.Count);
		if (i >= 0 && i < _routes.Count)
		{
			enemy.RoutePath = _routes[i].GetPath();
		}
		GetNode<Node3D>(RoutesNodePath).AddChild(enemy);
		
	}
}
