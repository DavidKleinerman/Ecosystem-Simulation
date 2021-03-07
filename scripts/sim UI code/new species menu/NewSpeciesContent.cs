using Godot;
using System;

public class NewSpeciesContent : VBoxContainer
{
	private int numOfGroundTiles;
	public override void _Ready()
	{
		GetNode<Label>("Speed/Label").Text = "Speed";
		GetNode<Label>("Perception/Label").Text = "Perception";
		GetNode<Label>("MatingCycle/Label").Text = "Mating Cycle";
		GetNode<Label>("HungerResistance/Label").Text = "Hunger Resistance";
		GetNode<Label>("ThirstResistance/Label").Text = "Thirst Resistance";
		GetNode<Label>("Gestation/Label").Text = "Gestation";
		GetNode<Label>("LitterSize/Label").Text = "LitterSize";
		GetNode<Label>("Longevity/Label").Text = "Longevity";
		GetNode<Label>("GeneticVariation/Label").Text = "Genetic Variation";
		GetNode<Label>("PopulationSize/Label").Text = "Population Size";
		
	}

	private void _on_StartSimulation_pressed()
	{
		int numOfGroundTiles = GetNode<BiomeGrid>("../../../BiomeGrid").GetGroundTiles().Count;
		GD.Print("number of ground tiles is" + numOfGroundTiles);
		GetNode<SpinBox>("PopulationSize/SpinBox").MinValue = 1;
		GetNode<HSlider>("PopulationSize/HSlider").MinValue = 1;
		GetNode<SpinBox>("PopulationSize/SpinBox").MaxValue = numOfGroundTiles;
		GetNode<HSlider>("PopulationSize/HSlider").MaxValue = numOfGroundTiles;
		GetNode<SpinBox>("PopulationSize/SpinBox").Step = 1;
		GetNode<HSlider>("PopulationSize/HSlider").Step = 1;
		GetNode<SpinBox>("PopulationSize/SpinBox").Rounded = true;
		GetNode<HSlider>("PopulationSize/HSlider").Rounded = true;
		GetNode<SpinBox>("PopulationSize/SpinBox").HintTooltip = "Min is " + 1 + ". Max is " + numOfGroundTiles;
		GetNode<HSlider>("PopulationSize/HSlider").HintTooltip = "Min is " + 1 + ". Max is " + numOfGroundTiles;
		GetNode<Label>("PopSizeWarning/Label").Text = "Max Population Size = " + numOfGroundTiles;
		GetNode<Label>("PopulationSize/Label").HintTooltip = "Max population is limited by the size of the habitable area (" + numOfGroundTiles + ")";
		GetNode<Label>("PopulationSize/Label").MouseFilter = 0;
		GetNode<Label>("PopSizeWarning/Label").HintTooltip = "Max population is limited by the size of the habitable area (" + numOfGroundTiles + ")";
		GetNode<Label>("PopSizeWarning/Label").MouseFilter = 0;

	}

	private void _on_NewSpeciesButton_pressed()
	{
		Godot.Collections.Array speciesList = GetTree().GetNodesInGroup("Species");
		Godot.Collections.Array speciesNames = (Godot.Collections.Array) new Godot.Collections.Array();
		foreach (Node n in speciesList){
			speciesNames.Add(((Species)n).GetSpeciesName());
		}
		if (speciesNames.Contains(GetNode<LineEdit>("SpeciesName/LineEdit").Text)){
			GetParent<ScrollContainer>().ScrollVertical = 0;
			GetNode<LineEdit>("SpeciesName/LineEdit").Clear();
			GetNode<LineEdit>("SpeciesName/LineEdit").GrabFocus();
			GetNode<LineEdit>("SpeciesName/LineEdit").PlaceholderText = "A Species With This Name Already Exists!";
			GetNode<LineEdit>("SpeciesName/LineEdit").PlaceholderAlpha = 1;
		} else {
			Godot.Collections.Array InitialValues = (Godot.Collections.Array) new Godot.Collections.Array();
			InitialValues.Add((float)GetNode<HSlider>("Speed/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("Perception/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("MatingCycle/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("HungerResistance/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("ThirstResistance/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("Gestation/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("LitterSize/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("Longevity/HSlider").Value);
			float geneticVariation = (float)GetNode<HSlider>("GeneticVariation/HSlider").Value;
			String speciesName = GetNode<LineEdit>("SpeciesName/LineEdit").Text;
			int popSize = (int)(GetNode<HSlider>("PopulationSize/HSlider").Value);
			Color speciesColor = GetNode<ColorPicker>("RepresentationsPicker/ColorPicker").Color;
			GetTree().CallGroup("SpeciesHolder", "AddSpecies", speciesName, popSize, speciesColor, InitialValues, geneticVariation);
			GetNode<LineEdit>("SpeciesName/LineEdit").Clear();
		}
	}

	private void _on_LineEdit_gui_input(object @event)
	{
		GetNode<LineEdit>("SpeciesName/LineEdit").PlaceholderText = "Species Name";
		GetNode<LineEdit>("SpeciesName/LineEdit").PlaceholderAlpha = 0.6f;
	}
}





