using Godot;
using System;

public class CreatureCollider : Area
{
	public bool MyCreatureAlive = true;
	public Species.Creature MyCreature;
	public MultiMeshMeat.Meat MyMeat = null;
}
