using Godot;
using System;

public partial class Tutorial : Node2D
{
	PackedScene PlayArea;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayArea = GD.Load<PackedScene>("res://PlayerArea.tscn");
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToPacked(PlayArea);
	}
}
