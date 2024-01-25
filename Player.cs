using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public static Player playerScript;
	public const float Speed = 300.0f;
	public float LanternScale;
	public float Fuel = 1;
	public const float LanternEnergy = 3f;
	public double Hp = 100;
	PointLight2D Light = new PointLight2D();
	public int InLightZones = 0;
	ResourceSignals ResSignals;
	LightSignals LightSignals;
	Label Grass, Logs, Rocks;
	PackedScene Torch;
	PackedScene Lamp;
	PackedScene GameOver;
	Inventory inventory;
	ProgressBar SanityBar;
	Area2D LightZone;
	AnimationTree Animator;
	AnimationNodeStateMachinePlayback StateMachine;
	
	public override void _Ready()
	{
		playerScript = this;
		// Called every time the node is added to the scene.
		// Initialization here.
		Light = (PointLight2D)GetNode("Lantern");
		LightZone = GetNode<Area2D>("Lantern/PlayerLightZone");
		InLightZones = 0;
		ResSignals = GetNode<ResourceSignals>("/root/ResourceSignals");
		LightSignals = GetNode<LightSignals>("/root/LightSignals");
		inventory = GetNode<Inventory>("/root/Inventory");
		Grass = GetNode<Label>("../CanvasLayer/BoxContainer/GrassAmount");
		Logs = GetNode<Label>("../CanvasLayer/BoxContainer/LogAmount");
		Rocks = GetNode<Label>("../CanvasLayer/BoxContainer/RockAmount");
		SanityBar = GetNode<ProgressBar>("../CanvasLayer/BoxContainer/SanityBar");
		Animator = GetNode<AnimationTree>("AnimationTree");
		Torch = GD.Load<PackedScene>("res://Objects/torch.tscn");
		Lamp = GD.Load<PackedScene>("res://Objects/Lamppost.tscn");
		GameOver = GD.Load<PackedScene>("res://GameOver.tscn");
		ResSignals.Wood += HandleLogs;
		ResSignals.Grass += HandleGrass;
		ResSignals.Rock += HandleRocks;
		LightSignals.AddLightArea += AddLightArea;
		LightSignals.RemoveLightArea += RemoveLightArea;
		LanternScale = Light.TextureScale;
		StateMachine = (AnimationNodeStateMachinePlayback)Animator.Get("parameters/playback");
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
			Animator.Set("parameters/Idle/blend_position", velocity);
			Animator.Set("parameters/Walk/blend_position", velocity);
			Animator.Set("parameters/LowSanIdle/blend_position", velocity);
			Animator.Set("parameters/LowSanWalk/blend_position", velocity);
			
			if (Hp < 31){ StateMachine.Travel("LowSanWalk");}
			else { StateMachine.Travel("Walk");	}
			
		}
		else
		{
			if (Hp < 31){ StateMachine.Travel("LowSanIdle");}
			else { StateMachine.Travel("Idle");	}
			
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	//every interval on the timer for the light sources
	private void LightTick()
	{
		Light.TextureScale = Math.Abs(LanternScale * Fuel);
		LightZone.Scale = new(Math.Abs(1f * Fuel), Math.Abs(1f * Fuel));


		if(Hp <= 0)
		{
	   GetTree().ChangeSceneToPacked(GameOver);
	}
	if (InLightZones > 0)
		{
			Fuel += 0.015f;
		}
		if(InLightZones < 0) InLightZones = 0;
		if(Fuel <= 0 && InLightZones == 0) 
		{
			Hp -= 0.5;
		}
		GD.Print("In " + InLightZones + " Light Zones");
		SanityBar.Value = Hp;
		if(InLightZones == 0)
		{
			Fuel -= 0.03f;
		}
		if(Fuel < 0) Fuel = 0;
	}
	//updates labels for the inventory
	private void UpdateInv()
	{
		Grass.Text = "Grass: " + Inventory.Grass;
		Logs.Text = "Logs: " + Inventory.Wood;
		Rocks.Text = "Rocks: " + Inventory.Rocks;
	}
	//adds and removes light zones that the player is in
	private void AddLightArea()
	{
		InLightZones++;
	}
	private void RemoveLightArea()
	{
		InLightZones--;
	}
	private void HandleRocks(int RockDelta)
	{
		Inventory.Rocks += RockDelta;
		UpdateInv();
	}
	public void HandleLogs(int WoodDelta)
	{
		Inventory.Wood += WoodDelta;
		UpdateInv();
	}
	private void HandleGrass(int GrassDelta)
	{
		Inventory.Grass += GrassDelta;
		UpdateInv();
	}
	//method for building a torch 
	private void PlayerBuiltTorch()
	{
		if (Inventory.Grass >= 2 && Inventory.Wood >= 1)
		{
			Inventory.Grass -= 2;
			Inventory.Wood -= 1;
			UpdateInv();
			Node2D SpawnedTorch;
			SpawnedTorch = (Node2D)Torch.Instantiate();
			AddSibling(SpawnedTorch);
			SpawnedTorch.Position = this.Position;
		}
	}
	//method for building a lamp
	private void PlayerBuiltLamp()
	{
		if (Inventory.Wood >= 2 && Inventory.Rocks >= 2)
		{
			Inventory.Rocks -= 2;
			Inventory.Wood -= 2;
			UpdateInv();
			Node2D SpawnedLamp;
			SpawnedLamp = (Node2D)Lamp.Instantiate();
			AddSibling(SpawnedLamp);
			SpawnedLamp.Position = this.Position;
		}
	}
}
