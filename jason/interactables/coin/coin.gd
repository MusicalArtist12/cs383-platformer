extends Node2D

@export var value = 1

func _on_area_2d_body_entered(body: Node2D) -> void:
	# Global.add_coin(value)
	queue_free()
