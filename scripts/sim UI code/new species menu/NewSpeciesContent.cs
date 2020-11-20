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
		GetNode<Label>("GeneticVariation/Label").Text = "Genetic Variation";
		GetNode<Label>("PopulationSize/Label").Text = "Population Size";
		
	}

	private void _on_StartSimulation_pressed()
	{
		int numOfGroundTiles = GetTree().Root.GetTree().GetNodesInGroup("GroundTiles").Count;
		GD.Print("number of ground tiles is" + numOfGroundTiles);
		GetNode<SpinBox>("PopulationSize/SpinBox").MinValue = 1;
		GetNode<HSlider>("PopulationSize/HSlider").MinValue = 1;
		GetNode<SpinBox>("PopulationSize/SpinBox").MaxValue = numOfGroundTiles*4;
		GetNode<HSlider>("PopulationSize/HSlider").MaxValue = numOfGroundTiles*4;
		GetNode<SpinBox>("PopulationSize/SpinBox").Step = 1;
		GetNode<HSlider>("PopulationSize/HSlider").Step = 1;
		GetNode<SpinBox>("PopulationSize/SpinBox").Rounded = true;
		GetNode<HSlider>("PopulationSize/HSlider").Rounded = true;
		GetNode<SpinBox>("PopulationSize/SpinBox").HintTooltip = "Min is " + 1 + ". Max is " + numOfGroundTiles*4 + ".";
		GetNode<HSlider>("PopulationSize/HSlider").HintTooltip = "Min is " + 1 + ". Max is " + numOfGroundTiles*4 + ".";
		GetNode<Label>("PopSizeWarning/Label").Text = "Max Population Size = " + numOfGroundTiles*4;
		GetNode<Label>("PopulationSize/Label").HintTooltip = "Max Population Size = Habitable Area x 4 : \n(" + numOfGroundTiles*4 + " = " + numOfGroundTiles + " x 4)";
		GetNode<Label>("PopulationSize/Label").MouseFilter = 0;
		GetNode<Label>("PopSizeWarning/Label").HintTooltip = "Max Population Size = Habitable Area x 4 : \n(" + numOfGroundTiles*4 + " = " + numOfGroundTiles + " x 4)";
		GetNode<Label>("PopSizeWarning/Label").MouseFilter = 0;

	}

	private void _on_NewSpeciesButton_pressed()
	{
		String speciesName = GetNode<LineEdit>("SpeciesName/LineEdit").Text;
		int popSize = (int)(GetNode<HSlider>("PopulationSize/HSlider").Value);
		Color speciesColor = GetNode<ColorPicker>("RepresentationsPicker/ColorPicker").Color;
		GetTree().CallGroup("SpeciesHolder", "AddSpecies", speciesName, popSize, speciesColor);
	}
}


