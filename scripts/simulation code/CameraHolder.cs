using Godot;
using System;

public class CameraHolder : KinematicBody
{
private const float speed = 900;
	private Vector3 velocity = (Vector3) new Vector3(0,0,0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{
		move(delta);
	}

	private void move(float delta){
		if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_left"))
			velocity.x = 0;
		else if (Input.IsActionPressed("ui_right"))
			velocity.x = speed*delta;
		else if (Input.IsActionPressed("ui_left"))
			velocity.x = -speed*delta;
		else
			velocity.x = 0;

		if (Input.IsActionPressed("ui_up") && Input.IsActionPressed("ui_down"))
			velocity.z = 0;
		else if (Input.IsActionPressed("ui_up"))
			velocity.z = -speed*delta;
		else if (Input.IsActionPressed("ui_down"))
			velocity.z = speed*delta;
		else
			velocity.z = 0;

		MoveAndSlide(velocity, Vector3.Up);
	}
}
