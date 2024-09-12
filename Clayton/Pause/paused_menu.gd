extends Control

#sets value to determine if game is paused
var _is_paused:bool = false:   
	set = set_paused
	
#Detects whether the button is pressed to pause the game	
func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed("pause"):
		_is_paused = !_is_paused		#sets paused status opposite of current status
	
	
#Pauses the game and stops character movement
func set_paused(value:bool) -> void:
	_is_paused = value	
	get_tree().paused = _is_paused		#pauses the game and stops movement
	visible = _is_paused		#sets visibility of pause menu
	
#Functionality for resume button
func _on_resume_btn_pressed() -> void:
	_is_paused = false

#Functionality for settings button
func _on_settings_btn_pressed() -> void:
	pass # Replace with function body.

#Functionality for quit button
func _on_quit_btn_pressed() -> void:
	get_tree().quit()
