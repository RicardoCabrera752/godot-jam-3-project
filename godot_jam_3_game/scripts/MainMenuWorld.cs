using Godot;
using System;

public partial class MainMenuWorld : Node3D
{
	// Signals

	// Exports

	// Properties
	// Access to the GameData variables
	private GameData _gameData;
	// Access to the CustomSignals signals
	private CustomSignals _customSignals;

	// Methods
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetTree().Root.GetNode<CustomSignals>("CustomSignals");

		_gameData = GetTree().Root.GetNode<GameData>("GameData");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
