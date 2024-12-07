using Godot;
using System;
using System.Collections.Generic;


public partial class GameData : Node
{
	public static GameData Instance { get; private set; }

	// Initial Volume Slider Values
	public float MasterVolume { get; set; } = 0.75f;
	public float MusicVolume { get; set; } = 0.0f;
	public float SFXVolume { get; set; } = 0.65f;

	// Game Variables

	// General Metadata
	// Is a run in progress
	public bool IsGameInProgress { get; set; } = false;
	// Can the player pause the game
	public bool IsGamePausable { get; set; } = false;
	// Is the game paused
	public bool IsGamePaused { get; set; } = false;
	// Has the player won the game
	public bool IsGameWon { get; set; } = false;
	// Has the player lost the game
	public bool IsGameLost { get; set; } = false;
	// Is the shop open
	public bool IsShopOpen { get; set; } = false;
	// Is the inventory open
	public bool IsInventoryOpen { get; set; } = false;


	// Shop Metadata
	// Shopkeeper Greeting Text
	public List<string> ShopkeeperGreetingText = new List<string>();
	// Shopkeeper Lore Text
	public List<string> ShopkeeperLoreText = new List<string>();
	// Shopkeeper Advice Text
	public List<string> ShopkeeperAdviceText = new List<string>();
	// Shop Item Name List
	public List<string> ShopItemNameList = new List<string>();
	// Shop Minion Name List
	public List<string> ShopMinionNameList = new List<string>();
	// Shop Special Sale Name List
	public List<string> ShopSpecialSaleNameList = new List<string>();
	// Current Shop Items
	public List<string> CurrentShopItemNames = new List<string>();
	// Current Shop Minions
	public List<string> CurrentShopMinionNames = new List<string>();
	// Current Shop Special Sale
	public string CurrentShopSpecialSaleName = "";
	

	// Battle Metadata
	// Has the player won the battle
	public bool IsBattleWon { get; set; } = false;
	// Has the player lost the battle
	public bool IsBattleLost { get; set; } = false;

	// Worlds Metadata
	// Has the MainMenuWorld been killed
	public bool IsMainMenuWorldDead { get; set; } = false;
	// Has the GameMapWorld been killed
	public bool IsGameMapWorldDead { get; set; } = false;

	// Player Metadata
	// Amount of Runes the Player has currently
	public int CurrentPlayerRunes { get; set; } = 0;
	// Amount of Mana Cores the Player has currently
	public int CurrentManaCores { get; set; } = 0;
	// Amount of Morph Slime the Player has currently
	public int CurrentMorphSlime { get; set; } = 0;
	// Max number of items the player can hold
	public int ItemCapacity { get; set; } = 9;
	// Current number of items the player is holding
	public int CurrentItemCount { get; set; } = 0;
	// Max number of units the player can have
	public int PartySizeCapacity { get; set; } = 9;
	// Current number of units the player has
	public int CurrentPartySize { get; set; } = 0;


	// Dictionary of all Units the player has
	public Dictionary<string, Unit> PlayerUnitDictionary = new Dictionary<string, Unit>();
	// Dictionary of all Items the player has
	public Dictionary<string, string> PlayerItemDictionary = new Dictionary<string, string>();
	// List of all Items the player has
	public List<Item> PlayerItemList = new List<Item>();
	

	// Amount of enemy units defeated
	// Amount of bosses defeated
	// Amount of runes collected
	// Amount of player units lost

	// Master Metadata
	// Dictionary of all items
	public Dictionary<string, Item> MasterItemDictionary = new Dictionary<string, Item>();
	// Dictionary of all player units
	public Dictionary<string, Unit> MasterUnitDictionary = new Dictionary<string, Unit>();
	// Dictionart of all enemy units
	public Dictionary<string, Unit> MasterEnemyUnitDictionary = new Dictionary<string, Unit>();
	
	// Methods
	// Method to reset all game variables to default values
	public void ResetAllGameVariables()
	{
		// General Metadata
		IsGameInProgress = false;
		IsGamePausable = false;
		IsGamePaused = false;
		IsGameWon = false;
		IsGameLost = false;
		IsShopOpen = false;
		IsInventoryOpen = false;

		// Shop Metadata
		CurrentShopItemNames.Clear();
		CurrentShopMinionNames.Clear();
		CurrentShopSpecialSaleName = "";

		// Battle Metadata
		IsBattleWon = false;
		IsBattleLost = false;

		// Worlds Metadata
		IsMainMenuWorldDead = false;
		IsGameMapWorldDead = false;

		// Player Metadata
		CurrentPlayerRunes = 0;
		CurrentManaCores = 0;
		CurrentMorphSlime = 0;
		CurrentItemCount = 0;
		CurrentPartySize = 0;
		//PlayerItemDictionary.Clear();
		PlayerItemList.Clear();

	}

	// Method to initialize the Player Item dictionary
	public void InitializePlayerItemDictionary()
	{
		for (int i = 0; i < ItemCapacity; i++)
		{
			PlayerItemDictionary.Add("Slot " + i.ToString(), "EmptyName");
		}
	}

	// Method to initialize the Player Item list
	public void InitializePlayerItemList()
	{

	}

