extends KinematicBody


var SpeciesMaterial
var velocity = Vector3(0,0,0)
var GRAV = 9.81

var Speed = 200

var RotateTimer

var StraightTimer
var FallingTimer

var RotationRNG = RandomNumberGenerator.new()

var TimerRNG = RandomNumberGenerator.new()

var RotateDirectionRNG = RandomNumberGenerator.new()

var RoatationRate = 0

var RotateDirection = 1

	
func _ready():
	RotateTimer = $RotateTimer
	StraightTimer = $StraightTimer
	FallingTimer = $FallingTimer
	FallingTimer.one_shot = true
	FallingTimer.start()
	RotateTimer.one_shot = true
	StraightTimer.one_shot = true
	TimerRNG.randomize()
	RotateTimer.wait_time = TimerRNG.randf_range(0.5, 2)
	RotateTimer.start()


func _process(delta):
	if not RotateTimer.is_stopped():
		rotate_y(deg2rad(RoatationRate * RotateDirection * delta))

func _physics_process(delta):
	if FallingTimer.is_stopped():
		var frontVector = to_global($BodyHolder/Head.translation) - to_global($BodyHolder/Body.translation)
		frontVector.y = 0;
		velocity = frontVector.normalized() * Speed * delta;
	else:
		 velocity.y -= GRAV * delta
	move_and_slide(velocity, Vector3.UP);


	

func SetMaterial(material):
	SpeciesMaterial = material;
	$BodyHolder/Body.set_surface_material(0, material)
	$BodyHolder/Head.set_surface_material(0, material)


func _on_RotateTimer_timeout():
	TimerRNG.randomize()
	StraightTimer.wait_time = TimerRNG.randf_range(0.5, 2)
	StraightTimer.start()


func _on_StraightTimer_timeout():
	RotationRNG.randomize();
	RoatationRate = RotationRNG.randf_range(20, 200);
	RotateDirectionRNG.randomize()
	var generatedRotateValue = RotateDirectionRNG.randf_range(-1, 1);
	if generatedRotateValue > 0:
		RotateDirection = 1;
	else:
		 RotateDirection = -1
	TimerRNG.randomize();
	RotateTimer.wait_time = TimerRNG.randf_range(0.5, 2)
	RotateTimer.start();
