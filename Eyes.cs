using Godot;
using System;

public partial class Eyes : Node2D
{
	Timer Timer;
	private void PlayerLightKills(Rid area_rid, Area2D area, long area_shape_index, long local_shape_index)
	{
		if (area.IsInGroup("Player"))
		{
			Timer = GetNode<Timer>("Timer");
			Timer.Start();
		}
	}
	private void _on_timer_timeout()
	{
		Timer.Stop();
		QueueFree();
	}
	private void _on_tree_exiting()
	{
		GD.Print("Eye is despawning");
	}
}
