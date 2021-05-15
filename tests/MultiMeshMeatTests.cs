using Godot;
using System;

public class MultiMeshMeatTests : WAT.Test
{
	
	public override String Title()
	{
		return "MultiMeshMeat Unit Tests";
	}
	
	[Test]
	public void UpdateMeatBiomassEmptyArray() 
	{
		MultiMeshMeat MMM = new MultiMeshMeat();
		MMM.UpdateMeatBiomass();
		Assert.IsEqual(0.0f, MMM.GetMeatBiomassArray()[0], "Then it passes"); 
	}

	[Test]
	public void UpdateMeatBiomassOneElementArrayWholeNumbers(){
		int Y = 1;
		MultiMeshMeat MMM = new MultiMeshMeat();
		MultiMeshMeat.Meat meat = new MultiMeshMeat.Meat();
		meat.meatSpatial = new Spatial();
		meat.meatSpatial.Scale = new Vector3(Y, 0, 0);
		
		MMM.GetMeatArray().Add(meat);
		MMM.UpdateMeatBiomass();
		Assert.IsEqual(1f, MMM.GetMeatBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void UpdateMeatBiomassOneElementArrayFractions(){
		float Y = 0.3f;
		MultiMeshMeat MMM = new MultiMeshMeat();
		MultiMeshMeat.Meat meat = new MultiMeshMeat.Meat();
		meat.meatSpatial = new Spatial();
		meat.meatSpatial.Scale = new Vector3(Y, 0, 0);
		
		MMM.GetMeatArray().Add(meat);
		MMM.UpdateMeatBiomass();
		Assert.IsEqual(0.3f, MMM.GetMeatBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void UpdateMeatBiomassMultiElementArrayWholeNumbers(){
		int X = 3;
		int Y = 1;
		
		MultiMeshMeat MMM = new MultiMeshMeat();
		for (int i = 0; i < X; i++){
			MultiMeshMeat.Meat meat = new MultiMeshMeat.Meat();
			meat.meatSpatial = new Spatial();
			meat.meatSpatial.Scale = new Vector3(Y, 0, 0);
			
			MMM.GetMeatArray().Add(meat);
		}
		MMM.UpdateMeatBiomass();
		Assert.IsEqual(3f, MMM.GetMeatBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void UpdateMeatBiomassMultiElementArrayFractions(){
		int X = 3;
		float Y = 0.3f;
		float Z = 0.3f + 0.3f + 0.3f;
		MultiMeshMeat MMM = new MultiMeshMeat();
		for (int i = 0; i < X; i++){
			MultiMeshMeat.Meat meat = new MultiMeshMeat.Meat();
			meat.meatSpatial = new Spatial();
			meat.meatSpatial.Scale = new Vector3(Y, 0, 0);
			
			MMM.GetMeatArray().Add(meat);
		}
		MMM.UpdateMeatBiomass();
		Assert.IsEqual(Z, MMM.GetMeatBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void AddMeatEmptyArrayZeroVector(){
		MultiMeshMeat MMM = new MultiMeshMeat();
		Spatial meatSpatial = new Spatial();
		CreatureCollider collider = new CreatureCollider();
		MMM.AddMeat(meatSpatial,collider);

		Assert.IsEqual(1, MMM.GetMeatToAdd().Count, "Then it Passes!");
        Assert.IsEqual(new Vector3(), ((Godot.Collections.Array<MultiMeshMeat.Meat>)MMM.GetMeatToAdd())[0].meatSpatial.Translation, "Then it Passes!");

	}
    [Test]
    public void AddMeatNotEmptyArrayZeroVector(){
        MultiMeshMeat.Meat newMeat = (MultiMeshMeat.Meat) new MultiMeshMeat.Meat();
        MultiMeshMeat MMM = new MultiMeshMeat();
		Spatial meatSpatial = new Spatial();
		CreatureCollider collider = new CreatureCollider();
		MMM.GetMeatToAdd().Add(newMeat);
        MMM.AddMeat(meatSpatial,collider);
        Assert.IsEqual(2, MMM.GetMeatToAdd().Count, "Then it Passes!");
        Assert.IsEqual(new Vector3(), ((Godot.Collections.Array<MultiMeshMeat.Meat>)MMM.GetMeatToAdd())[1].meatSpatial.Translation, "Then it Passes!");
    }
    	[Test]
	public void AddMeatEmptyArrayNotZeroVector(){
		MultiMeshMeat MMM = new MultiMeshMeat();
		Spatial meatSpatial = new Spatial();
        meatSpatial.Translation = new Vector3(1,1,1);
		CreatureCollider collider = new CreatureCollider();
		MMM.AddMeat(meatSpatial,collider);

		Assert.IsEqual(1, MMM.GetMeatToAdd().Count, "Then it Passes!");
        Assert.IsEqual(new Vector3(1,1,1), ((Godot.Collections.Array<MultiMeshMeat.Meat>)MMM.GetMeatToAdd())[0].meatSpatial.Translation, "Then it Passes!");

	}
    [Test]
    public void AddMeatNotEmptyArrayNotZeroVector(){
        MultiMeshMeat.Meat newMeat = (MultiMeshMeat.Meat) new MultiMeshMeat.Meat();
        MultiMeshMeat MMM = new MultiMeshMeat();
		Spatial meatSpatial = new Spatial();
        meatSpatial.Translation = new Vector3(1,1,1);
		CreatureCollider collider = new CreatureCollider();
		MMM.GetMeatToAdd().Add(newMeat);
        MMM.AddMeat(meatSpatial,collider);
        Assert.IsEqual(2, MMM.GetMeatToAdd().Count, "Then it Passes!");
        Assert.IsEqual(new Vector3(1,1,1), ((Godot.Collections.Array<MultiMeshMeat.Meat>)MMM.GetMeatToAdd())[1].meatSpatial.Translation, "Then it Passes!");
    }
   
	
}
