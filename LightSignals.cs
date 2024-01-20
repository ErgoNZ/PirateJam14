using Godot;
using System;

public partial class LightSignals : Node
{
    [Signal]
    public delegate void TakeGrassEventHandler(int grassCost);
    [Signal]
    public delegate void TakeLogEventHandler(int woodCost);
    [Signal]
    public delegate void AddLightAreaEventHandler();
    [Signal]
    public delegate void RemoveLightAreaEventHandler();
}
