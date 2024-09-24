extends Control


@onready var reset_button = $VBoxContainer2/Reset as Button		#defines start button as button
@onready var quit_button =$VBoxContainer2/Quit as Button		#defines quit button as button

func _ready():
	reset_button.button_down.connect(on_reset_pressed)		#connects start button with start function
	quit_button.button_down.connect(on_quit_pressed)		#connects quit button with quit function
	
	
func on_reset_pressed() -> void:
	get_tree().change_scene_to_packed(Global.main_menu)

func on_quit_pressed() -> void:
	get_tree().quit()		#quits game
