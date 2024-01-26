using Godot;
using System;
using static LightSourceList;

public partial class Campfire : Node2D
{
	LightSignals LightSignals;
	ResourceSignals ResourceSignals;
	Area2D LightZone, ForcedInteractionRange;
	PointLight2D Light;
	ProgressBar FuelBar;
	Label GrassLbl, WoodLbl;
	PackedScene Eyes;
	LightInfo lightInfo;
	bool PlayerInLight = false;
	bool PlayerInRange = false;
	int grassCost = 1;
	int woodCost = 1;
	float LightPercentage = 1;
	float LightZoneDefault;
	float LightTexureScaleDefault;
	float LightEnergyDefault;
	int EyeCount = 0;
	int LightID;
	bool Active = false;
	
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
		Eyes = GD.Load<PackedScene>("res://Objects/Eyes.tscn");
		LightZoneDefault = LightZone.Scale.X;
		LightTexureScaleDefault = Light.TextureScale;
		LightEnergyDefault = Light.Energy;
		LightZone.AddToGroup("LightAreas");
		lightInfo = new LightInfo();
		lightInfo.Light = this;
		lightInfo.Active = false;
		campfire = lightInfo;
	}

	private void LightTick()
	{
		if (LightPercentage > 0 && Active)
		{
			LightPercentage = LightPercentage - (0.005f + (0.005f * EyeCount));
			campfire.Active = true;
		}
		else
		{
			LightPercentage = 0;
		}
		LightZone.Scale = new(MathF.Abs(LightZoneDefault * LightPercentage), MathF.Abs(LightZoneDefault * LightPercentage));
		Light.TextureScale = MathF.Abs(LightTexureScaleDefault * LightPercentage);
		Light.Energy = MathF.Abs(LightEnergyDefault * LightPercentage);
		FuelBar.Value = LightPercentage;
		if (LightZone.Scale.X <= 0)
		{
			LightFaded();
		}
	}

	private void LightFaded()
	{
        campfire.Active = false;
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
		changeVisibility(PlayerInRange);
	}
	private void PlayerLeftInteractionRange(Node2D body)
	{
		PlayerInRange = false;
		changeVisibility(PlayerInRange);
	}

	private void changeVisibility(bool PlayerInRange)
	{
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
	private void _on_child_exiting_tree(Node node)
	{
		if (node.IsInGroup("Eyes"))
		{
			EyeCount--;
		}
	}
}
