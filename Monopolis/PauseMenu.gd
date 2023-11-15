extends CanvasGroup

func _on_resume_button_pressed():
	visible = false
	
func _on_main_menu_button_pressed():
	visible = false
	$"../Game".visible = false
	$"../Menu".visible = true
	$"../Winner".visible = false
	
func _on_exit_button_pressed():
	get_tree().quit()


