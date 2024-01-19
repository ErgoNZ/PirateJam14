using Godot;
using System;

public partial class GrassLogic : Node2D
{
	int GrassAmount = 0;
	const int GrassMin = 1;
	const int GrassMax = 7;
	public bool PlayerCanReach = false;
	Random Random = new Random();
	Label InteractIcon = new Label();
	ResourceSignals signals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		signals = GetNode<ResourceSignals>("/root/ResourceSignals");
		GrassAmount = Random.Next(GrassMin, GrassMax);
		InteractIcon = (Label)GetNode("Label");		
	}

	private void PlayerInRange(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			InteractIcon.Visible = true;
			PlayerCanReach = true;
		}
	}

	private void PlayerLeftRange(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			InteractIcon.Visible = false;
			PlayerCanReach = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact") && PlayerCanReach == true)
		{
            QueueFree();
            GD.Print("Grass was gathered and gave " + GrassAmount +" grass!");
			signals.EmitSignal("GrassCollection", GrassAmount);		
		}
	}
}
