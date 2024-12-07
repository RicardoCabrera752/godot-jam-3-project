using Godot;
using System;

public partial class ItemButton : Button
{
	// Properties
	public TextureRect ButtonIcon;
	public Label ButtonLabel;
	public int Index;
	public string Content;
	public bool IsHoldingItem;

	// Access to custom signals
	private CustomSignals _customSignals;

	// Methods

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonIcon = GetNode<TextureRect>("ButtonIcon");
		ButtonLabel = GetNode<Label>("CountLabel");

		_customSignals = GetTree().Root.GetNode<CustomSignals>("CustomSignals");

		//ButtonIcon.Texture = (Texture2D)GD.Load("res://images/icon.svg");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateItemButton(int index, string content, Texture2D texture, string count, bool isHoldingItem)
	{
		Index = index;
		Content = content;
		ButtonIcon.Texture = texture;
		ButtonLabel.Text = count;
		IsHoldingItem = isHoldingItem;
		
	}
	
	public void UpdateItemButtonCount(string count)
	{
		ButtonLabel.Text = count;
	}

	public void ResetItemButton(int index)
	{
		Index = index;
		Content = "";
		ButtonIcon.Texture = null;
		ButtonLabel.Text = "";
		IsHoldingItem = false;
	}

	private void OnPressed()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.InspectInventoryItem), Index, Content);
	}
}
