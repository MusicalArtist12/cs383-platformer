# Jason Culbertson
extends Node2D

@export var yVelocity: int = 100
@onready var animplayer = $AnimatedSprite2D


func _on_area_2d_body_entered(body: Node2D) -> void:
	animplayer.play("launch")

	body.velocity.y = -yVelocity*10
