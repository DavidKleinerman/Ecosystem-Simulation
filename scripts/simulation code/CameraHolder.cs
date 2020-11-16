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

	private bool atFront = false;
	private bool atBack = false;

	private bool atRight = false;
	private bool atLeft = false;

	private bool newSpeciesMenuOpened = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{	
		Move(delta);
		if (newSpeciesMenuOpened){
			Vector2 windowSize = GetViewport().Size;
			Vector2 mousePos = GetViewport().GetMousePosition();
			if (mousePos.x < windowSize.x - 600)
				Zoom(delta);
		} else Zoom(delta);
		
	}

	private void Move(float delta){
		velocity.y = 0;
		LeftRightMove(delta);
		FrontBackMove(delta);
		MoveAndSlide(velocity, Vector3.Up);
	}

	private void FrontBackMove(float delta){
		if (Input.IsActionPressed("up") && Input.IsActionPressed("down"))
			velocity.z = 0;
		else if (Input.IsActionPressed("up") && !atFront)
			velocity.z = -speed*delta;
		else if (Input.IsActionPressed("down") && !atBack)
			velocity.z = speed*delta;
		else
			velocity.z = 0;
	}

	private void LeftRightMove (float delta){
		if (Input.IsActionPressed("right") && Input.IsActionPressed("left"))
			velocity.x = 0;
		else if (Input.IsActionPressed("right") && !atRight)
			velocity.x = speed*delta;
		else if (Input.IsActionPressed("left") && !atLeft)
			velocity.x = -speed*delta;
		else
			velocity.x = 0;
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
			resetZoom();
		}
		MoveAndSlide(zoomDirection);
	}

	private void resetZoom(){
		zoomDirection.x = 0;
		zoomDirection.y = 0;
		zoomDirection.z = 0;
	}

	private void _on_AddNewSpecies_pressed()
	{
		newSpeciesMenuOpened = true;
	}


	private void _on_CloseNewSpecies_pressed()
	{
		newSpeciesMenuOpened = false;
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

	public bool getAtBottom(){
		return atBottom;
	}

	public void SetAtFront(bool atFront){
		this.atFront = atFront;
	}

	public bool getAtFront(){
		return atFront;
	}

	public void SetAtBack(bool atBack){
		this.atBack = atBack;
	}

	public bool getAtBack(){
		return atBack;
	}

	public void SetAtRight(bool atRight){
		this.atRight = atRight;
	}

	public bool getAtRight(){
		return atRight;
	}

	public void SetAtLeft(bool atLeft){
		this.atLeft = atLeft;
	}

	public bool getAtLeft(){
		return atLeft;
	}

}
