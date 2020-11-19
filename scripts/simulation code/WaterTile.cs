using Godot;
using System;

public class WaterTile : Spatial
{
	public void AddWallCollider(){
		if(IsNearGround()){
			PackedScene wallCollider = (PackedScene)GD.Load("res://assets/biomes/WallCollider.tscn");
			Node wallColliderInst = wallCollider.Instance();
			AddChild(wallColliderInst);
		}
	}

	private bool IsNearGround(){
		Vector3 position = this.Translation;
		position.x = this.Translation.x + 4;
		position.z = this.Translation.z + 4;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x + 4;
		position.z = this.Translation.z;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x + 4;
		position.z = this.Translation.z - 4;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x;
		position.z = this.Translation.z + 4;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x;
		position.z = this.Translation.z - 4;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x - 4;
		position.z = this.Translation.z + 4;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x - 4;
		position.z = this.Translation.z;
		if (CheckCoordinate(position))
			return true;
		position.x = this.Translation.x - 4;
		position.z = this.Translation.z - 4;
		if (CheckCoordinate(position))
			return true;
		return false;
	}

	private bool CheckCoordinate(Vector3 selectedPos){
		foreach (Node n in GetTree().GetNodesInGroup("GroundTiles")){
			Vector3 position = ((Spatial)n).Translation;
			if (position.x == selectedPos.x && position.y == selectedPos.y && position.z == selectedPos.z){
				return true;
			}
		}
		return false;
	}
}
