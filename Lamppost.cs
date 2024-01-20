using Godot;
using System;
using System.Collections.Generic;

public partial class Lamppost : Node2D
{
	LightSignals LightSignals;
	Area2D LightZone, ForcedInteractionRange;
	PointLight2D Light;
	ProgressBar FuelBar;
	Label GrassLbl, WoodLbl;
    ResourceSignals signals;
    List<int> lampID = new List<int>();
    int ID;

    bool PlayerInLight = false;
	bool PlayerInRange = false;
    int grassCost = 1;
    int woodCost = 1;
	float LightPercentage = 1;
	float LightZoneDefault;
	float LightTexureScaleDefault;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        ID++;
        lampID.Add(ID);

       /* int grass = Player.Inventory.inventoryScript.Grass;*/

		LightZone = GetNode<Area2D>("Area2D");
		Light = GetNode<PointLight2D>("PointLight2D");
		LightSignals = GetNode<LightSignals>("/root/LightSignals");
		FuelBar = GetNode<ProgressBar>("ProgressBar");
		ForcedInteractionRange = GetNode<Area2D>("ForcedInteractionRange");
		GrassLbl = GetNode<Label>("GrassLabel");
        WoodLbl = GetNode<Label>("WoodLabel");
        LightZoneDefault = LightZone.Scale.X;
		LightTexureScaleDefault = Light.TextureScale;
    }

	private void LightTick()
	{
        LightPercentage -= 0.01f;
        LightZone.Scale = new(LightZoneDefault * LightPercentage, LightZoneDefault * LightPercentage);
        Light.TextureScale = LightTexureScaleDefault * LightPercentage;
        FuelBar.Value = LightPercentage;
        if (LightZone.Scale.X <= 0)
        {
            TorchDespawn();
        }
    }

	private void TorchDespawn()
	{
        QueueFree();
        GD.Print("Torch has faded away!");
        if (PlayerInLight)
		{
			LightSignals.EmitSignal("RemoveLightArea");
		}
    }
	private void OnPlayerEnter(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			PlayerInLight = true;
			FuelBar.Visible = true;
            LightSignals.EmitSignal("AddLightArea");
        }
	}
	private void OnPlayerExit(Node2D body)
	{
        if (body.IsInGroup("Player"))
        {
            PlayerInLight = false;
            FuelBar.Visible = false;
            LightSignals.EmitSignal("RemoveLightArea");
        }
    }

    private void PlayerInInteractionRange(Node2D body)
    {
		PlayerInRange = true;
		FuelBar.Visible = PlayerInRange;
		GrassLbl.Visible = PlayerInRange;
		WoodLbl.Visible = PlayerInRange;
    }
    private void PlayerLeftInteractionRange(Node2D body)
    {
        PlayerInRange = false;
        FuelBar.Visible = PlayerInRange;
        GrassLbl.Visible = PlayerInRange;
        WoodLbl.Visible = PlayerInRange;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Interact") && PlayerInRange == true)
        {
            GD.Print("interaction");
            GD.Print(ID.ToString());
            if (Player.Inventory.inventoryScript.Grass >= 1)
            {
                signals.EmitSignal("TakeGrass", grassCost);
            }

           
          
        }
        if (@event.IsActionPressed("ALTInteract") && PlayerInRange == true)
        {
            if (Player.Inventory.inventoryScript.Wood >= 1) signals.EmitSignal("TakeLog", woodCost);
        }
    }
}
