using Godot;
using System;

public partial class Label2 : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = "You survived for " + (Inventory.LightTicksPassed/2) + " seconds before losing your sanity to the darkness.";

    }
}
