using Godot;
using System;

public partial class WorldManager : Node3D
{
	// Signals

	// Exports
	// Main Menu World Instance
	[Export] 
	public PackedScene MainMenuWorldScene { get; set; }
	// Game Map World Instance
	[Export] 
	public PackedScene GameMapWorldScene { get; set; }

	// Properties
	// Access to the GameData variables
	private GameData _gameData;
	// Access to the CustomSignals signals
	private CustomSignals _customSignals;

	private MainMenuWorld MainMenuWorldInstance;


	// Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//MainMenuWorld mainMenuWorld = MainMenuWorldScene.Instantiate<MainMenuWorld>();
		MainMenuWorldInstance = MainMenuWorldScene.Instantiate<MainMenuWorld>();
		AddChild(MainMenuWorldInstance);

		_customSignals = GetTree().Root.GetNode<CustomSignals>("CustomSignals");
		_gameData = GetTree().Root.GetNode<GameData>("GameData");

		// Load MainMenuWorld again
		_customSignals.LoadMainMenuWorld += HandleLoadMainMenuWorld;

		// Kill MainMenuWorld
		_customSignals.KillMainMenuWorld += HandleKillMainMenuWorld;

		// Load GameMapWorld

		// Kill GameMapWorld
		_customSignals.KillGameMapWorld += HandleKillGameMapWorld;

		// First time loading GameMapWorld
		_customSignals.FirstTimeLoadGameMapWorld += HandleFirstTimeLoadGameMapWorld;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Handle killing MainMenuWorld
	private void HandleKillMainMenuWorld()
	{
		// Safety Check
		if(_gameData.IsMainMenuWorldDead)
		{
			GD.Print("MainMenuWorld is already dead!");
			return;
		}else
		{
			//GetTree().Root.GetNode<GameData>("GameData").IsMainMenuWorldDead = true;
			_gameData.IsMainMenuWorldDead = true;
			//_gameData.IsMainMenuWorldAlive = false;
			GD.Print("Killing MainMenuWorld");
			GetTree().Root.GetNode<Node3D>("Main/WorldManager/MainMenuWorld").QueueFree();
			//QueueFree(); // Kill the MainMenuWorld
		}

	}

	// Handle loading MainMenuWorld
	private void HandleLoadMainMenuWorld()
	{

		// Safety Check
		if(!_gameData.IsMainMenuWorldDead)
		{
			GD.Print("MainMenuWorld is already alive!");
			return;
		}else
		{
			//_gameData.IsMainMenuWorldAlive = true;
			_gameData.IsMainMenuWorldDead = false;
			GD.Print("Loading MainMenuWorld");
			MainMenuWorldInstance = MainMenuWorldScene.Instantiate<MainMenuWorld>();
			AddChild(MainMenuWorldInstance);
		}
		//GD.Print("Loading MainMenuWorld");

		//_gameData.IsMainMenuWorldAlive = true;

		//MainMenuWorld mainMenuWorld = MainMenuWorldScene.Instantiate<MainMenuWorld>();
		//AddChild(mainMenuWorld);
	}


	// Handle Killing GameMapWorld
	private void HandleKillGameMapWorld()
	{
		// Safety Check
		if(_gameData.IsGameMapWorldDead)
		{
			GD.Print("MainMenuWorld is already dead!");
			return;
		}else
		{
			//GetTree().Root.GetNode<GameData>("GameData").IsMainMenuWorldDead = true;
			_gameData.IsMainMenuWorldDead = true;
			//_gameData.IsMainMenuWorldAlive = false;
			GD.Print("Killing GameMapWorld");
			GetTree().Root.GetNode<Node3D>("Main/WorldManager/GameMapWorld").QueueFree();
			//QueueFree(); // Kill the MainMenuWorld
		}
	}

	// Handle Loading GameMapWorld After Battle

	// Handle loading GameMapWorld for the first time
	private void HandleFirstTimeLoadGameMapWorld()
	{
		GD.Print("First time loading GameMapWorld for this run");
		GameMapWorld gameMapWorld = GameMapWorldScene.Instantiate<GameMapWorld>();
		AddChild(gameMapWorld);
		_gameData.IsGameMapWorldDead = false;

		GetTree().Root.GetNode<GameData>("GameData").IsGameInProgress = true;
		GetTree().Root.GetNode<GameData>("GameData").IsGamePausable = true;
		GetTree().Root.GetNode<GameData>("GameData").IsGamePaused = false;

	}


}
