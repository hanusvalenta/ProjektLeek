using Godot;
using System;

public class DeathCollider : Area
{
	[Export]
	public string DeathScenePath = "res://Scenes/Death.tscn";

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Tom)
		{
			LoadDeathScene();
		}
	}

	private void LoadDeathScene()
	{
		if (!string.IsNullOrEmpty(DeathScenePath))
		{
			GetTree().ChangeScene(DeathScenePath);
		}
		else
		{
			GD.PrintErr("DeathScenePath is not set!");
		}
	}
}
