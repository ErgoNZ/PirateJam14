using Godot;
using System;

public partial class ResourceNodeLogic : Node2D
{
	PackedScene Grass, Tree, Rock;
	Random Random = new Random();
    Node2D SpawnedResource;
    int Roll;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Grass = GD.Load<PackedScene>("res://Objects/Grass.tscn");
        Tree = GD.Load<PackedScene>("res://Objects/Tree.tscn");
        Rock = GD.Load<PackedScene>("res://Objects/Rock.tscn");
        Roll = Random.Next(1, 101);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        QueueFree();
        if (Roll > 0 && Roll < 60)
        {
            SpawnedResource = (Node2D)Grass.Instantiate();
            AddSibling(SpawnedResource);
            SpawnedResource.Position = this.Position;
        }
        if (Roll > 60 && Roll <= 80)
        {
            SpawnedResource = (Node2D)Tree.Instantiate();
            AddSibling(SpawnedResource);
            SpawnedResource.Position = this.Position;
        }
        if (Roll > 80 && Roll <= 100)
        {
            SpawnedResource = (Node2D)Rock.Instantiate();
            AddSibling(SpawnedResource);
            SpawnedResource.Position = this.Position;
        }
    }
}
