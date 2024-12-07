using Godot;
using System;

public partial class Main : Node
{
	// Signals

	// Exports

	// Properties
	// Audio Bus Indices
	public int MasterVolumeIndex = AudioServer.GetBusIndex("Master");
	public int MusicVolumeIndex = AudioServer.GetBusIndex("Music");
	public int SFXVolumeIndex = AudioServer.GetBusIndex("SFX");

	// Random Generation helper variables
	public string RandomText = "";
	public int CollectionSize = 0;

	// Initial Volume Slider Values
	//public float MasterVolume = 1;
	//public float MusicVolume = 1;
	//public float SFXVolume = 1;

	// Access to the GameData variables
	private GameData _gameData;

	// Access to the CustomSignals signals
	private CustomSignals _customSignals;

	// Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameData = GetTree().Root.GetNode<GameData>("GameData");
		//var gameData = GetTree().Root.GetNode<GameData>("GameData");
		//var MasterVolume = gameData.MasterVolume;
		//var MusicVolume = gameData.MusicVolume;
		//var SFXVolume = gameData.SFXVolume;
		_customSignals = GetTree().Root.GetNode<CustomSignals>("CustomSignals");

		var MasterVolume = _gameData.MasterVolume;
		var MusicVolume = _gameData.MusicVolume;
		var SFXVolume = _gameData.SFXVolume;

		// Initialize the Volume Slider Values
		GetNode<HSlider>("UIManager/OptionsUI/MasterVolumeSlider").Value = MasterVolume;
		GetNode<HSlider>("UIManager/OptionsUI/MusicVolumeSlider").Value = MusicVolume;
		GetNode<HSlider>("UIManager/OptionsUI/SoundEffectsVolumeSlider").Value = SFXVolume;

		// Initialize Shopkeeper Text
		_gameData.InitializeShopkeeperText();

		// Initialize Master Item Dictionary
		_gameData.InitializeMasterItemDictionary();

		// Initialize Shop Lists
		_gameData.InitializeShopItemNameList();
		_gameData.InitializeShopMinionNameList();
		_gameData.InitializeShopSpecialSaleNameList();

		// Play the Main Menu Music
		GetNode<AudioStreamPlayer>("AudioManager/MainMenuMusic").Play();

		GD.Print("1<1", 1 < 1);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Handle Game Exit
	private void OnExitGame()
	{
		GetTree().Quit();
	}

	// Handle Master Volume Slider Changes
	private void OnChangeMasterVolume(float value)
	{
		AudioServer.SetBusVolumeDb(MasterVolumeIndex, Mathf.LinearToDb(value));
		GetTree().Root.GetNode<GameData>("GameData").MasterVolume = value;
	}

	// Handle Music Volume Slider Changes
	private void OnChangeMusicVolume(float value)
	{
		AudioServer.SetBusVolumeDb(MusicVolumeIndex, Mathf.LinearToDb(value));
		GetTree().Root.GetNode<GameData>("GameData").MusicVolume = value;
	}

	// Handle SFX Volume Slider Changes
	private void OnChangeSoundEffectsVolume(float value)
	{
		AudioServer.SetBusVolumeDb(SFXVolumeIndex, Mathf.LinearToDb(value));
		GetTree().Root.GetNode<GameData>("GameData").SFXVolume = value;
	}

	// Handle Game Start
	private void OnStartGame(string clanName)
	{
		//GetNode<AudioStreamPlayer>("AudioManager/MainMenuMusic").Stop();
		GD.Print("Starting Game with " + clanName);

		// Emit level loading
		_customSignals.EmitSignal(nameof(CustomSignals.KillMainMenuWorld));
		_customSignals.EmitSignal(nameof(CustomSignals.FirstTimeLoadGameMapWorld));

		// Generate random Shop Items, Minions, and Special Sale
		GenerateRandomShopItems(6);
		//GenerateRandomShopMinions(3);
		GenerateRandomSpecialSale();

		// Emit Shop Refresh
		_customSignals.EmitSignal(nameof(CustomSignals.RefreshShopCards));

		// Emit Inventory Reset
		_customSignals.EmitSignal(nameof(CustomSignals.ResetPlayerInventoryItems));
		// Initialize Player Inventory
		//_gameData.InitializePlayerItemDictionary();


	}

	// Handle Abandon Current Game
	private void OnAbandonGame()
	{
		// Kill GameMapWorld
		_customSignals.EmitSignal(nameof(CustomSignals.KillGameMapWorld));
		GD.Print("Game Abandoned, Returning to Main Menu");

		// Load MainMenuWorld again
		_customSignals.EmitSignal(nameof(CustomSignals.LoadMainMenuWorld));

		// Reset all Game Variables
		_gameData.ResetAllGameVariables();

		// Play the Main Menu Music
		GetNode<AudioStreamPlayer>("AudioManager/MainMenuMusic").Play();

		GetTree().Paused = false;
	}

	// Generate Random Shop Items
	private void GenerateRandomShopItems(int count)
	{
		// Clear list
		_gameData.CurrentShopItemNames.Clear();

		RandomText = "";
		CollectionSize = _gameData.ShopItemNameList.Count;

		// Generate random items
		for (int i = 0; i < count; i++)
		{
			RandomText = _gameData.ShopItemNameList[(int)(GD.Randi() % CollectionSize)];
			_gameData.CurrentShopItemNames.Add(RandomText);
		}
	}

	// Generate Random Shop Minions
	private void GenerateRandomShopMinions(int count)
	{
		// Clear list
		_gameData.CurrentShopMinionNames.Clear();

		RandomText = "";
		CollectionSize = _gameData.ShopMinionNameList.Count;

		// Generate random minions
		for (int i = 0; i < count; i++)
		{
			RandomText = _gameData.ShopMinionNameList[(int)(GD.Randi() % CollectionSize)];
			_gameData.CurrentShopMinionNames.Add(RandomText);
		}
	}

	// Generate Random Special Sale
	private void GenerateRandomSpecialSale()
	{
		// Clear string
		_gameData.CurrentShopSpecialSaleName = "";

		RandomText = "";
		CollectionSize = _gameData.ShopSpecialSaleNameList.Count;

		RandomText = _gameData.ShopSpecialSaleNameList[(int)(GD.Randi() % CollectionSize)];
		_gameData.CurrentShopSpecialSaleName = RandomText;
	}


	private void KillMenu()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.KillMainMenuWorld));
	}

	private void LoadMenu()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.LoadMainMenuWorld));
	}
}
