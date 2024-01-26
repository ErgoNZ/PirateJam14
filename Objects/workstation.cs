using Godot;
using System;

public partial class workstation : Node2D
{
	public static float LanternLV = 1, LampLV = 1, ResourceLV = 1, CampfireLV = 1;
	
	int resourcesUpgradeCost = 1;

	int lanternWoodUpgradeCost = 1;
	int lanternGrassUpgradeCost = 1;

	int lampRockUpgradeCost = 1;
	int lampWoodUpgradeCost = 1;

	int campfireRockUpgradeCost = 1;
	int campfireWoodUpgradeCost = 1;

	public static bool campfireUnlock = false, LampUnlock = false;
	bool PlayerInRange = false;
	bool PlayerInteracting = false;
	Button lamppostButton;
	Label lamppostLabel;
	Area2D ForcedInteractionRange;
	Control GUIContainer;
	Label workstationLbl, lanternLevelLbl, lampLevelLbl, campfireLevelLbl, resourceLevelLbl;
	Label campfireWoodLbl, campfireRocksLbl;
	Label lanternWoodlbl, lanternGrassLbl;
	Label lampWoodlbl, lampRocksLbl;
	Label resourceWoodLbl, resourceGrassLbl, resourceRocksLbl;
	Sprite2D background;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lamppostButton = GetNode<Button>("../CanvasLayer/BoxContainer/Button/lamp_postButton");
		lamppostLabel = GetNode<Label>("../CanvasLayer/BoxContainer/Button/lamp_postButton/Label");
		background = GetNode<Sprite2D>("sprBlackBackground");
		ForcedInteractionRange = GetNode<Area2D>("ForcedInteractionRange");
		GUIContainer = GetNode<Control>("displayGUIControl");
		workstationLbl = GetNode<Label>("workstationLabel");
		lanternLevelLbl = GetNode<Label>("displayGUIControl/lblLanternLevel");
		lampLevelLbl = GetNode<Label>("displayGUIControl/lblLampLevel");
		campfireLevelLbl = GetNode<Label>("displayGUIControl/lblCampfireUnlock");
		resourceLevelLbl = GetNode<Label>("displayGUIControl/lblResourcelvl");

		campfireWoodLbl = GetNode<Label>("displayGUIControl/lblCampfireUnlock/lblCampfirelvlWood");
		campfireRocksLbl = GetNode<Label>("displayGUIControl/lblCampfireUnlock/lblCampfirelvlRocks");

		lanternWoodlbl = GetNode<Label>("displayGUIControl/lblLanternLevel/lblLanternlvlWood");
		lanternGrassLbl = GetNode<Label>("displayGUIControl/lblLanternLevel/lblLanternlvlGrass");

		lampWoodlbl = GetNode<Label>("displayGUIControl/lblLampLevel/lblLamplvlWood");
		lampRocksLbl = GetNode<Label>("displayGUIControl/lblLampLevel/lblLamplvlRocks");

		resourceWoodLbl = GetNode<Label>("displayGUIControl/lblResourcelvl/lblResourcelvlRocks");
		resourceGrassLbl = GetNode<Label>("displayGUIControl/lblResourcelvl/lblResourcelvlGrass");
		resourceRocksLbl = GetNode<Label>("displayGUIControl/lblResourcelvl/lblResourcelvlWood");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		resourceLevelLbl.Text = "Resource Collection Level: " + ResourceLV;
		resourceWoodLbl.Text = ": " + resourcesUpgradeCost.ToString();
		resourceGrassLbl.Text = ": " + resourcesUpgradeCost.ToString();
		resourceRocksLbl.Text = ": " + resourcesUpgradeCost.ToString();

		lanternLevelLbl.Text = "Lantern Level: " + LanternLV;
		lanternWoodlbl.Text = ": " + lanternWoodUpgradeCost.ToString();
		lanternGrassLbl.Text = ": " + lanternGrassUpgradeCost.ToString();

