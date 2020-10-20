using Godot;
using System;

public class CameraHolder : KinematicBody
{
	private const float speed = 3000;

	private float zoomIntensity = 600;
	private Vector3 velocity = (Vector3) new Vector3(0,0,0);

	private Vector3 zoomDirection = (Vector3) new Vector3(0,0,0);

	private Vector2 screenCenter = (Vector2) new Vector2(0, 0);

	private bool atTop = false;

	private bool atBottom = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{	
		Move(delta);
		Zoom(delta);
		
	}

	private void Move(float delta){
		velocity.y = 0;
		if (Input.IsActionPressed("right") && Input.IsActionPressed("left"))
			velocity.x = 0;
		else if (Input.IsActionPressed("right"))
			velocity.x = speed*delta;
		else if (Input.IsActionPressed("left"))
			velocity.x = -speed*delta;
		else
			velocity.x = 0;

		if (Input.IsActionPressed("up") && Input.IsActionPressed("down"))
			velocity.z = 0;
		else if (Input.IsActionPressed("up"))
			velocity.z = -speed*delta;
		else if (Input.IsActionPressed("down"))
			velocity.z = speed*delta;
		else
			velocity.z = 0;

		MoveAndSlide(velocity, Vector3.Up);
	}

	private void Zoom(float delta){
		Vector2 mousePos = (Vector2) new Vector2(0, 0);
		mousePos = GetViewport().GetMousePosition();
		Vector3 rayFrom = ((Camera)GetNode("Camera")).ProjectRayOrigin(mousePos);
		if (Input.IsActionJustReleased("zoom_in") && !atBottom){
			zoomDirection = rayFrom + ((Camera)GetNode("Camera")).ProjectRayNormal(mousePos) * zoomIntensity;
		}
		else if(Input.IsActionJustReleased("zoom_out") && !atTop){
			zoomDirection = rayFrom + ((Camera)GetNode("Camera")).ProjectRayNormal(mousePos) * -zoomIntensity;
		}
		else {
			zoomDirection.x = 0;
			zoomDirection.y = 0;
			zoomDirection.z = 0;
		}
			MoveAndSlide(zoomDirection);
	}


	public void SetAtTop(bool atTop){
		this.atTop = atTop;
	}

	public bool getTop(){
		return atTop;
	}

	public void SetAtBottom(bool atBottom){
		this.atBottom = atBottom;
	}

	public bool getBottom(){
		return atBottom;
	}
}
