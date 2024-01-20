using Godot;
using System;

public partial class ResourceSignals : Node
{
	[Signal]
	public delegate void GrassEventHandler(int GrassGained);
	[Signal]
	public delegate void WoodEventHandler(int WoodGained);
	[Signal]
	public delegate void RockEventHandler(int RocksGained);
}
