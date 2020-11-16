using Godot;
using System;

public class Trait : Control
{


	private String validFloat;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(Input.IsActionJustPressed("ui_accept")){
			CorrectValue();
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseButton){

			if(!(new Rect2(GetNode<SpinBox>("SpinBox").GetLineEdit().RectGlobalPosition, GetNode<SpinBox>("SpinBox").GetLineEdit().RectSize).HasPoint(((InputEventMouseButton)inputEvent).Position))){
				CorrectValue();
			}
		}
	}

	private void CorrectValue(){
		if(!GetNode<SpinBox>("SpinBox").GetLineEdit().Text.IsValidFloat())
				GetNode<SpinBox>("SpinBox").GetLineEdit().Text = (GetNode<HSlider>("HSlider").Value.ToString());
				GetNode<SpinBox>("SpinBox").Value = GetNode<HSlider>("HSlider").Value;
				GetNode<SpinBox>("SpinBox").GetLineEdit().ReleaseFocus();
	}

	private void _on_SpinBox_value_changed(float value)
	{
		GetNode<HSlider>("HSlider").Value = value;
		GD.Print("changed hslider value!");
	}

	private void _on_HSlider_value_changed(float value)
	{
		GetNode<SpinBox>("SpinBox").Value = value;
		GD.Print("changed spinbox value!");
	}


}