		if (LampUnlock)
		{
			lampLevelLbl.Text = "Lamp Level: " + LampLV;
			lampWoodlbl.Text = ": " + lampWoodUpgradeCost.ToString();
			lampRocksLbl.Text = ": " + lampRockUpgradeCost.ToString();
		}
		else
		{
			lampLevelLbl.Text = "Unlock Lamp Post: ";
			lampWoodlbl.Text = ": 5 ";
			lampRocksLbl.Text = ": 10";

		}
		if (campfireUnlock)
		{
			campfireLevelLbl.Text = "Campfire Level: " + CampfireLV;
			campfireWoodLbl.Text = ": " + campfireRockUpgradeCost.ToString();
			campfireRocksLbl.Text = ": " + campfireWoodUpgradeCost.ToString();
		}
		else
		{
			campfireLevelLbl.Text = "Unlock Campfire: ";
			campfireWoodLbl.Text = ": 10";
			campfireRocksLbl.Text = ": 15";
		}
	}

	private void inInteractionArea(Node2D body)
	{
		PlayerInRange = true;
		changeVisibility(PlayerInRange, PlayerInteracting);
	}
	private void leftInteractionArea(Node2D body)
	{
		PlayerInRange = false;
		PlayerInteracting = false;
		changeVisibility(PlayerInRange, PlayerInteracting);
	}
	private void changeVisibility(bool PlayerInRange, bool PlayerInteracting)
	{
			GUIContainer.Visible = PlayerInteracting;
			background.Visible = PlayerInteracting;
		if (!PlayerInteracting)
		{
			workstationLbl.Visible = PlayerInRange;
		}
		else
		{
			workstationLbl.Visible = false;
		}
	}
	private void lanternUpgradeCheck()
	{
		if(Inventory.Grass >= lanternGrassUpgradeCost && Inventory.Wood >= lanternWoodUpgradeCost)
        {
			Inventory.Grass = lanternGrassUpgradeCost;
			Inventory.Wood -= lanternWoodUpgradeCost;
			doLanternUpgrade();
		}
	}
	private void lampUpgradeCheck()
	{
		if (!LampUnlock)
		{
			if (Inventory.Wood >= 5 && Inventory.Rocks >= 10)
			{
				LampUnlock = true;
				lamppostButton.Visible = true;
				lamppostLabel.Visible = true;
				
			}	
		}
		else
		{
			if(Inventory.Wood >= lampWoodUpgradeCost && Inventory.Rocks >= lampRockUpgradeCost)
            {
				Inventory.Wood -= lampWoodUpgradeCost;
				Inventory.Rocks -= lampRockUpgradeCost;
				doLampUpgrade();
			}
			
		}
	}
	private void campfireUpgradeCheck()
	{
		if(!campfireUnlock)
		{
			if(Inventory.Wood >= 10 && Inventory.Rocks >= 15)
            {
				Inventory.Wood -= 10;
				Inventory.Rocks -= 15;
				campfireUnlock = true;
			}
		}
		else
		{
			if(Inventory.Wood >= campfireWoodUpgradeCost && Inventory.Rocks >= campfireRockUpgradeCost)
            {
				Inventory.Wood -= campfireWoodUpgradeCost;
				Inventory.Rocks -= campfireRockUpgradeCost;
				doCampfireUpgrade();
			}
		}
	}
	private void resourceUpgradeCheck()
	{
        if(Inventory.Wood >= resourcesUpgradeCost && Inventory.Rocks >= resourcesUpgradeCost && Inventory.Grass >= resourcesUpgradeCost)
		{ 
			Inventory.Wood -= resourcesUpgradeCost;
			Inventory.Rocks -= resourcesUpgradeCost;
			Inventory.Grass -= resourcesUpgradeCost;
			doResourceUpgrade();
		}
	}
	private void doResourceUpgrade()
	{

		ResourceLV++;
	}
	private void doLanternUpgrade()
	{
		LanternLV++;
	}
	private void doLampUpgrade()
	{
		LampLV++;
	}
	private void doCampfireUpgrade()
	{
		CampfireLV++;
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact") && PlayerInRange == true)
		{
			PlayerInteracting = true;
			changeVisibility(PlayerInRange, PlayerInteracting);
		}

	}
}

