using Godot;
using System;

public partial class workstation : Node2D
{
	int TorchLV = 1, LampLV = 1;
	bool PlayerInRange = false;
	bool PlayerInteracting = false;
	Area2D ForcedInteractionRange;
	Control GUIContainer;
	Label workstationLbl, torchLevelLbl, lampLevelLbl;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ForcedInteractionRange = GetNode<Area2D>("ForcedInteractionRange");
		GUIContainer = GetNode<Control>("displayGUIControl");
		workstationLbl = GetNode<Label>("workstationLabel");
		torchLevelLbl = GetNode<Label>("displayGUIControl/lblTorchLevel");
		lampLevelLbl = GetNode<Label>("displayGUIControl/lblLampLevel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		torchLevelLbl.Text = "Torch Upgrade Level: " + TorchLV;
		lampLevelLbl.Text = "Lamp Upgrade Level: " + LampLV;
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
			workstationLbl.Visible = PlayerInRange;
	}
	private void torchUpgradeCheck()
	{
		doTorchUpgrade();
	}
	private void lampUpgradeCheck()
    {
		
		doLampUpgrade();
    }
	private void doTorchUpgrade()
    {
		TorchLV++;
	}
	private void doLampUpgrade()
    {
		LampLV++;
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

