using Godot;
using System;
using System.Collections.Generic;

public partial class LightSourceList : Node
{
	public static List<LightInfo> Lights;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Lights = new List<LightInfo>();
	}
	public class LightInfo 
	{
		public Node2D Light;
		public bool Active;
	}

}
