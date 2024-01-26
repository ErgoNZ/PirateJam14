using Godot;
using System;

public partial class workstation : Node2D
{
	int LanternLV = 1, LampLV = 1, ResourceLV = 1, CampfireLV = 1;
	bool campfireUnlock = false, LampUnlock = false;
	bool PlayerInRange = false;
	bool PlayerInteracting = false;
	Area2D ForcedInteractionRange;
	Control GUIContainer;
	Label workstationLbl, lanternLevelLbl, lampLevelLbl, campfireLevelLbl, resourceLevelLbl;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ForcedInteractionRange = GetNode<Area2D>("ForcedInteractionRange");
		GUIContainer = GetNode<Control>("displayGUIControl");
		workstationLbl = GetNode<Label>("workstationLabel");
		lanternLevelLbl = GetNode<Label>("displayGUIControl/lblLanternLevel");
		lampLevelLbl = GetNode<Label>("displayGUIControl/lblLampLevel");
		campfireLevelLbl = GetNode<Label>("displayGUIControl/lblCampfireUnlock");
		resourceLevelLbl = GetNode<Label>("displayGUIControl/lblResourcelvl");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lanternLevelLbl.Text = "Lantern Upgrade Level: " + LanternLV;
		resourceLevelLbl.Text = "Resource Collection Level: " + ResourceLV;
		if (LampUnlock)
		{
			lampLevelLbl.Text = "Lamp Upgrade Level: " + LampLV;
		}
		else
		{
			lampLevelLbl.Text = "Unlock Lamp Post: ";
		}
		if(campfireUnlock)
		{
			campfireLevelLbl.Text = "Campfire Unlocked";
		}
		else
		{
			campfireLevelLbl.Text = "Unlock Campfire: ";
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
		doLanternUpgrade();
	}
	private void lampUpgradeCheck()
	{
		if(!LampUnlock)
		{
			LampUnlock = true;
		}
		else
		{
			doLampUpgrade();
		}
		
	}
	private void campfireUpgradeCheck()
	{
		if(!campfireUnlock)
		{
			campfireUnlock = true;
		}
		else
		{
			doCampfireUpgrade();
		}
	}
	private void resourceUpgradeCheck()
	{
		doResourceUpgrade();
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

