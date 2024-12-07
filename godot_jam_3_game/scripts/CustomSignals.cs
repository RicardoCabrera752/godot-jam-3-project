using Godot;
using System;

public partial class CustomSignals : Node
{
	// Signals
	// Emitted when the Player starts a new game, sent to kill MainMenuWorld
	[Signal] 
	public delegate void KillMainMenuWorldEventHandler();
	// Emitted when the GameMapWorld needs to be killed
	[Signal] 
	public delegate void KillGameMapWorldEventHandler();
	// Emitted when the GameMapWorld is loaded for a new game
	[Signal] 
	public delegate void FirstTimeLoadGameMapWorldEventHandler();
	// Emitted when the Player chooses to abandon the current game
	//[Signal] public delegate void AbandonGameEventHandler();

	// Emitted to load the MainMenuWorldMap
	[Signal] 
	public delegate void LoadMainMenuWorldEventHandler();

	// Emitted to tell the UIManager to refresh the Shop Cards
	[Signal] 
	public delegate void RefreshShopCardsEventHandler();

	// Emitted to tell the UIManager to reset the player's inventory items
	[Signal] 
	public delegate void ResetPlayerInventoryItemsEventHandler();

	// Emitted when an inventory item is pressed
	[Signal] public delegate void InspectInventoryItemEventHandler(int index, string content);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
