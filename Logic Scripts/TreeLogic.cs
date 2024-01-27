using Godot;
using System;

public partial class TreeLogic : Node2D
{
	int WoodAmount = 0;
	const int WoodMin = 1;
	int WoodMax = 4;
	public bool PlayerCanReach = false;
	Random Random = new Random();
	Label InteractIcon = new Label();
	ResourceSignals signals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		signals = GetNode<ResourceSignals>("/root/ResourceSignals");
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
            //deletes node
            QueueFree();
            int woodMultiplier = (int)workstation.ResourceLV;
            var w = WoodMin * woodMultiplier;
            if (WoodMax + woodMultiplier < (WoodMin * woodMultiplier))
            {
                WoodMax = w;
            }
            WoodAmount = Random.Next(WoodMin * woodMultiplier, WoodMax + woodMultiplier);
            GD.Print("Wood was gathered and gave " + WoodAmount +" wood!");
			signals.EmitSignal("Wood",WoodAmount);
		}
	}
}