	// Method to initialize the Master Item dictionary
	public void InitializeMasterItemDictionary()
	{
		MasterItemDictionary.Add("Health Vial", new Item(0, "Health Vial", 1, (Texture2D)GD.Load("res://images/items/HealthVial.png"), 0, 99, true, "restore 15% health"));
		MasterItemDictionary.Add("Mana Vial", new Item(1, "Mana Vial", 1, (Texture2D)GD.Load("res://images/items/ManaVial.png"), 0, 99, true, "restore 15% mana"));
		MasterItemDictionary.Add("Health Potion", new Item(2, "Health Potion", 2, (Texture2D)GD.Load("res://images/items/HealthPotion.png"), 0, 99, true, "restore 30% health"));
		MasterItemDictionary.Add("Mana Potion", new Item(3, "Mana Potion", 2, (Texture2D)GD.Load("res://images/items/ManaPotion.png"), 0, 99, true, "restore 30% mana"));
		MasterItemDictionary.Add("Health Tonic", new Item(4, "Health Tonic", 3, (Texture2D)GD.Load("res://images/items/HealthTonic.png"), 0, 99, true, "restore 45% health"));
		MasterItemDictionary.Add("Mana Tonic", new Item(5, "Mana Tonic", 3, (Texture2D)GD.Load("res://images/items/ManaTonic.png"), 0, 99, true, "restore 45% mana"));
		MasterItemDictionary.Add("Purification Scroll", new Item(6, "Purification Scroll", 3, (Texture2D)GD.Load("res://images/items/PurificationScroll.png"), 0, 99, true, "remove all debuffs"));
		MasterItemDictionary.Add("Divine Elixir", new Item(7, "Divine Elixir", 7, (Texture2D)GD.Load("res://images/items/DivineElixir.png"), 0, 99, true, "restore health to max"));
		MasterItemDictionary.Add("Chrono Watch", new Item(8, "Chrono Watch", 8, (Texture2D)GD.Load("res://images/items/ChronoWatch.png"), 0, 99, true, "restore Action Points to max"));
		
	}

	// Method to initialize the Shop Item Name list
	public void InitializeShopItemNameList()
	{
		ShopItemNameList.Add("Health Vial");
		ShopItemNameList.Add("Mana Vial");
		ShopItemNameList.Add("Health Potion");
		ShopItemNameList.Add("Mana Potion");
		ShopItemNameList.Add("Health Tonic");
		ShopItemNameList.Add("Mana Tonic");
	}

	// Method to initialize the Shop Minion Name list
	public void InitializeShopMinionNameList()
	{
		// Nature
		ShopMinionNameList.Add("Clay Lumber Hulk");
		ShopMinionNameList.Add("Dendroid Prowler");
		ShopMinionNameList.Add("Jade Dragon");

		// Holy

		// Undead

		// Demon
		ShopMinionNameList.Add("Legionnaire");
		ShopMinionNameList.Add("Imp");
		ShopMinionNameList.Add("Siege Engine MKI");

		// Eldritch
	}

	// Method to initialize the Shop Special Sale Name list
	public void InitializeShopSpecialSaleNameList()
	{
		ShopSpecialSaleNameList.Add("Divine Elixir");
		ShopSpecialSaleNameList.Add("Chrono Watch");
		ShopSpecialSaleNameList.Add("Purification Scroll");
	}

	// Method to initialize the Shopkeeper text lists
	public void InitializeShopkeeperText()
	{
		// Greeting text options
		ShopkeeperGreetingText.Add
		(
			"Welcome to my shop stranger. I sell only the finest wares."
		);

		ShopkeeperGreetingText.Add
		(
			"Greetings stranger. Looking to spend your runes?"
		);

		ShopkeeperGreetingText.Add
		(
			"Hello stranger. Please feel free to browse my merchandise."
		);

		// Advice text options
		ShopkeeperAdviceText.Add
		(
			"Fighter class minions are general purpose units. They are good at taking and dealing damage. You should prioritize placing them in the first row to form a strong frontline."
		);

		ShopkeeperAdviceText.Add
		(
			"Rogue class minions are agile raid units. They trade defense for improved initiative, critical hit chance, and evasion. Use them to take out high priority targets or to perform devastating first strikes."
		);

		ShopkeeperAdviceText.Add
		(
			"Caster class minions are powerful support units. They have access to a wide array of offensive and defensive abilities. Place them in the last row to keep them safe from enemy melee attacks."
		);

		ShopkeeperAdviceText.Add
		(
			"Fusion class minions are formed by combining two Tier III minions. Their abilities depend on the Soul type of the minions that were combined. They can use Tier V spells and skills to obliterate enemies!"
		);

		ShopkeeperAdviceText.Add
		(
			"Your minions have the ability to shapeshift! A minion can change to another minion of the same Soul type and Tier. For example, a Tier II Nature minion can change to any Tier II Nature minion regardless of Class type."
		);

		ShopkeeperAdviceText.Add
		(
			"After you win a battle, all of the dead minions in your party will be revived. This only applies if Minion Permadeath is disabled."
		);

		// Lore text options
		ShopkeeperLoreText.Add
		(
			"Nature lore"
		);

		ShopkeeperLoreText.Add
		(
			"Holy lore"
		);

		ShopkeeperLoreText.Add
		(
			"Undead lore"
		);

		ShopkeeperLoreText.Add
		(
			"Demon lore"
		);

		ShopkeeperLoreText.Add
		(
			"Eldritch lore"
		);
	}



    public override void _Ready()
    {
        Instance = this;
		
    }
}
