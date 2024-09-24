extends Node

var coins = 0 # Use Global.coins to access from any script
signal coin_added

var main_level = preload("res://Sohan/Level1.tscn") as PackedScene


# This func will fire whenever coin is picked up, and emit coin_added
# Use Global.connect("coin_added", self.NEWFUNCNAME) in UI script
# to trigger NEWFUNCNAME when signal coin_added emitted
func add_coin(value):
	coins += value
	emit_signal("coin_added")
