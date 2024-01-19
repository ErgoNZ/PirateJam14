using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float LanternEnergy = 3f;
	public int Hp = 100;
	PointLight2D Light = new PointLight2D();
	public int InLightZones = 0;
	Inventory inventory = new Inventory();
	ResourceSignals ResSignals;
	LightSignals LightSignals;
	Label Grass, Logs, Rocks;
	PackedScene Torch;

	public class Inventory
	{
		public int Rocks;
		public int Grass;
		public int Wood;
	}
	public override void _Ready()
	{
		// Called every time the node is added to the scene.
		// Initialization here.
		Light = (PointLight2D)GetNode("Lantern");
		InLightZones = 0;
        ResSignals = GetNode<ResourceSignals>("/root/ResourceSignals");
        LightSignals = GetNode<LightSignals>("/root/LightSignals");
		Grass = GetNode<Label>("../CanvasLayer/BoxContainer/GrassAmount");
        Logs = GetNode<Label>("../CanvasLayer/BoxContainer/LogAmount");
        Rocks = GetNode<Label>("../CanvasLayer/BoxContainer/RockAmount");
		Torch = GD.Load<PackedScene>("res://Objects/torch.tscn");
        ResSignals.WoodCollection += CollectLogs;
        ResSignals.GrassCollection += CollectGrass;
        ResSignals.RockCollection += CollectRocks;
		LightSignals.DecreaseInLightAmount += LightFaded;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void LightTick()
	{
		if (Light.TextureScale > 0 && InLightZones == 0)
		{
			Light.TextureScale -= 0.3f;
		}
		else if (InLightZones > 0 && Light.TextureScale < 10)
		{
			Light.TextureScale += 1f;
			Light.Energy = LanternEnergy;
		}
		if (Light.TextureScale > 10)
		{
			Light.TextureScale = 10f;
		}
		if (Light.TextureScale <= 0)
		{
			Light.TextureScale = 0f;
			Light.Energy = 0f;
		}
		GD.Print(InLightZones);
	}
	private void UpdateInv()
	{
		Grass.Text = "Grass: " + inventory.Grass;
        Logs.Text = "Logs: " + inventory.Wood;
        Rocks.Text = "Rocks: " + inventory.Rocks;
    }

	private void EnteredLightZone(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			InLightZones++;
		}
	}
	private void ExitedLightZone(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			InLightZones--;
		}
	}
    private void LightFaded()
    {
        InLightZones--;
    }

    private void CollectRocks(int RocksGained)
	{
		inventory.Rocks += RocksGained;
		UpdateInv();
    }
	public void CollectLogs(int WoodGained)
	{
		inventory.Wood += WoodGained;
		UpdateInv();
    }
	private void CollectGrass(int GrassGained)
	{
		inventory.Grass += GrassGained;
		UpdateInv();
    }

	private void PlayerBuiltTorch()
	{
		if (inventory.Grass >= 2 && inventory.Wood >= 1)
		{
			inventory.Grass -= 2;
            inventory.Wood -= 1;
            UpdateInv();
            Node2D SpawnedTorch;
            SpawnedTorch = (Node2D)Torch.Instantiate();
            AddSibling(SpawnedTorch);
            SpawnedTorch.Position = this.Position;
        }
	}

	private void TakeGrass()
	{

	}
    private void TakeLog()
    {

    }
}
