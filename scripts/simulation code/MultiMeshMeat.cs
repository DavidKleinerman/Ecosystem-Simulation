using Godot;
using System;

public class MultiMeshMeat : MultiMeshInstance
{
	private const float MaxTimeOnGround = 12;

	public class Meat : Godot.Reference { //godot has major bugs when using structs. This is a work-around.
		public Spatial meatSpatial;
		public float timeOnGround = 0f;
		public int EatersCount = 0;
		public CreatureCollider Collider;
		public int decay = 0;
		public bool meatGone = false;
	}
	private Godot.Collections.Array<Meat> MeatArray = (Godot.Collections.Array<Meat>) new Godot.Collections.Array<Meat>();
	private Godot.Collections.Array<Meat> MeatToAdd = (Godot.Collections.Array<Meat>) new Godot.Collections.Array<Meat>();
	private Godot.Collections.Array MeatToRemove = new Godot.Collections.Array();

	public override void _Ready()
	{
		this.Multimesh.InstanceCount = 0;
	}
	
	public override void _Process(float delta)
	{
		Godot.Collections.Array<Meat> temp = (Godot.Collections.Array<Meat>) new Godot.Collections.Array<Meat>();
		for (int i = 0; i < MeatArray.Count; i++){
			if (!MeatToRemove.Contains(i))
				temp.Add(MeatArray[i]);
		}
		MeatArray.Clear();
		for (int i=0; i < temp.Count; i++){
			MeatArray.Add(temp[i]);
		}
		for(int i = 0; i < MeatToAdd.Count; i++){
			MeatArray.Add(MeatToAdd[i]);
		}
		MeatToRemove.Clear();
		MeatToAdd.Clear();
		temp.Clear();
		Multimesh.InstanceCount = MeatArray.Count;
		for (int i = 0; i < MeatArray.Count; i++){
			MeatArray[i].timeOnGround += delta;
			if (MeatArray[i].timeOnGround >= MaxTimeOnGround){
				MeatArray[i].decay = 1;
			}
			if (MeatArray[i].EatersCount> 0 || MeatArray[i].decay > 0){
				Vector3 eatRate = (Vector3) new Vector3(1,1,1);
				eatRate *= (MeatArray[i].EatersCount + MeatArray[i].decay) * 0.5f * delta;
				MeatArray[i].meatSpatial.Scale -= eatRate;
				if (MeatArray[i].meatSpatial.Scale.x < 0.05f){
					MeatArray[i].meatGone = true;
					MeatArray[i].Collider.QueueFree();
					MeatToRemove.Add(i);
				}
			}
			Multimesh.SetInstanceTransform(i, MeatArray[i].meatSpatial.Transform);
		}
	}

	public void AddMeat(Spatial meatSpatial, CreatureCollider collider){
		Meat newMeat = (Meat) new Meat();
		collider.MyMeat = newMeat;
		collider.MyCreatureAlive = false;
		newMeat.meatSpatial = meatSpatial;
		newMeat.Collider = collider;
		MeatToAdd.Add(newMeat);
		GD.Print("new meat collider poisition: " + newMeat.Collider.Translation);
		GD.Print("new meat mesh poisition: " + newMeat.meatSpatial.Translation);
	}
}
