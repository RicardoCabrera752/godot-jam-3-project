using Godot;
using System;

public partial class Item : Resource
{
	// Properties
	[Export]
	public int ID { get; set; }
	[Export] 
	public string ItemName { get; set; }
	[Export] 
	public int RuneCost { get; set; }
	//[Export] public string ResourcePath { get; set; }

	[Export] public Texture2D Icon { get; set; }
	//[Export] public string IconPath { get; set; }
	[Export] 
	public int Quantity { get; set; }
	[Export] 
	public int MaxStackSize { get; set; }
	[Export] 
	public bool IsStackable { get; set; }
	[Export] public string Description { get; set; }

	// Constructors
	public Item()
	{
		
	}

	public Item(int id, string itemName, int runeCost, Texture2D icon, int quantity, int maxStackSize, bool isStackable, string description)
	{
		ID = id;
		ItemName = itemName;
		RuneCost = runeCost;
		Icon = icon;
		Quantity = quantity;
		MaxStackSize = maxStackSize;
		IsStackable = isStackable;
		Description = description;
	}

	public Item DeepCopy()
	{
		Item clone = (Item)this.MemberwiseClone();
		return clone;
	}

	public Item Copy() => MemberwiseClone() as Item;
}
