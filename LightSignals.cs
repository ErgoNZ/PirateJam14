using Godot;
using System;

public partial class LightSignals : Node
{
	[Signal]
	public delegate void AddLightAreaEventHandler();
	[Signal]
	public delegate void RemoveLightAreaEventHandler();
	[Signal]
	public delegate void TakeOneGrassEventHandler();
	[Signal]
	public delegate void TakeOneLogEventHandler();
    [Signal]
    public delegate void TorchDiedEventHandler(int ID);
}
