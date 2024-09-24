class_name ControlMenu
extends Control


@onready var exit_btn: Button = $GridContainer3/ExitBtn as Button


func _ready():
	exit_btn.button_down.connect(on_exit_pressed)
	

func on_exit_pressed() -> void:
	queue_free();
	# get_tree().change_scene_to_packed(Global.load_level)
	
