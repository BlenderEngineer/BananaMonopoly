extends Node


var itemData = {}
var playerCount = 2

var data_file_path = "res://Data/Board.json"

func _ready():
	itemData = loadJsonFile(data_file_path)

func loadJsonFile(filePath : String = data_file_path):
	if FileAccess.file_exists(filePath):
		var dataFile = FileAccess.open(filePath, FileAccess.READ)
		var parsedResult = JSON.parse_string(dataFile.get_as_text())
		
		if parsedResult is Dictionary:
			print("JSON data loaded successfully.")
			$"../Game/BoardBackground"
			var itemArray = []
			for key in parsedResult:
				itemArray.append(parsedResult[key])
				var v = parsedResult[key]
#				print (v.name.eng)
#				print (v.description.eng)
#				print (v.price)
#				print (Color.hex(int(v.color)))
			
			return itemArray
		else:
			print("Error reading JSON file")
	else:
		print("File doesn't exist")
