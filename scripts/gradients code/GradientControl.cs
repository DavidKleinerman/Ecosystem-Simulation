using Godot;
using System;

public class GradientControl : Control
{
   	private Line2D lineSpeed;
	private Line2D lineStamina;
	private Line2D linePerception;
	private Line2D lineMatingCycle;
	private Line2D lineHungerResistance;
	private Line2D lineThirstResistance;
	private Line2D lineGestation;
	public override void _Ready()
	{
		//this.Visible = false;
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		Godot.Collections.Array arr = (Godot.Collections.Array) new Godot.Collections.Array();
		for (int i = 0; i < 100; i++){
			rng.Randomize();
			arr.Add(rng.Randf());
		}
		lineStamina = GetNode<Line2D>("StaminaGradient");
		
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,370 +(float)arr[i]*30);
			lineStamina.AddPoint(NewPoint);
		}
		//AddChild(line);
		lineSpeed = GetNode<Line2D>("SpeedGradient");
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,300+ ((float)arr[i]*50)/2);
			lineSpeed.AddPoint(NewPoint);
		}
		linePerception = GetNode<Line2D>("PerceptionGradient");
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,250+(float)arr[i]*50);
			linePerception.AddPoint(NewPoint);
		}
		lineGestation = GetNode<Line2D>("GestationGradient");
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,340+(float)arr[i]*80);
			lineGestation.AddPoint(NewPoint);
		}
		lineMatingCycle = GetNode<Line2D>("MatingCycleGradient");
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,330+(float)arr[i]*35);
			lineMatingCycle.AddPoint(NewPoint);
		}
		lineThirstResistance = GetNode<Line2D>("ThirstResistanceGradient");
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,280+(float)arr[i]*50);
			lineThirstResistance.AddPoint(NewPoint);
		}
		lineHungerResistance = GetNode<Line2D>("HungerResistanceGradient");
		for(int i=0; i < 10; i++){
			Vector2 NewPoint = (Vector2) new Vector2(i*98 ,320+(float)arr[i]*50);
			lineHungerResistance.AddPoint(NewPoint);
		}
		
		
	}
}
