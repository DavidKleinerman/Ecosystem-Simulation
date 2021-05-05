using Godot;
using System;

public class LoadSimButton : Button
{
	public override void _Ready()
	{
		
	}

	private void _on_FileDialog_confirmed()
	{
		try{
			Global.IsLoaded = true;
			String path = GetParent().GetNode<FileDialog>("FileDialog").CurrentPath;
			var saveFile = new File();
			if (!saveFile.FileExists(path)){
				GD.Print("problem in opening file in path: " + path);
				return;
			}
			saveFile.Open(path, File.ModeFlags.Read);
			var loadedData = new Godot.Collections.Dictionary((Godot.Collections.Dictionary)JSON.Parse(saveFile.GetLine()).Result);
			Global.LoadedTiles = (Godot.Collections.Array)loadedData["BiomeTiles"];
			Global.LoadedMeat = (Godot.Collections.Array)loadedData["Meat"];
			Global.LoadedSpecies = (Godot.Collections.Array)loadedData["Species"];
			Global.worldSize = (int)((float)loadedData["WorldSize"]);
			Global.biomeGrowthRate = (float)loadedData["PlantGrowthRate"];
			Global.LoadedGlobalTime = (Godot.Collections.Array)loadedData["GloablTimeArray"];
			Global.LoadedGlobalWaitingTime = (float)loadedData["GlobalWaitingTime"];
			Global.LoadedBiomesWaitingTime = (float)loadedData["BiomesWaitingTime"];
			GetTree().ChangeScene("res://assets/Simulation.tscn");
		} catch (Exception e) {
			return;
		}
		
	}

}



