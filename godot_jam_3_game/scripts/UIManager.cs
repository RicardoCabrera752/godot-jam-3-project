using Godot;
using System;

public partial class UIManager : Control
{
	// Signals
	// Emitted when the player wants to quit the game
	[Signal] 
	public delegate void ExitGameEventHandler();

	// Emitted when the player starts the game
	[Signal] 
	public delegate void StartGameEventHandler(string clanName);

	// Emitted when the player chooses to abandon the current game
	[Signal] 
	public delegate void AbandonGameEventHandler();

	// Emitted when the player changes the Master Volume
	[Signal] 
	public delegate void ChangeMasterVolumeEventHandler(float value);

	// Emitted when the player changes the Music Volume
	[Signal] 
	public delegate void ChangeMusicVolumeEventHandler(float value);

	// Emitted when the player changes the SFX Volume
	[Signal] 
	public delegate void ChangeSoundEffectsVolumeEventHandler(float value);

	// Properties
	public bool ShowMainMenuScreen = true;
	public bool ShowStartScreen = false;
	public bool ShowControlsScreen = false;
	public bool ShowOptionsScreen = false;
	public bool ShowCreditsScreen = false;

	// Access to the inventory items
	public GridContainer ItemGridContainer;

	// Current item index used for handling selling items
	public int CurrentItemButtonIndex;

	// Random Gneration helper variables
	public string RandomText = "";
	public int CollectionSize = 0;
	public string LastShopkeeperText = "";

	// Access to the GameData variables
	private GameData _gameData;

	// Access to the CustomSignals signals
	private CustomSignals _customSignals;

	// Packed Scenes
	public PackedScene ItemButtonInstance;
	[Export] public string ItemButtonPath = "res://item_button.tscn";

	// Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameData = GetTree().Root.GetNode<GameData>("GameData");
		_customSignals = GetTree().Root.GetNode<CustomSignals>("CustomSignals");

		_customSignals.RefreshShopCards += HandleRefreshShopCards;

		_customSignals.ResetPlayerInventoryItems += HandleResetPlayerInventoryItems;

		_customSignals.InspectInventoryItem += HandleInspectInventoryItem;

		// Item Buttons
		ItemButtonInstance = ResourceLoader.Load<PackedScene>(ItemButtonPath);

		ItemGridContainer = GetNode<GridContainer>("InventoryUI/PlayerItemContainer/ScrollContainer/GridContainer");

