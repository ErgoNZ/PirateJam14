using Godot;
using System;

public partial class MainMenu : Node2D
{
	PackedScene PlayArea,Menu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayArea = GD.Load<PackedScene>("res://PlayerArea.tscn");
		Menu = GD.Load<PackedScene>("res://MainMenu.tscn");
	}
	
	private void StartGame()
	{
		GetTree().ChangeSceneToPacked(PlayArea);
	}
	private void QuitGame()
	{
		GetTree().Quit();
	}
	private void LoadMainMenu()
	{
		GetTree().ChangeSceneToPacked(Menu);
	}
}
