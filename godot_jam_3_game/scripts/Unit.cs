using Godot;
using System;

public partial class Unit : Resource
{
	// Properties
	// Basic Stats:
	public int ID { get; set; }
	public bool IsAlive { get; set; } = false;
	public string UnitChasis { get; set; } = "Empty";
	public string UnitRole { get; set; } = "Empty";
	public int CreditCost { get; set; } = 0;
	public Texture2D IconTexture { get; set; }

	// Component Stats:

	// Resource Stats:
	public float Armor { get; set; } = 0.0f;
	public float Structure { get; set; } = 0.0f;
	public float HeatCapacity { get; set; } = 0.0f;
	public float ActionPoints { get; set; } = 0.0f;

	public float ArmorRegeneration { get; set; } = 0.0f;
	public float StructureRegeneration { get; set; } = 0.0f;
	public float ArmorDegeneration { get; set; } = 0.0f;
	public float StructureDegeneration { get; set; } = 0.0f;

	public float HeatDissipation { get; set; } = 0.0f;

	// Combat Stats:

	public int LongRangeSensors { get; set; } = 0;
	public int CloseRangeSensors { get; set; } = 0;
	public int Evasion { get; set; } = 0;

	public int EnergyResistance { get; set; } = 0;
	public int BallisticResistance { get; set; } = 0;
	public int MissileResistance { get; set; } = 0;
	public int MeleeResistance { get; set; } = 0;

	public int ElectronicCounterMeasures { get; set; } = 0;

	// Constructors
	public Unit()
	{
		
	}

	// Methods
}