		InitializeInventoryItemButtons();
		// Safety check to display correct UI
		ShowOnlyMainMenuUI();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Check for Player Inputs
		// Pause Game
		if (Input.IsActionJustPressed("pause_game"))
        {
            if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused)
			{
				GetTree().Paused = true;

				GetNode<CanvasLayer>("PauseMenuUI").Show();
				_gameData.IsGamePaused = true;
		
				GetNode<Button>("MenuBarUI/PauseGameButton").Disabled = true;

				GetNode<CanvasLayer>("ControlsUI").Hide();
				GetNode<CanvasLayer>("OptionsUI").Hide();
				GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
				//GetNode<Control>("PauseMenuUI/AbandonGameUI").Hide();
				
			}
			else 
			{
				GetTree().Paused = false;

				GetNode<CanvasLayer>("PauseMenuUI").Hide();
				_gameData.IsGamePaused = false;
		
				GetNode<Button>("MenuBarUI/PauseGameButton").Disabled = false;

				GetNode<CanvasLayer>("ControlsUI").Hide();
				GetNode<CanvasLayer>("OptionsUI").Hide();
				GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
				//GetNode<Control>("PauseMenuUI/AbandonGameUI").Hide();
			}

			
        }

		// Open Shop
		if (Input.IsActionJustPressed("open_shop"))
		{
			if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsShopOpen && !_gameData.IsInventoryOpen)
			{
				GetNode<CanvasLayer>("ShopUI").Show();
				_gameData.IsShopOpen = true;

				GetNode<CanvasLayer>("InventoryUI").Hide();

				// Generate Random Greeting Text
				var randomText = GetRandomShopkeeperText("Greeting");
				//GD.Print(randomText);

				// Replace Shopkeeper Text Box
				GetNode<Label>("ShopUI/ShopKeeperTextBoxContainer/ShopKeeperTextBox").Text = randomText;

			}
			else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsShopOpen && _gameData.IsInventoryOpen)
			{
				GetNode<CanvasLayer>("ShopUI").Show();
				_gameData.IsShopOpen = true;

				_gameData.IsInventoryOpen = false;

				GetNode<CanvasLayer>("InventoryUI").Hide();

				// Generate Random Greeting Text
				var randomText = GetRandomShopkeeperText("Greeting");
				//GD.Print(randomText);

				// Replace Shopkeeper Text Box
				GetNode<Label>("ShopUI/ShopKeeperTextBoxContainer/ShopKeeperTextBox").Text = randomText;
			}
			else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && _gameData.IsShopOpen)
			{
				GetNode<CanvasLayer>("ShopUI").Hide();
				_gameData.IsShopOpen = false;
			}
		}

		// Open Inventory
		if (Input.IsActionJustPressed("open_inventory"))
		{
			if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsInventoryOpen && !_gameData.IsShopOpen)
			{
				GetNode<CanvasLayer>("InventoryUI").Show();
				ResetInventoryItemDetails();
				_gameData.IsInventoryOpen = true;

				GetNode<CanvasLayer>("ShopUI").Hide();

			}
			else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsInventoryOpen && _gameData.IsShopOpen)
			{
				GetNode<CanvasLayer>("InventoryUI").Show();
				ResetInventoryItemDetails();
				_gameData.IsInventoryOpen = true;

				_gameData.IsShopOpen = false;

				GetNode<CanvasLayer>("ShopUI").Hide();
			}
			else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && _gameData.IsInventoryOpen)
			{
				GetNode<CanvasLayer>("InventoryUI").Hide();
				_gameData.IsInventoryOpen = false;
			}
		}

	}

	// Method to hide all UIs except the MainMenuUI
	private void ShowOnlyMainMenuUI()
	{
		// Main Menu
		GetNode<CanvasLayer>("MainMenuUI").Show();
		GetNode<CanvasLayer>("StartUI").Hide();
		GetNode<CanvasLayer>("ControlsUI").Hide();
		GetNode<CanvasLayer>("OptionsUI").Hide();
		GetNode<CanvasLayer>("CreditsUI").Hide();
		GetNode<CanvasLayer>("BackButtonUI").Hide();

		// In-Game Menus
		GetNode<CanvasLayer>("PauseMenuUI").Hide();
		
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
		GetNode<CanvasLayer>("InventoryUI").Hide();
		GetNode<CanvasLayer>("ShopUI").Hide();
		GetNode<CanvasLayer>("GameOverUI").Hide();
		GetNode<CanvasLayer>("GameWinUI").Hide();
		GetNode<CanvasLayer>("MenuBarUI").Hide();
	}

	// Method to hide all UIs except for MenuBarUI
	private void ShowOnlyMenuBarUI()
	{
		// Main Menu
		GetNode<CanvasLayer>("MainMenuUI").Hide();
		GetNode<CanvasLayer>("StartUI").Hide();
		GetNode<CanvasLayer>("ControlsUI").Hide();
		GetNode<CanvasLayer>("OptionsUI").Hide();
		GetNode<CanvasLayer>("CreditsUI").Hide();
		GetNode<CanvasLayer>("BackButtonUI").Hide();

		// In-Game Menus
		GetNode<CanvasLayer>("MenuBarUI").Show();
		GetNode<CanvasLayer>("PauseMenuUI").Hide();
		
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
		GetNode<CanvasLayer>("InventoryUI").Hide();
		GetNode<CanvasLayer>("ShopUI").Hide();
		GetNode<CanvasLayer>("GameOverUI").Hide();
		GetNode<CanvasLayer>("GameWinUI").Hide();

	}

	// Method to hide all UIs except for for MenuBarUI and ShopUI

	// Handle Start Button being pressed
	private void OnStartButtonPressed()
	{
		GD.Print("Start Button Pressed");
		GetNode<CanvasLayer>("BackButtonUI").Show();
		GetNode<CanvasLayer>("MainMenuUI").Hide();
		GetNode<CanvasLayer>("StartUI").Show();
		ShowStartScreen = true;
		GetNode<AudioStreamPlayer>("../AudioManager/ButtonSoundEffect").Play();
	}

	// Handle Controls Button being pressed
	private void OnControlsButtonPressed()
	{
		GD.Print("Controls Button Pressed");
		GetNode<CanvasLayer>("BackButtonUI").Show();
		GetNode<CanvasLayer>("MainMenuUI").Hide();
		GetNode<CanvasLayer>("ControlsUI").Show();

		GetNode<ColorRect>("ControlsUI/ControlsBackground").Hide();
		GetNode<Button>("ControlsUI/CloseControls").Hide();

		ShowControlsScreen = true;
		GetNode<AudioStreamPlayer>("../AudioManager/ButtonSoundEffect").Play();
	}

	// Handle Options Button being pressed
	private void OnOptionsButtonPressed()
	{
		GD.Print("Options Button Pressed");
		GetNode<CanvasLayer>("BackButtonUI").Show();
		GetNode<CanvasLayer>("MainMenuUI").Hide();
		GetNode<CanvasLayer>("OptionsUI").Show();

		GetNode<Button>("OptionsUI/CloseOptions").Hide();

		ShowOptionsScreen = true;
		GetNode<AudioStreamPlayer>("../AudioManager/ButtonSoundEffect").Play();
	}

	// Handle Credits Button being pressed
	private void OnCreditsButtonPressed()
	{
		GD.Print("Credits Button Pressed");
		GetNode<CanvasLayer>("BackButtonUI").Show();
		GetNode<CanvasLayer>("MainMenuUI").Hide();
		GetNode<CanvasLayer>("CreditsUI").Show();
		ShowCreditsScreen = true;
		GetNode<AudioStreamPlayer>("../AudioManager/ButtonSoundEffect").Play();
	}

	// Handle Exit Button being pressed
	private void OnExitButtonPressed()
	{
		EmitSignal(SignalName.ExitGame);
	}

	// Handle Back Button being pressed
	private void OnBackButtonPressed()
	{
		GD.Print("Back Button Pressed");
		GetNode<CanvasLayer>("BackButtonUI").Hide();

		if(ShowStartScreen)
		{
			GetNode<CanvasLayer>("StartUI").Hide();
			ShowStartScreen = false;
		}

		if(ShowControlsScreen)
		{
			GetNode<CanvasLayer>("ControlsUI").Hide();
			ShowControlsScreen = false;
		}

		if(ShowOptionsScreen)
		{
			GetNode<CanvasLayer>("OptionsUI").Hide();
			ShowOptionsScreen = false;
		}

		if(ShowCreditsScreen)
		{
			GetNode<CanvasLayer>("CreditsUI").Hide();
			ShowCreditsScreen = false;
		}


		GetNode<CanvasLayer>("MainMenuUI").Show();
		GetNode<AudioStreamPlayer>("../AudioManager/ButtonSoundEffect").Play();
	}

	// Handle Master Volume Slider Changes
	private void OnMasterVolumeSliderValueChanged(float value)
	{
		//GD.Print(value);
		EmitSignal(SignalName.ChangeMasterVolume, value);
	}

	// Handle Music Volume Slider Changes
	private void OnMusicVolumeSliderValueChanged(float value)
	{
		EmitSignal(SignalName.ChangeMusicVolume, value);
	}

	// Handle SFX Volume Slider Changes
	private void OnSoundEffectsVolumeSliderValueChanged(float value)
	{
		EmitSignal(SignalName.ChangeSoundEffectsVolume, value);
	}

	// Handle Select Clan button being pressed
	private void OnSelectClanButtonPressed(string clanName)
	{
		GetNode<CanvasLayer>("StartUI").Hide();
		ShowStartScreen = false;
		GetNode<CanvasLayer>("BackButtonUI").Hide();

		GD.Print("Selected Clan: " + clanName);
		EmitSignal(SignalName.StartGame, clanName);

		ShowOnlyMenuBarUI();
		ResetPlayerResourcesText();

		_gameData.IsGameInProgress = true;

		// Print Starting Units
		//_gameData.PlayerUnitDictionary.Add("A", new Unit());
		//GD.Print("Starting Units: ");
		//GD.Print(_gameData.PlayerUnitDictionary["A"].UnitName);
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////
	// In-Game UI

	// Reset Player Runes and Mana Cores
	private void ResetPlayerResourcesText()
	{
		// Reset Runes
		UpdatePlayerRunesText(10);

		// Reset Mana Cores
		UpdatePlayerManaCoresText(0);

		// Reset Morph Slime
		UpdatePlayerMorphSlimeText(0);
	}

	// Method to update Player Runes
	private void UpdatePlayerRunesText(int runes)
	{
		_gameData.CurrentPlayerRunes = runes;
		GetNode<Label>("MenuBarUI/PlayerRunesCount").Text = runes.ToString();


	}

	// Method to update Player Morph Slime
	private void UpdatePlayerMorphSlimeText(int morphSlime)
	{
		_gameData.CurrentMorphSlime = morphSlime;
		GetNode<Label>("MenuBarUI/PlayerMorphSlimeCount").Text = morphSlime.ToString();
	}

	// Method to update Player Mana Cores
	private void UpdatePlayerManaCoresText(int manaCores)
	{
		_gameData.CurrentManaCores = manaCores;
		GetNode<Label>("MenuBarUI/PlayerManaCoresCount").Text = manaCores.ToString() + "/5";

	}

	//*********************************************************
	// Pause Menu
	// Handle Pause Menu Button being pressed
	private void OnPauseGameButtonPressed()
	{
		GetTree().Paused = true;

		GetNode<CanvasLayer>("PauseMenuUI").Show();
		_gameData.IsGamePaused = true;
		
		GetNode<Button>("MenuBarUI/PauseGameButton").Disabled = true;

		
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
	}

	// Handle Resume Game Button being pressed
	private void OnResumeGameButtonPressed()
	{
		GetTree().Paused = false;

		GetNode<CanvasLayer>("PauseMenuUI").Hide();
		_gameData.IsGamePaused = false;
		
		GetNode<Button>("MenuBarUI/PauseGameButton").Disabled = false;

		
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
	}

	// Handle Pause Menu Controls Button being pressed
	private void OnPauseMenuControlsButtonPressed()
	{
		GetNode<CanvasLayer>("ControlsUI").Show();
		
		GetNode<ColorRect>("ControlsUI/ControlsBackground").Show();
		GetNode<Button>("ControlsUI/CloseControls").Show();

		GetNode<CanvasLayer>("PauseMenuUI").Hide();
	}

	// Handle Pause Menu Close Controls Button being pressed
	private void OnCloseControlsButtonPressed()
	{
		GetNode<CanvasLayer>("ControlsUI").Hide();
		GetNode<CanvasLayer>("PauseMenuUI").Show();
	}

	// Handle Pause Menu Options Button being pressed
	private void OnPauseMenuOptionsButtonPressed()
	{
		GetNode<CanvasLayer>("OptionsUI").Show();
		
		GetNode<Button>("OptionsUI/CloseOptions").Show();

		GetNode<CanvasLayer>("PauseMenuUI").Hide();
	}

	// Handle Pause Menu Close Options Button being pressed
	private void OnCloseOptionsPressed()
	{
		GetNode<CanvasLayer>("OptionsUI").Hide();
		GetNode<CanvasLayer>("PauseMenuUI").Show();
	}

	// Handle Abandon Game Button being pressed
	private void OnAbandonGameButtonPressed()
	{
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Show();
	}

	// Handle Confirm Abandon Game Button being pressed
	private void OnConfirmAbandonGameButtonPressed()
	{
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
		GetNode<Button>("MenuBarUI/PauseGameButton").Disabled = false;
		EmitSignal(SignalName.AbandonGame);

		ShowOnlyMainMenuUI();
	}

	// Handle Cancel Abandon Game Button being pressed
	private void OnCancelAbandonGameButtonPressed()
	{
		GetNode<CanvasLayer>("PauseMenuUI/AbandonGameUI").Hide();
	}

	//*********************************************************
	// Inventory Menu
	// Handle Inventory Menu Button being pressed
	private void OnOpenInventoryButtonPressed()
	{
		if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsInventoryOpen && !_gameData.IsShopOpen)
		{
			GetNode<CanvasLayer>("InventoryUI").Show();
			ResetInventoryItemDetails();
			_gameData.IsInventoryOpen = true;

			GetNode<CanvasLayer>("ShopUI").Hide();

		}
		else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsInventoryOpen && _gameData.IsShopOpen)
		{
			GetNode<CanvasLayer>("InventoryUI").Show();
			ResetInventoryItemDetails();
			_gameData.IsInventoryOpen = true;

			_gameData.IsShopOpen = false;

			GetNode<CanvasLayer>("ShopUI").Hide();
		}
		else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && _gameData.IsInventoryOpen)
		{
			GetNode<CanvasLayer>("InventoryUI").Hide();
			_gameData.IsInventoryOpen = false;
		}
	}

	// Method for initializing inventory item buttons
	private void InitializeInventoryItemButtons()
	{
		ItemButton currentItemButton;

		for (int i = 0; i < _gameData.ItemCapacity; i++)
		{
			currentItemButton = ItemButtonInstance.Instantiate<ItemButton>();
			//GetNode<GridContainer>("InventoryUI/PlayerItemContainer/ScrollContainer/GridContainer").AddChild(currentItemButton);
			ItemGridContainer.AddChild(currentItemButton);

		}
	}

	// Method for reseting player inventory items
	private void HandleResetPlayerInventoryItems()
	{
		// Reset all inventory buttons
		for(int i = 0; i < _gameData.ItemCapacity; i++)
		{
			ResetInventoryItemButton(i);
		}
	}

	// Method for inventory item reflow
	private void ReflowInventoryItems()
	{
		string content;
		Texture2D texture;
		string count;

		for (int i = 0; i < _gameData.CurrentItemCount; i++)
		{
			content = _gameData.PlayerItemList[i].ItemName;
			texture = _gameData.PlayerItemList[i].Icon;
			count = _gameData.PlayerItemList[i].Quantity.ToString();

			UpdateInventoryItemButton(i, content, texture, count);
		}
		
		
	}

	// Method for updating an inventory item button
	private void UpdateInventoryItemButton(int index, string content, Texture2D texture, string count)
	{
		ItemGridContainer.GetChild<ItemButton>(index).UpdateItemButton(index, content, texture, count, true);
	}

	// Method for updating an item button's count
	private void UpdateInventoryItemButtonLabel(int index, string count)
	{
		ItemGridContainer.GetChild<ItemButton>(index).UpdateItemButtonCount(count);
	}

	// Method for reseting an item button
	private void ResetInventoryItemButton(int index)
	{
		ItemGridContainer.GetChild<ItemButton>(index).ResetItemButton(index);
	}

	// Handle Inspect Inventory Item
	private void HandleInspectInventoryItem(int index, string content)
	{
		string message;

		// Check if the item exists
		if(content == "")
		{
			message = "There is no item to inspect!";
			GD.Print(message);
			ResetInventoryItemDetails();
			return;
		}

	
		// Get item details
		string itemName = _gameData.MasterItemDictionary[content].ItemName;
		Texture2D texture = _gameData.MasterItemDictionary[content].Icon;
		string description = _gameData.MasterItemDictionary[content].Description;

		message = "Inspecting Item!: " + itemName;
		GD.Print(message);
		CurrentItemButtonIndex = index;
		GD.Print("current item index: " + CurrentItemButtonIndex);

		GetNode<Label>("InventoryUI/ItemDetailsContainer/ItemName").Text = itemName;
		GetNode<Label>("InventoryUI/ItemDetailsContainer/ItemDescription").Text = description;
		GetNode<TextureRect>("InventoryUI/ItemDetailsContainer/ItemIcon").Texture = texture;
		GetNode<Button>("InventoryUI/ItemDetailsContainer/SellItemButton").Show();

	}

	// Method for reseting the Inventory item details container
	private void ResetInventoryItemDetails()
	{
		CurrentItemButtonIndex = -1;
		GetNode<Label>("InventoryUI/ItemDetailsContainer/ItemName").Text = "";
		GetNode<Label>("InventoryUI/ItemDetailsContainer/ItemDescription").Text = "";
		GetNode<TextureRect>("InventoryUI/ItemDetailsContainer/ItemIcon").Texture = null;
		GetNode<Button>("InventoryUI/ItemDetailsContainer/SellItemButton").Hide();
	}

	// Handle Sell Item Button being pressed
	private void OnSellItemButtonPressed()
	{
		string message;
		string itemName;

		itemName = _gameData.PlayerItemList[CurrentItemButtonIndex].ItemName;

		// Case 1 Single Item in Inventory
		if (_gameData.CurrentItemCount == 1)
		{
			// Check if item quantity is 1
			if(_gameData.PlayerItemList[CurrentItemButtonIndex].Quantity == 1)
			{
				// Remove the item
				_gameData.PlayerItemList.Remove(_gameData.PlayerItemList[CurrentItemButtonIndex]);

				// Decrement item count
				_gameData.CurrentItemCount--;

				// Set item button index to 0
				CurrentItemButtonIndex = 0;

				// Reset item button
				ResetInventoryItemButton(CurrentItemButtonIndex);

				// Reset item details
				ResetInventoryItemDetails();

				// Print details
				message = "Item Sold: " + itemName;
				GD.Print(message);
				GD.Print("Item List Count: ",_gameData.PlayerItemList.Count);
				GD.Print("Current Item Count: ", _gameData.CurrentItemCount);
				//GD.Print("Item Qnt: ", _gameData.PlayerItemList[CurrentItemButtonIndex].Quantity);
			}
			else
			{
				// Else decrement the item quantity
				DecreaseInventoryItemQuantity();

				// Print details
				message = "Item Sold: " + itemName;
				GD.Print(message);
				GD.Print("Item List Count: ",_gameData.PlayerItemList.Count);
				GD.Print("Current Item Count: ", _gameData.CurrentItemCount);
				GD.Print("Item Qnt: ", _gameData.PlayerItemList[CurrentItemButtonIndex].Quantity);

			}
		}
		// Case 2: Multiple items in inventory
		else
		{
			// Check if the item quantity is 1
			if(_gameData.PlayerItemList[CurrentItemButtonIndex].Quantity == 1)
			{
				// Remove the item
				_gameData.PlayerItemList.Remove(_gameData.PlayerItemList[CurrentItemButtonIndex]);

				// Decrement item count
				_gameData.CurrentItemCount--;

				// Reset item details
				ResetInventoryItemDetails();

				// Reflow item buttons
				ReflowInventoryItems();

				// Reset the button whose index is CurretnItemCount
				ResetInventoryItemButton(_gameData.CurrentItemCount);

				// Print the details
				message = "Item Sold: " + itemName;
				GD.Print(message);
				GD.Print("Item List Count: ",_gameData.PlayerItemList.Count);
				GD.Print("Current Item Count: ", _gameData.CurrentItemCount);
			}
			else
			{
				// Else decrement the item quantity
				DecreaseInventoryItemQuantity();

				// Print details
				message = "Item Sold: " + itemName;
				GD.Print(message);
				GD.Print("Item List Count: ",_gameData.PlayerItemList.Count);
				GD.Print("Current Item Count: ", _gameData.CurrentItemCount);
				GD.Print("Item Qnt: ", _gameData.PlayerItemList[CurrentItemButtonIndex].Quantity);
			}
		}
		
		
	}

	// Method to handle decrementing an item quantity
	private void DecreaseInventoryItemQuantity()
	{

		// Decrement the quantity
		_gameData.PlayerItemList[CurrentItemButtonIndex].Quantity--;

		// Update the item button
		UpdateInventoryItemButtonLabel(CurrentItemButtonIndex, _gameData.PlayerItemList[CurrentItemButtonIndex].Quantity.ToString());

	}

	//*********************************************************
	// Shop Menu
	// Handle Shop Menu Button being pressed
	private void OnOpenShopButtonPressed()
	{
		if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsShopOpen && !_gameData.IsInventoryOpen)
		{
			GetNode<CanvasLayer>("ShopUI").Show();
			_gameData.IsShopOpen = true;

			GetNode<CanvasLayer>("InventoryUI").Hide();

			// Generate Random Greeting Text
			var randomText = GetRandomShopkeeperText("Greeting");
			//GD.Print(randomText);

			// Replace Shopkeeper Text Box
			GetNode<Label>("ShopUI/ShopKeeperTextBoxContainer/ShopKeeperTextBox").Text = randomText;

		}
		else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && !_gameData.IsShopOpen && _gameData.IsInventoryOpen)
		{
			GetNode<CanvasLayer>("ShopUI").Show();
			_gameData.IsShopOpen = true;

			_gameData.IsInventoryOpen = false;

			GetNode<CanvasLayer>("InventoryUI").Hide();

			// Generate Random Greeting Text
			var randomText = GetRandomShopkeeperText("Greeting");
			//GD.Print(randomText);

			// Replace Shopkeeper Text Box
			GetNode<Label>("ShopUI/ShopKeeperTextBoxContainer/ShopKeeperTextBox").Text = randomText;
		}
		else if (_gameData.IsGameInProgress && !_gameData.IsGameLost && !_gameData.IsGameWon && !_gameData.IsGamePaused && _gameData.IsShopOpen)
		{
			GetNode<CanvasLayer>("ShopUI").Hide();
			_gameData.IsShopOpen = false;
		}
	}

	// Handle Shop Buy button being pressed
	private void OnBuyButtonPressed(int purchaseIndex, string purchaseRequest)
	{
		string purchaseName = "";
		int purchaseCost = 0;

		// Get purchase details
		if (purchaseRequest == "Minion")
		{
			purchaseName = _gameData.CurrentShopMinionNames[purchaseIndex];
			purchaseCost = 1;

			//_customSignals.EmitSignal(nameof(CustomSignals.PurchaseUnit), purchaseIndex, purchaseRequest, purchaseName, purchaseCost);
		}
		else if (purchaseRequest == "Item")
		{
			purchaseName = _gameData.CurrentShopItemNames[purchaseIndex];
			purchaseCost = _gameData.MasterItemDictionary[purchaseName].RuneCost;

			HandlePurchaseItem(purchaseName, purchaseCost);

			//_customSignals.EmitSignal(nameof(CustomSignals.PurchaseItem), purchaseIndex, purchaseRequest, purchaseName, purchaseCost);

		}else if (purchaseRequest == "Special Sale")
		{
			purchaseName = _gameData.CurrentShopSpecialSaleName;
			purchaseCost = _gameData.MasterItemDictionary[purchaseName].RuneCost;

			HandlePurchaseItem(purchaseName, purchaseCost);

			//_customSignals.EmitSignal(nameof(CustomSignals.PurchaseItem), purchaseIndex, purchaseRequest, purchaseName, purchaseCost);
		}

		// Emit purchase signal
		// Emit purchase signal
		//_customSignals.EmitSignal(nameof(CustomSignals.PurchaseShopCard), purchaseIndex, purchaseRequest);
	}

	// Method for handling an item purchase
	private void HandlePurchaseItem(string purchaseName, int purchaseCost)
	{
		int index;
		string message;
		bool itemFound = false;

		// Check if player has enough runes
		if(_gameData.CurrentPlayerRunes < purchaseCost)
		{
			
			message = "Error: Not Enough Runes!";
			GD.Print(message);
			//_customSignals.EmitSignal(nameof(CustomSignals.SendPurchaseReceipt), index, request);
			return;
		}

		// Check if player has room in inventory
		if (_gameData.CurrentItemCount > _gameData.ItemCapacity)
		{
			message = "Error: Item Inventory is Full!";
			GD.Print(message);
			//_customSignals.EmitSignal(nameof(CustomSignals.SendPurchaseReceipt), index, request);
			return;
		}

		// Check if the item already exists in player inventory
		int i = 0;
		while (!itemFound && i < _gameData.CurrentItemCount)
		{
			if (purchaseName == _gameData.PlayerItemList[i].ItemName)
			{
				// If found then increment the item count
				itemFound = true;
				message = "Item Found!: " + purchaseName;
				_gameData.PlayerItemList[i].Quantity++;
				UpdateInventoryItemButtonLabel(i, _gameData.PlayerItemList[i].Quantity.ToString());
				//pdateInventoryItemButton(i, purchaseName, )

				GD.Print(message);
				GD.Print("Item List Count: ",_gameData.PlayerItemList.Count);
				GD.Print("Current Item Count: ", _gameData.CurrentItemCount);
				GD.Print("Item Qnt: ", _gameData.PlayerItemList[i].Quantity);

				// Update inventory button
			}

			i++;
		}
		
		// If item was not found then add it to inventory
		if (!itemFound)
		{

			//ItemButton currentItemButton;
			message = "Adding Item!: " + purchaseName;

			_gameData.CurrentItemCount++;

			index = _gameData.CurrentItemCount - 1;
			Texture2D texture = _gameData.MasterItemDictionary[purchaseName].Icon;
			string count;

			// Add the item
			//Item itemToAdd = _gameData.MasterItemDictionary[purchaseName].DeepCopy();
			Item itemToAdd = _gameData.MasterItemDictionary[purchaseName].Duplicate() as Item;
			//Item itemToAdd = _gameData.MasterItemDictionary[purchaseName].Copy();
			//_gameData.PlayerItemList.Add(_gameData.MasterItemDictionary[purchaseName]);
			_gameData.PlayerItemList.Add(itemToAdd);

			_gameData.PlayerItemList[index].Quantity++;

			count = _gameData.PlayerItemList[index].Quantity.ToString();

			UpdateInventoryItemButton(index, purchaseName, texture, count);

			GD.Print(message);
			GD.Print("Item List Count: ",_gameData.PlayerItemList.Count);
			GD.Print("Current Item Count: ", _gameData.CurrentItemCount);
			GD.Print("Item Qnt: ", _gameData.PlayerItemList[i].Quantity);

			//ItemButtonInstance;

			//GetNode<GridContainer>("InventoryUI/PlayerItemContainer/ScrollContainer/GridContainer");
		}

	}

	// Method for handling a minion purchase


	// Handle Shop Lore button being pressed
	private void OnLoreButtonPressed()
	{
		// Generate Random Greeting Text
		var randomText = GetRandomShopkeeperText("Lore");
		//GD.Print(randomText);

		// Replace Shopkeeper Text Box
		GetNode<Label>("ShopUI/ShopKeeperTextBoxContainer/ShopKeeperTextBox").Text = randomText;
	}

	// Handle Shop Advice button being pressed
	private void OnAdviceButtonPressed()
	{
		// Generate Random Greeting Text
		var randomText = GetRandomShopkeeperText("Advice");
		//GD.Print(randomText);

		// Replace Shopkeeper Text Box
		GetNode<Label>("ShopUI/ShopKeeperTextBoxContainer/ShopKeeperTextBox").Text = randomText;
	}

	// Method for generating random Shopkeeper text
	private string GetRandomShopkeeperText(string textRequestType)
	{
		//string randomText = "";
		//string lastText = "";
		//int size;

		RandomText = "";

		// Generate Random Text
		if (textRequestType == "Greeting")
		{
			CollectionSize = _gameData.ShopkeeperGreetingText.Count;
			RandomText = _gameData.ShopkeeperGreetingText[(int)(GD.Randi() % CollectionSize)];

			while (RandomText == LastShopkeeperText)
			{
				RandomText = _gameData.ShopkeeperGreetingText[(int)(GD.Randi() % CollectionSize)];
			}

			LastShopkeeperText = RandomText;
			
		} 
		else if (textRequestType == "Advice")
		{
			CollectionSize = _gameData.ShopkeeperAdviceText.Count;
			RandomText = _gameData.ShopkeeperAdviceText[(int)(GD.Randi() % CollectionSize)];

			while (RandomText == LastShopkeeperText)
			{
				RandomText = _gameData.ShopkeeperAdviceText[(int)(GD.Randi() % CollectionSize)];
			}

			LastShopkeeperText = RandomText;
		}
		else if (textRequestType == "Lore")
		{
			CollectionSize = _gameData.ShopkeeperLoreText.Count;
			RandomText = _gameData.ShopkeeperLoreText[(int)(GD.Randi() % CollectionSize)];

			while (RandomText == LastShopkeeperText)
			{
				RandomText = _gameData.ShopkeeperLoreText[(int)(GD.Randi() % CollectionSize)];
			}

			LastShopkeeperText = RandomText;
		}

		return RandomText;
	}

	// Handle refreshing the Shop Cards
	private void HandleRefreshShopCards()
	{
		// Item Card 1
		GetNode<Label>("ShopUI/ItemCardContainer1/CardLabel").Text = _gameData.CurrentShopItemNames[0];
		// Item Card 2
		GetNode<Label>("ShopUI/ItemCardContainer2/CardLabel").Text = _gameData.CurrentShopItemNames[1];
		// Item Card 3
		GetNode<Label>("ShopUI/ItemCardContainer3/CardLabel").Text = _gameData.CurrentShopItemNames[2];

		// Item Card 4
		GetNode<Label>("ShopUI/ItemCardContainer4/CardLabel").Text = _gameData.CurrentShopItemNames[3];
		// Item Card 5
		GetNode<Label>("ShopUI/ItemCardContainer5/CardLabel").Text = _gameData.CurrentShopItemNames[4];
		// Item Card 6
		GetNode<Label>("ShopUI/ItemCardContainer6/CardLabel").Text = _gameData.CurrentShopItemNames[5];

		// Special Sale Card
		GetNode<Label>("ShopUI/SpecialSaleContainer/CardLabel").Text = _gameData.CurrentShopSpecialSaleName;
	}

}
