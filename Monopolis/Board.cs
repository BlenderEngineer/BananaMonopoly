using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

class Square
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Type { get; set; }
	public int Price { get; set; }
}
class Player
{
	public int ID { get; set; }
	public int Money { get; set; }
	public LinkedListNode<Square> CurrentSquare { get; set; }
	public string[] Property { get; set; }
}

public partial class Board : Node2D
{
	LinkedList<Square> linkedList = new LinkedList<Square>();
	private Queue<Player> playerQueue;
	private int currentTileIndex = 0;
	private int turnNumber = 1;
	private int playerCount = 2;
	private int moneyGoal = 0;
	private Tile[] tiles;
	
	public override void _Ready()
	{
		GD.Print("Game init");
	}
	
	private void OnGameDraw() //make sure event is using C# syntax, not gdscript
	{
		StartGame();
		GD.Print("Game prepared.");
		GD.Print("Game is ready");
	}
	
	private void OnRollButtonPressed()
	{
		GD.Print("Rolling Dice");
		
		Random random = new Random();
		
		// Generate a random number between 1 and 6 (inclusive)
		int diceRoll = random.Next(1, 7);

		GD.Print("The dice rolled: " + diceRoll);
		GetNode<Label>("Game/DiceValue").Text = diceRoll.ToString();
		turnNumber++;
		UpdateStats(NextPlayerTurn(diceRoll));
	}
	
	private void OnPlayersButtonPressed()
	{
		if (playerCount == 2) {
			playerCount = 3;
			GetNode<Label>("Game/MoneyLabel/PlayerLabel3").Visible = true;
			GetNode<Sprite2D>("Game/Players/3").Visible = true;
		}
		else {
			playerCount = 2;
			GetNode<Label>("Game/MoneyLabel/PlayerLabel3").Visible = false;
			GetNode<Sprite2D>("Game/Players/3").Visible = false;
		}
	}
	
	private void UpdateStats(string CurrentPlayerName)
	{
		GetNode<Label>("Game/TurnNumberLabel/TurnNumber").Text = turnNumber.ToString();
		GetNode<Label>("Game/CurrentPlayer/CurrentPlayerNumber").Text = CurrentPlayerName;
		GetNode<Label>("Game/MoneyLabel/PlayerLabel" + CurrentPlayerName + "/MoneyNumber").Text = playerQueue.Peek().Money.ToString();
//		GetNode<Label>("Game/PropertyLabel/Property").Text = "WIP";
//		GetNode<Label>("").Text = "WIP";
//		GetNode<Label>("").Text = "WIP";
//		GetNode<Label>("").Text = "WIP";
//		GetNode<Label>("").Text = "WIP";
	}
	private void InitializePlayers()
	{
		// Create and initialize player objects
		playerQueue = new Queue<Player>();
		
		for (int i = 1; i < playerCount+1; i++){
			Player player = new Player();
			player.ID = i;
			player.Money = 200;
			player.CurrentSquare = linkedList.First;
			playerQueue.Enqueue(player);
		}
		currentTileIndex = 0;
		GD.Print("Players enqueued.");
	}

	private void StartGame()
	{
		
		GD.Print("Player count ", playerCount);
		//set money goal
		moneyGoal = GetNode<LineEdit>("Menu/MoneyGoal").Text.ToInt();
		
		var board = GetNode<Node2D>("Board");
		
		int i = 0;
		foreach (var item in board.GetChildren()) //go through nodes
		{
			Square newSquare = new Square();
			newSquare.ID = i;
			newSquare.Name = item.GetNode<Label>("name").Text;
			newSquare.Price = item.GetNode<Label>("price").Text.ToInt();
			newSquare.Type = item.GetNode<Label>("type").Text.ToInt();
			
			linkedList.AddLast(newSquare);
			i++;
		}
		
		InitializePlayers();
		// Begin the game loop
		turnNumber = 1;
		currentTileIndex = 0;
		var SCALE = 50;
		var players = GetNode<Node2D>("Game/Players");
		foreach (Sprite2D player in players.GetChildren())
		{
			string playerName = player.Name;
			GD.Print("Player  ==",player.Position);
			player.Position = new Vector2(0, playerName.ToInt() * 10) * SCALE;
		}
		for(int playerID = 1; playerID < playerCount+1; playerID++){
			UpdateStats(playerID.ToString());
		}
	}

