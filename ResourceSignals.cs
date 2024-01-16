using Godot;
using System;

public partial class ResourceSignals : Node
{
	[Signal]
	public delegate void GrassCollectionEventHandler(int GrassGained);
	[Signal]
	public delegate void WoodCollectionEventHandler(int WoodGained);
	[Signal]
	public delegate void RockCollectionEventHandler(int RocksGained);
}
