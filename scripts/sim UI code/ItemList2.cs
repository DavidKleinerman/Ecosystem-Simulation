using Godot;
using System;

public class ItemList2 : ItemList
{
	public override void _Ready()
	{
		this.Visible = false;    
	}
	private void _on_DisplayCharts_pressed()
	{
   		this.Visible = true;
	}
}