	public string NextPlayerTurn(int diceRoll)
	{
		// Get the current player
		// roll dice
		var player = playerQueue.Peek();
		var currentPlayerName = player.ID.ToString();
		GD.Print(currentPlayerName, " turn");
		
// replace with for loop to go to next
		var SCALE = 50; // because image size
		for(int i = 0; i < diceRoll; i++){
			if (player.CurrentSquare.Next == null) {
				player.CurrentSquare = linkedList.First;
				continue;
			}
			player.CurrentSquare = player.CurrentSquare.Next;
			var SquareNode = GetNode<Node2D>("Board").GetNode<Panel>(player.CurrentSquare.Value.ID.ToString());
			GD.Print(SquareNode.Position);
			GetNode<Sprite2D>("Game/Players/" + player.ID.ToString()).Position = (SquareNode.Position + new Vector2(0, player.ID*10)) * SCALE;
			
		}
		int SquareType = player.CurrentSquare.Value.Type;
		//Different types of squares
		GD.Print(SquareType);
		switch(SquareType)
		{
			case 0:
				player.Money += 200; //start
				break;
			case 1:
				player.Money += player.CurrentSquare.Value.Price; // basic
				break;
			case 2: // banana market
				break;
			case 3: // banana factory
				player.Money -= player.CurrentSquare.Value.Price;
				break;
			case 4: // banana resort
				break;
			case 5: // banana lottery (chance)
				Random random = new Random();
				int RNG = random.Next(-200, 201);
				player.Money += RNG; // banana factory
				break;
			default:
				break;
		}
		GD.Print("New Square = ", player.CurrentSquare.Value.Name);
		for(int playerID = 1; playerID < playerCount+1; playerID++){
			UpdateStats(playerID.ToString());
		}
		//TODO: add player position display
//		currentTileIndex = newPosition;

//		 Check if the player landed on a property
//		var tileType = currentTile.Type;
//		if (tileType == 2)
//		{
//			var owner = currentTile.Owner;
//			var price = currentTile.Price;
//			var rent = currentTile.Rent;
//
//			if (owner == "")
//			{
//				// Tile is unowned, player can buy it
//				GD.Print(currentPlayerName + " landed on an unowned property. Price: " + price);
//				var playerDecision = PromptPlayerToBuyProperty(price);
//				if (playerDecision)
//				{
//					currentTile.Owner = currentPlayerName;
//					GD.Print(currentPlayerName + " bought the property.");
//				}
//			}
//			else if (owner != currentPlayerName)
//			{
//				// Tile is owned by another player, player pays rent
//				GD.Print(currentPlayerName + " landed on a property owned by " + owner + ". Rent: " + rent);
//				PayRent(owner, rent);
//			}
//		}

		// Check if the game is over
		if (CheckGameOver())
		{
			GD.Print("Game over!");
			GD.Print("Player ", currentPlayerName, " wins!");
			
			var WinnerLabel = GetNode<Label>("Winner");
			WinnerLabel.Text = currentPlayerName;
			WinnerLabel.Visible = true;
			return currentPlayerName;
		}
		
		// Move to the next player
		playerQueue.Enqueue(playerQueue.Dequeue());
		currentPlayerName = playerQueue.Peek().ID.ToString();

		return currentPlayerName;
	}
	
	private bool PromptPlayerToBuyProperty(int price)
	{
		return true;
	}

	private void PayRent(string owner, int rent)
	{
		// Same as before
	}

	private bool CheckGameOver()
	{
		return playerQueue.Peek().Money >= moneyGoal;
	}
}

public struct Tile
{
	public string Type;
	public string Owner;
	public int Price;
	public int Rent;
}
