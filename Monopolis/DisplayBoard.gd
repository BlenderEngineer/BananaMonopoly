extends Node2D

@onready var panel_scene = preload("res://SquareTemplate.tscn")

var localeMapping = {
	"lt": "ltu",
	"en_US": "eng"
}

var typeMapping = [
	"Start",
	"Property",
	"Property",
	"Jail",
	"Company",
	"Free",
	"Chance"
]
@onready var Board = get_node("../Board")

func DrawCell(position, ID, name, description, color, type, price = null, rent = null):
	var Cell = panel_scene.instantiate()
	Cell.position = position
#	print(position)
#	print(name)
#	print(description)
#	print(color)
#	print(type)
#	print(price)
#	print(rent)
	Cell.name = str(ID)
	Cell.modulate = Color.html(color)
	Cell.get_node("name").text = name
	Cell.tooltip_text = description
	Cell.get_node("type").text = str(type)
	if price > 0:
		Cell.get_node("price").text = str(price)
	else:
		Cell.get_node("price").visible = false
		Cell.get_node("currency").visible = false
	Board.add_child(Cell)

func RenderBoard(ItemData):
	var PANEL_COUNT = len(ItemData)
	var DELAY = 2/(PANEL_COUNT+1)
	var num_per_side = floor(sqrt(PANEL_COUNT))
	if PANEL_COUNT > 12:
		num_per_side = 4
	
	var padding = 5
	
	var total_size = Vector2(80, 80) + Vector2(padding, padding)
	var max_size = num_per_side*total_size.x
	
	var locale = localeMapping[TranslationServer.get_locale()]
	
	#clear previous cells
	for i in range(Board.get_child_count()):
		Board.remove_child(Board.get_child(0))
	
	# draw rectangle of cells
	var count = 0
	var data = null
	for i in range(num_per_side+1):
		if count >= PANEL_COUNT: break
		
		data = ItemData[count]
		
		DrawCell(Vector2((i) * total_size.x, 0),
		 count,
		 data["name"][locale],
		 data["description"][locale],
		 data["color"],
		 data["type"],
		 data["price"],
		 data["rent"],
		) #top
		count += 1
		#await get_tree().create_timer(DELAY).timeout
	for i in range(num_per_side):
		if count >= PANEL_COUNT: break
		data = ItemData[count]
		
		DrawCell(Vector2(max_size, (i+1) * total_size.y),
		 count,
		 data["name"][locale],
		 data["description"][locale],
		 data["color"],
		 data["type"],
		 data["price"],
		 data["rent"],
		) #right
		count += 1
		#await get_tree().create_timer(DELAY).timeout
	for i in range(num_per_side-1):
		if count >= PANEL_COUNT: break
		data = ItemData[count]
		
		DrawCell(Vector2((num_per_side-i-1) * total_size.x, max_size),
		 count,
		 data["name"][locale],
		 data["description"][locale],
		 data["color"],
		 data["type"],
		 data["price"],
		 data["rent"],
		) #bottom
		count += 1
		#await get_tree().create_timer(DELAY).timeout
	for i in range(num_per_side):
		if count >= PANEL_COUNT: break
		data = ItemData[count]
		
		DrawCell(Vector2(0, max_size-1 - (i) * total_size.x),
		 count,
		 data["name"][locale],
		 data["description"][locale],
		 data["color"],
		 data["type"],
		 data["price"],
		 data["rent"],
		) #left
		count += 1
		#await get_tree().create_timer(DELAY).timeout

func _on_game_draw():
	RenderBoard(StaticData.itemData)


func _ready():
	RenderBoard(StaticData.itemData)


func _on_language_button_pressed():
	await get_tree().create_timer(.1).timeout
	RenderBoard(StaticData.itemData)


func _on_clear_button_pressed():
	RenderBoard(StaticData.itemData)


func _on_reload_button_pressed():
	RenderBoard(StaticData.itemData)


func _on_remove_button_pressed():
	RenderBoard(StaticData.itemData)


func _on_add_node_pressed():
	RenderBoard(StaticData.itemData)
