class_name MainMenu
extends Control



@onready var start_button = $MarginContainer/HBoxContainer/VBoxContainer/StartBtn as Button		#defines start button as button
@onready var quit_button =$MarginContainer/HBoxContainer/VBoxContainer/QuitBtn as Button		#defines quit button as button



func _ready():
	start_button.button_down.connect(on_start_pressed)		#connects start button with start function
	quit_button.button_down.connect(on_quit_pressed)		#connects quit button with quit function
	
	
func on_start_pressed() -> void:
	get_tree().change_scene_to_packed(Global.load_level)		#starts game
	
func on_quit_pressed() -> void:
	get_tree().quit()		#quits game
