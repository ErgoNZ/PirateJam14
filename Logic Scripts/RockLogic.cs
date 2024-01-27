using Godot;
using System;

public partial class RockLogic : Node2D
{
	int RockAmount = 0;
	const int RockMin = 1;
	int RockMax = 4;
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
            int rockMultiplier = (int)workstation.ResourceLV;
            var w = RockMin * rockMultiplier;
            if (RockMax + rockMultiplier < (RockMin * rockMultiplier))
            {
                RockMax = w;
            }
            RockAmount = Random.Next(RockMin * rockMultiplier, RockMax + rockMultiplier);
            GD.Print("Rock was gathered and gave " + RockAmount +" rocks!");
			signals.EmitSignal("Rock", RockAmount);
		}
	}
}
