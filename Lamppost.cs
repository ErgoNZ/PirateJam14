using Godot;
using System;
using System.Collections.Generic;

public partial class Lamppost : Node2D
{
	LightSignals LightSignals;
    ResourceSignals ResourceSignals;
	Area2D LightZone, ForcedInteractionRange;
	PointLight2D Light;
	ProgressBar FuelBar;
	Label GrassLbl, WoodLbl;
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
		LightZone = GetNode<Area2D>("Area2D");
		Light = GetNode<PointLight2D>("PointLight2D");
		LightSignals = GetNode<LightSignals>("/root/LightSignals");
        ResourceSignals = GetNode<ResourceSignals>("/root/ResourceSignals");
        FuelBar = GetNode<ProgressBar>("ProgressBar");
		ForcedInteractionRange = GetNode<Area2D>("ForcedInteractionRange");
		GrassLbl = GetNode<Label>("GrassLabel");
        WoodLbl = GetNode<Label>("WoodLabel");
        LightZoneDefault = LightZone.Scale.X;
		LightTexureScaleDefault = Light.TextureScale;
    }

	private void LightTick()
	{
        if (LightPercentage > 0)
        {
            LightPercentage -= 0.01f;
        }
        else
        {
            LightPercentage = 0;
        }
        LightZone.Scale = new(MathF.Abs(LightZoneDefault * LightPercentage), MathF.Abs(LightZoneDefault * LightPercentage));
        Light.TextureScale = MathF.Abs(LightTexureScaleDefault * LightPercentage);
        FuelBar.Value = LightPercentage;
        if (LightZone.Scale.X <= 0)
        {
            LightFaded();
        }
    }

	private void LightFaded()
	{
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
            LightSignals.EmitSignal("AddLightArea");
        }
	}
	private void OnPlayerExit(Node2D body)
	{
        if (body.IsInGroup("Player"))
        {
            PlayerInLight = false;
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
            GD.Print(Inventory.Grass);
            if (Inventory.Grass >= 1)
            {
                ResourceSignals.EmitSignal("Grass", -1);
                LightPercentage += 0.1f;
                if (LightPercentage > 1) LightPercentage = 1;
            }
        }
        if (@event.IsActionPressed("ALTInteract") && PlayerInRange == true)
        {
            GD.Print(Inventory.Wood);
            if (Inventory.Wood >= 1)
            {
                ResourceSignals.EmitSignal("Wood", -1);
                LightPercentage += 0.4f;
                if (LightPercentage > 1) LightPercentage = 1;
            }
        }
    }
}
