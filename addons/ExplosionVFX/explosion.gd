class_name Explosion
extends Node3D
## Explosion VFX node - mesh-based explosion visible at any distance.
##
## Usage:
##   var explosion = Explosion.create_at(global_position)
##   get_tree().current_scene.add_child(explosion)

@export var auto_start: bool = true
@export var auto_free: bool = true
@export var scale_multiplier: float = 1.0
@export var explosion_duration: float = 0.4
@export var flash_color: Color = Color(1.0, 0.7, 0.3, 1.0)
@export var smoke_color: Color = Color(0.3, 0.25, 0.2, 0.8)

var _time: float = 0.0
var _is_exploding: bool = false

@onready var flash_sphere: MeshInstance3D = $FlashSphere
@onready var core_sphere: MeshInstance3D = $CoreSphere
@onready var smoke_sphere: MeshInstance3D = $SmokeSphere
@onready var explosion_light: OmniLight3D = $ExplosionLight
@onready var cleanup_timer: Timer = $CleanupTimer


func _ready() -> void:
	cleanup_timer.timeout.connect(_on_cleanup_timeout)

	# Start hidden
	_set_all_visible(false)

	if scale_multiplier != 1.0:
		scale = Vector3.ONE * scale_multiplier

	if auto_start:
		explode()


func _process(delta: float) -> void:
	if not _is_exploding:
		return

	_time += delta
	var t: float = _time / explosion_duration

	if t >= 1.0:
		_is_exploding = false
		_set_all_visible(false)
		return

	# Flash sphere: quick expand and fade (first 30% of animation)
	var flash_t: float = clamp(t / 0.3, 0.0, 1.0)
	var flash_scale: float = lerp(0.5, 3.0, ease(flash_t, -2.0))
	var flash_alpha: float = lerp(1.0, 0.0, ease(flash_t, 2.0))
	flash_sphere.scale = Vector3.ONE * flash_scale
	var flash_mat: StandardMaterial3D = flash_sphere.get_surface_override_material(0)
	if flash_mat:
		flash_mat.albedo_color.a = flash_alpha
		flash_mat.emission_energy_multiplier = lerp(8.0, 0.0, flash_t)

	# Core sphere: expand and fade
	var core_scale: float = lerp(0.3, 2.5, ease(t, -1.5))
	var core_alpha: float = lerp(1.0, 0.0, ease(t, 1.5))
	core_sphere.scale = Vector3.ONE * core_scale
	var core_mat: StandardMaterial3D = core_sphere.get_surface_override_material(0)
	if core_mat:
		core_mat.albedo_color.a = core_alpha
		core_mat.emission_energy_multiplier = lerp(5.0, 0.0, t)

	# Smoke sphere: slower expand and fade
	var smoke_t: float = clamp((t - 0.1) / 0.9, 0.0, 1.0)
	var smoke_scale: float = lerp(0.5, 4.0, ease(smoke_t, -1.0))
	var smoke_alpha: float = lerp(0.6, 0.0, ease(smoke_t, 1.0))
	smoke_sphere.scale = Vector3.ONE * smoke_scale
	smoke_sphere.position.y = lerp(0.0, 1.5, smoke_t)
	var smoke_mat: StandardMaterial3D = smoke_sphere.get_surface_override_material(0)
	if smoke_mat:
		smoke_mat.albedo_color.a = smoke_alpha

	# Light: bright flash then fade
	var light_intensity: float = lerp(8.0, 0.0, ease(t, 2.0))
	explosion_light.light_energy = light_intensity
	explosion_light.omni_range = lerp(15.0, 5.0, t)


func explode() -> void:
	_is_exploding = true
	_time = 0.0
	_set_all_visible(true)

	if auto_free:
		cleanup_timer.start()


func _set_all_visible(visible: bool) -> void:
	flash_sphere.visible = visible
	core_sphere.visible = visible
	smoke_sphere.visible = visible
	explosion_light.visible = visible


func _on_cleanup_timeout() -> void:
	queue_free()


## Static helper to create an explosion at a position
static func create_at(pos: Vector3, explosion_scale: float = 1.0) -> Explosion:
	var scene := preload("res://addons/ExplosionVFX/explosion.tscn")
	var instance: Explosion = scene.instantiate()
	instance.global_position = pos
	instance.scale_multiplier = explosion_scale
	return instance
