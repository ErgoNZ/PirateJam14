using Godot;
using System;

public partial class workstation : Node2D
{
	bool PlayerInRange = false;
	Area2D ForcedInteractionRange;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ForcedInteractionRange = GetNode<Area2D>("ForcedInteractionRange");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void inInteractionArea(InputEvent @event)
    {
		PlayerInRange = true;
	}
	private void leftInteractionArea()
	{
		PlayerInRange = false;
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("Interact") && PlayerInRange == true)
		{

		}
	}
}

