class_name ControlMenu
extends Control


@onready var exit_btn: Button = $GridContainer3/ExitBtn as Button


func _ready():
	exit_btn.button_down.connect(on_exit_pressed)
	




func on_exit_pressed() -> void:
	
	get_tree().change_scene_to_file("res://julia/levels/CharacterTest/CharacterTest.tscn")
	
