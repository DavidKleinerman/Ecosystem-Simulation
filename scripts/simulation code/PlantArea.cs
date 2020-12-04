using Godot;
using System;

public class PlantArea : Area
{
	private GroundTile MyTile;
		public override void _Ready()
	{
		
	}

public void SetMyTile(GroundTile tile){
	MyTile = tile;
}

public void AddEater(Creature eater){
	MyTile.AddEater(eater);
}

public void RemoveEater(Creature eater){
	MyTile.RemoveEater(eater);
}

public void AddFutureEater(Creature eater){
	MyTile.AddFutureEater(eater);
}

public void RemoveFutureEater(Creature eater){
	MyTile.RemoveFutureEater(eater);
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
