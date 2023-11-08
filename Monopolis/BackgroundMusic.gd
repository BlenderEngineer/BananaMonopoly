extends AudioStreamPlayer2D

func _on_finished():
	play()


func _on_background_anglis_finished():
	$"../BackgroundAnglis".play()
