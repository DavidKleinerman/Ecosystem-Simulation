using Godot;
using System;

public class NewSpeciesContent : VBoxContainer
{
	public override void _Ready()
	{
		GetNode<Label>("Speed/Label").Text = "Speed";
		GetNode<Label>("Perception/Label").Text = "Perception";
		GetNode<Label>("MatingCycle/Label").Text = "Mating Cycle";
		GetNode<Label>("HungerResistance/Label").Text = "Hunger Resistance";
		GetNode<Label>("ThirstResistance/Label").Text = "Thirst Resistance";
		GetNode<Label>("Gestation/Label").Text = "Gestation";
	}

}
