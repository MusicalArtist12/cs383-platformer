extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass



func _on_area_2d_body_entered(body:Node2D) -> void:
	print(body)
	var win_screen = $CanvasLayer/Win_screen
	print("here");
	win_screen.score_text = str(Global.coins)
	win_screen.visible = true;	
	
	