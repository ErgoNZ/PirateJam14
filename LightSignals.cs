using Godot;
using System;

public partial class LightSignals : Node
{
    [Signal]
    public delegate void DecreaseInLightAmountEventHandler();
}
