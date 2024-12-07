using Godot;
using System;

public partial class Unit : Resource
{
	// Properties
	// Other Stats:
	public string UnitName { get; set; } = "Empty";
	public string UnitClass { get; set; } = "Fighter";
	public string UnitSoul { get; set; } = "Nature";
	public int UnitTier { get; set; } = 1;
	public bool IsUnitDead {get; set; } = false;

	// Growth Stats:
	public int CurrentLevel { get; set; } = 1;
	public int CurrentMaxLevel { get; set; } = 5;
	public int FinalMaxLevel { get; set; } = 99;
	public int CurrentExperience { get; set; } = 0;
	public int ExperienceForNextLevel { get; set; } = 100;

	// Resource Stats:
	public float Health { get; set; } = 100.0f;
	public float Mana { get; set; } = 10.0f;
	public int ActionPoints { get; set; } = 3;
	public float EnergyShield { get; set; } = 0.0f;

	// Base Stats:
	public int Armor { get; set; } = 1;
	public int MagicArmor { get; set; } = 1;
	//public int Ward { get; set; } = 0;

	public int NatureResistance { get; set; } = 0;
	public int HolyResistance { get; set; } = 0;
	public int UndeadResistance { get; set; } = 0;
	public int DemonResistance { get; set; } = 0;
	public int EldritchResistance { get; set; } = 0;

	public int MeleeSkill { get; set; } = 1;
	public int RangedSkill { get; set; } = 1;

	public int MeleeDodge { get; set; } = 1;
	public int RangedDodge { get; set; } = 1;

	public float HealthRegeneration { get; set; } = 0.0f;
	public float ManaRegeneration { get; set; } = 1.0f;
	public float EnergyShieldRegeneration { get; set; } = 0.0f;

	public float HealthDegeneration { get; set; } = 0.0f;
	public float ManaDegeneration { get; set; } = 0.0f;

	public int CriticalHitChance { get; set; } = 1;
	public float CriticalHitDamageMultiplier { get; set; } = 2.0f;

	public int Initiative { get; set; } = 1;

	// Constructors
	public Unit()
	{
		
	}

	// Methods
}
