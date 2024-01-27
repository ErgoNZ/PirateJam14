using Godot;
using System;
using static LightSourceList;

public partial class torch : Node2D
{
	LightSignals LightSignals;
	Area2D LightZone;
	PointLight2D Light;
	Random Random = new Random();
	PackedScene Eyes;
	LightInfo lightInfo;
	bool PlayerInLight = false;
	float LightPercentage = 1;
	float LightZoneDefault;
	float LightTexureScaleDefault;
	int LightID;
	int EyeCount = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LightZone = GetNode<Area2D>("Area2D");
		Light = GetNode<PointLight2D>("PointLight2D");
		LightSignals = GetNode<LightSignals>("/root/LightSignals");
		LightZoneDefault = LightZone.Scale.X;
		LightTexureScaleDefault = Light.TextureScale;
		LightZone.AddToGroup("LightAreas");
		LightSignals.TorchDied += DropLightID;
		lightInfo = new LightInfo();
		lightInfo.Light = this;
		lightInfo.Active = true;
		Lights.Add(lightInfo);
		LightID = Lights.Count - 1;
		Eyes = GD.Load<PackedScene>("res://Objects/Eyes.tscn");
	}
	public void SpawnEyes()
	{
		Node2D SpawnedEyes;
		Vector2 Displacement = new(Random.Next(-150, 150), Random.Next(-150, 150));
		SpawnedEyes = (Node2D)Eyes.Instantiate();
		AddChild(SpawnedEyes);
		SpawnedEyes.Position = Displacement;
		SpawnedEyes.AddToGroup("Eyes");
		EyeCount++;
	}
	private void LightTick()
	{
		LightPercentage -= (0.01f + (0.01f * EyeCount));
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
		LightSignals.EmitSignal("TorchDied",LightID);
		Lights.RemoveAt(LightID);
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
	private void EyesDissapear(Node2D Node)
	{
		EyeCount--;
	}

	private void DropLightID(int ID)
	{
		if (ID != LightID)
		{
			if(ID <= LightID)
			{
				LightID--;
			}
		}
	}
}
