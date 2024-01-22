using Godot;
using System;

public partial class torch : Node2D
{
	LightSignals LightSignals;
	Area2D LightZone;
	PointLight2D Light;
	bool PlayerInLight = false;
	float LightPercentage = 1;
	float LightZoneDefault;
	float LightTexureScaleDefault;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LightZone = GetNode<Area2D>("Area2D");
		Light = GetNode<PointLight2D>("PointLight2D");
		LightSignals = GetNode<LightSignals>("/root/LightSignals");
		LightZoneDefault = LightZone.Scale.X;
		LightTexureScaleDefault = Light.TextureScale;
	}

	private void LightTick()
	{
		LightPercentage -= 0.01f;
		LightZone.Scale = new(LightZoneDefault * LightPercentage, LightZoneDefault * LightPercentage);
		Light.TextureScale = LightTexureScaleDefault * LightPercentage;
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
}
