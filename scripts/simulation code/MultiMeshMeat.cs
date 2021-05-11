using Godot;
using System;

public class MultiMeshMeat : MultiMeshInstance
{
	private const float MaxTimeOnGround = 12f;
	private float TimeMultiplier = 1;

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
	private Godot.Collections.Array<Meat> temp = (Godot.Collections.Array<Meat>) new Godot.Collections.Array<Meat>();
	private Godot.Collections.Array MeatBiomassArray = new Godot.Collections.Array();

	public override void _Ready()
	{
		int i = 0;
		this.Multimesh.InstanceCount = 0;
		if (Global.IsLoaded){
			this.Multimesh.InstanceCount = Global.LoadedMeat.Count;
			foreach (Godot.Collections.Dictionary m in Global.LoadedMeat){
				Meat newMeat = new Meat();
				newMeat.meatSpatial = (Spatial) new Spatial();
				newMeat.meatSpatial.Translation = new Vector3((float)m["MeatTranslationX"], (float)m["MeatTranslationY"], (float)m["MeatTranslationZ"]);
				newMeat.meatSpatial.Scale = new Vector3((float)m["MeatScaleX"], (float)m["MeatScaleY"], (float)m["MeatScaleZ"]);
				newMeat.timeOnGround = (float)m["TimeOnGround"];
				newMeat.EatersCount = (int)((float)m["EatersCount"]);
				newMeat.Collider = (CreatureCollider) new CreatureCollider();
				newMeat.Collider.Translation = new Vector3((float)m["ColliderTranslationX"], (float)m["ColliderTranslationY"], (float)m["ColliderTranslationZ"]);
				newMeat.Collider.MyMeat = newMeat;
				newMeat.Collider.MyCreatureAlive = false;
				AddChild(newMeat.Collider);
				newMeat.decay = (int)((float)m["Decay"]);
				newMeat.meatGone = (bool)m["MeatGone"];
				MeatArray.Add(newMeat);
				Multimesh.SetInstanceTransform(i, newMeat.meatSpatial.Transform);
				i++;
			}
		} else {
			MeatBiomassArray.Add(0.0f);
		}
	}

	public void UpdateMeatBiomass(){
		float currentMeatBiomass = 0.0f;
		for (int i = 0; i < MeatArray.Count; i++){
			if (!MeatArray[i].meatGone)
				currentMeatBiomass += MeatArray[i].meatSpatial.Scale.x;
		}
		MeatBiomassArray.Add(currentMeatBiomass);
	}

	public Godot.Collections.Array GetMeatBiomassArray(){
		return MeatBiomassArray;
	}
	
	public override void _Process(float delta)
	{
		
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
			MeatArray[i].timeOnGround += TimeMultiplier * delta;
			if (MeatArray[i].timeOnGround >= MaxTimeOnGround){
				MeatArray[i].decay = 1;
			}
			if (MeatArray[i].EatersCount> 0 || MeatArray[i].decay > 0){
				Vector3 eatRate = (Vector3) new Vector3(1,1,1);
				eatRate *= (MeatArray[i].EatersCount + MeatArray[i].decay) * 0.5f * TimeMultiplier * delta;
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
	}

	public Godot.Collections.Array Save(){
		Godot.Collections.Array savedMeat = new Godot.Collections.Array();
		for(int i = 0; i < MeatArray.Count; i++){
			if (!MeatToRemove.Contains(i)){
				savedMeat.Add(MeatToDictionary(i, MeatArray));
			}
		}
		for(int i = 0; i < MeatToAdd.Count; i++){
			savedMeat.Add(MeatToDictionary(i, MeatToAdd));
		}
		return savedMeat;
	}

	private Godot.Collections.Dictionary<String, object> MeatToDictionary(int i, Godot.Collections.Array<Meat> array){
		Godot.Collections.Dictionary<String, object> meatDictionary = new Godot.Collections.Dictionary<String, object>() {
			{"MeatTranslationX", array[i].meatSpatial.Translation.x},
			{"MeatTranslationY", array[i].meatSpatial.Translation.y},
			{"MeatTranslationZ", array[i].meatSpatial.Translation.z},
			{"MeatScaleX", array[i].meatSpatial.Scale.x},
			{"MeatScaleY", array[i].meatSpatial.Scale.y},
			{"MeatScaleZ", array[i].meatSpatial.Scale.z},
			{"TimeOnGround", array[i].timeOnGround},
			{"EatersCount", array[i].EatersCount},
			{"ColliderTranslationX", array[i].Collider.Translation.x},
			{"ColliderTranslationY", array[i].Collider.Translation.y},
			{"ColliderTranslationZ", array[i].Collider.Translation.z},
			{"Decay", array[i].decay},
			{"MeatGone", array[i].meatGone}
		};
		return meatDictionary;
	}

	private void _on_SimulationRate_item_selected(int index)
	{
		switch (index){
			case 0:
				TimeMultiplier = 1f;
			break;
			case 1:
				TimeMultiplier = 2f;
			break;
			case 2:
				TimeMultiplier = 4f;
			break;
		}
	}
}



