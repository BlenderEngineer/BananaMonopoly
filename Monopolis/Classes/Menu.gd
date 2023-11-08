extends CanvasGroup

func _on_start_button_pressed():
	visible = false
	$"../Game".visible = true

func _on_open_button_pressed():
	OS.shell_open(ProjectSettings.globalize_path("res://Data/Board.json"))

func _on_advanced_button_pressed():
	$AdvancedButton.visible = false
	$AddData.visible = true
	$AddPos.visible = true
	$AddNode.visible = true
	$RemovePos.visible = true
	$RemoveButton.visible = true


func _on_language_button_pressed():
	if TranslationServer.get_locale() == "en_US":
		TranslationServer.set_locale("lt")
	else:
		TranslationServer.set_locale("en_US")
	$"../Background".texture = load("res://Backgrounds/Background_"+TranslationServer.get_locale()+".png")
	
	print(TranslationServer.get_locale())


func _on_players_button_pressed():
	if 	$PlayersButton.text == "PLAYERS2":
		$PlayersButton.text = "PLAYERS3"
		StaticData.playerCount = 3
	else:
		$PlayersButton.text = "PLAYERS2"
		StaticData.playerCount = 2


func _on_clear_button_pressed():
	StaticData.itemData = []


func _on_reload_button_pressed():
	StaticData.itemData = StaticData.loadJsonFile()


func _on_remove_button_pressed():
	var input = get_node("RemovePos")
	var ID = int(input.text)
	if ID == 0 and input.text != "0":
		for v in StaticData.itemData:
			for localName in v.name.keys():
				if v.name[localName] == input.text:
					StaticData.itemData.erase(v)
					return
		print("Item not found...")
	else:
		StaticData.itemData.remove_at(ID)


func _on_add_node_pressed():
	var inputData = get_node("AddData")
	var inputID = int(get_node("AddPos").text)
	
	if inputData.text == "Anglis":
		$"../BackgroundAnglis".play()
		$"../BackgroundMusic".stop()
		return
	
	if len(StaticData.itemData) >= 16:
		print("Board cannot have more than 16 squares...")
		return
	var nodeData = JSON.parse_string(inputData.text)
	StaticData.itemData.insert(inputID, nodeData)
