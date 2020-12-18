using Godot;
using System;

public class DisplayChartsMenu : Control
{
	
	public override void _Ready()
	{
		this.Visible = false;
		
	}

	private void _on_DisplayCharts_pressed()
	{
		this.Visible = true;
	}
	private void _on_CloseChartMenu_pressed()
	{
		this.Visible = false;
	}
	
}
