using Godot;
using System;

public partial class Darkness : TileMap
{
	Vector2I CurrentCell = new (0, 0);
	Vector2I Default = new (0, 0);
	Random rnd = new Random();
	Area2D LightChecker;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//darkness mechanics, may be removed.
		LightChecker = GetNode<Area2D>("Area2D");
		
		for (int i = 0; i < 20; i++) 
		{
  			for (int x = 0; x < 5; x++) 
			{
				CurrentCell = new(i, x);
				LightChecker.Position = new(i * 100, x * 100);
				
  				SetCell( 2, CurrentCell, rnd.Next(0, 2), Default, rnd.Next(0, 4));
				SetCell( 1, CurrentCell, 2, Default, rnd.Next(0, 4));
				SetCell( 0, CurrentCell, 3, Default, rnd.Next(0, 4));
			}
		}
		
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for (int i = 0; i < 20; i++) 
		{
  			for (int x = 0; x < 5; x++) 
			{
				CurrentCell = new(i, x);
				LightChecker.Position = new(i * 100, x * 100);
				foreach (Area2D area in LightChecker.GetOverlappingAreas()){
					if (area.IsInGroup("LightAreas")){
						SetCell( 2, CurrentCell, -1, Default, -1);
						SetCell( 1, CurrentCell, -1, Default, -1);
						SetCell( 0, CurrentCell, -1, Default, -1);
						continue;
					}
				}
			}
		}
	}
}
