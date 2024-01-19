using Godot;
using System;

public partial class LightSignals : Node
{
    [Signal]
    public delegate void DecreaseInLightAmountEventHandler();
    [Signal]
    public delegate void TakeGrassEventHandler();
    [Signal]
    public delegate void TakeLogEventHandler();
}
