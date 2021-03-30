using Godot;
using System;

public class NewSpeciesContent : VBoxContainer
{
	private int numOfGroundTiles;
	public override void _Ready()
	{
		GetNode<Label>("Speed/Label").Text = "Speed";
		GetNode<Label>("Speed/Label").HintTooltip = "The creature's movement speed.\nThe faster the creature moves the higher is the energy cost.\nLarger number = higher speed.";
		GetNode<Label>("Perception/Label").Text = "Perception";
		GetNode<Label>("Perception/Label").HintTooltip = "How far the creature can detect objects (water, food, and other creatures).\nMore advanced sensory system requires more energy upkeep.\nLarger number = larger detection radius.";
		GetNode<Label>("MatingCycle/Label").Text = "Mating Cycle";
		GetNode<Label>("MatingCycle/Label").HintTooltip = "How often the creature will want to reproduce.\nLarger number = more frequent reproduction.";
		GetNode<Label>("HungerResistance/Label").Text = "Hunger Resistance";
		GetNode<Label>("HungerResistance/Label").HintTooltip = "Reduces the passive dacay of the creature's energy.\nLarger number = larger the reduction to the energy decay.";
		GetNode<Label>("ThirstResistance/Label").Text = "Thirst Resistance";
		GetNode<Label>("ThirstResistance/Label").HintTooltip = "Reduces the passive growth of the creature's thirst.\nLarger number = larger the reduction to the thirst growth.";
		GetNode<Label>("Gestation/Label").Text = "Gestation";
		GetNode<Label>("Gestation/Label").HintTooltip = "Females Only. Determines the length of the pregnancy period.\nLarger number = longer pregnancies but with stronger offsprings.";
		GetNode<Label>("LitterSize/Label").Text = "LitterSize";
		GetNode<Label>("LitterSize/Label").HintTooltip = "Females Only. How many offsprings are born in a single clutch.\nEach child born reduces the mother's energy.\nLarger number = more children born per clutch.";
		GetNode<Label>("Longevity/Label").Text = "Longevity";
		GetNode<Label>("Longevity/Label").HintTooltip = "How long the creature will live.\nLarger number = longer lifespan.";
		GetNode<Label>("Intelligence/Label").Text = "Intelligence";
		GetNode<Label>("Intelligence/Label").HintTooltip = "How long it takes for the creature to process its environment.\nA bigger brain with higher intelligence requires a higher energy upkeep.\nLarger number = smaller delay between environment scans.";
		GetNode<Label>("Memory/Label").Text = "Memory";
		GetNode<Label>("Memory/Label").HintTooltip = "If male: how many females that rejected him he can remember.\nIf female: how many rejected males she can remember.\nIf predator: how many unsuccessful hunting attapts the predastor can remember.\nA bigger brain with better memory requires a higher energy upkeep.\nLarger number = better memory.";
		GetNode<Label>("Strength/Label").Text = "Strength";
		GetNode<Label>("Strength/Label").HintTooltip = "Influences the foodchain since creature would not hunt other creature that are stronger than them.\nRepresents the muscular strength and the mass of the creature.\nBigger muscles and mass require a higher energy upkeep.\nLarger number = higher strength.";
		GetNode<Label>("GeneticVariation/Label").Text = "Genetic Variation";
		GetNode<Label>("GeneticVariation/Label").HintTooltip = "The difference of each indvidual's trait value, in the initial population, form the values set by the user.";
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
			InitialValues.Add((float)GetNode<HSlider>("Intelligence/HSlider").Value);
			InitialValues.Add((float)GetNode<HSlider>("Memory/HSlider").Value);
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





