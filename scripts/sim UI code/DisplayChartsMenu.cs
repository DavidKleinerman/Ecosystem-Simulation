using Godot;
using System;

public class DisplayChartsMenu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void _on_DisplayCharts_pressed()
	{
		this.Visible = true;
	}
	private void _on_CloseChartMenu_pressed()
	{
		this.Visible = false;
	}
}
