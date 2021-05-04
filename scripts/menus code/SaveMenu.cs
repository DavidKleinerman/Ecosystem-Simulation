using Godot;
using System;

public class SaveMenu : Control
{

	private void _on_SaveSimulation_pressed()
	{
		GetNode<FileDialog>("FileDialog").Popup_();
		GetParent().GetNode<Control>("PauseMenu").Visible = false;
	}
	private void _on_FileDialog_popup_hide()
	{
		GetParent().GetNode<Control>("PauseMenu").Visible = true;
	}
	
	private void _on_FileDialog_confirmed()
	{
		String path = GetNode<FileDialog>("FileDialog").CurrentPath;
		path += ".save";
		var saveFile = new File();
		saveFile.Open(path, File.ModeFlags.Write);
		Godot.Collections.Array savedTiles = GetParent().GetNode<BiomeGrid>("BiomeGrid").Save();
		Godot.Collections.Array savedMeat = GetParent().GetNode<MultiMeshMeat>("MultiMeshMeat").Save();
		Godot.Collections.Array globalTimeArray = GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetGlobalTimeArray();
		float GlobalCurrentWaitingTime = GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetCurrentWaitingTime();
		float BiomesCurrentWaitingTime = GetParent().GetNode<BiomeGrid>("BiomeGrid").GetCurrentWaitingTime();
		Godot.Collections.Dictionary<String, object> saveData = new Godot.Collections.Dictionary<String, object>() {
			{"WorldSize", Global.worldSize},
			{"PlantGrowthRate", Global.biomeGrowthRate},
			{"GloablTimeArray", globalTimeArray},
			{"BiomeTiles", savedTiles},
			{"Meat", savedMeat},
			{"GlobalWaitingTime", GlobalCurrentWaitingTime},
			{"BiomesWaitingTime", BiomesCurrentWaitingTime}
		};
		saveFile.StoreLine(JSON.Print(saveData));
	}

}









