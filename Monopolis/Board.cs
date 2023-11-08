using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Node2D
{
	public Queue<string> playerQueue;
	private int currentTileIndex = 0;
	private int turnNumber = 1;
	private Tile[] tiles;
	
	public override void _Ready()
	{
		GD.Print("Game init");
	}
	
	private void OnGameDraw() //make sure event is using C# syntax, not gdscript
	{
		GD.Print("Preparing Game");
		InitializePlayers();
		GD.Print("Players enqueued.");
		StartGame();
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
		UpdateStats();
		GD.Print(NextPlayerTurn(diceRoll));
	}
	
	private void UpdateStats()
	{
		GetNode<Label>("Game/TurnNumberLabel/TurnNumber").Text = turnNumber.ToString();
		GetNode<Label>("Game/CurrentPlayer/CurrentPlayerNumber").Text = "WIP";
		GetNode<Label>("Game/PropertyLabel/Property").Text = "WIP";
		GetNode<Label>("").Text = "WIP";
		GetNode<Label>("").Text = "WIP";
		GetNode<Label>("").Text = "WIP";
		GetNode<Label>("").Text = "WIP";
	}
	private void InitializePlayers()
	{
		// Create and initialize player objects
		playerQueue = new Queue<string>();
		playerQueue.Enqueue("Player 1");
		playerQueue.Enqueue("Player 2");
		playerQueue.Enqueue("Player 3");
		currentTileIndex = 0;
	}

	private void StartGame()
	{
		
		GD.Print("Player count ", GetNode<Button>("%..%/Menu%/PlayersButton").Text);
		// Begin the game loop
		//TO DO: reset board
	}

	public string NextPlayerTurn(int diceRoll)
	{
		// Get the current player
		// roll dice
		// use abstractions to interact with these from game.gd
		var currentPlayerName = playerQueue.Peek();
		GD.Print(currentPlayerName, " turn");
		
		var newPosition = (currentTileIndex + diceRoll) % tiles.Length;
		//currentTileIndex = newPosition;

		// Check if the player landed on a property
		var currentTile = tiles[currentTileIndex];
		var tileType = currentTile.Type;
		if (tileType == "property")
		{
			var owner = currentTile.Owner;
			var price = currentTile.Price;
			var rent = currentTile.Rent;

			if (owner == "")
			{
				// Tile is unowned, player can buy it
				Console.WriteLine(currentPlayerName + " landed on an unowned property. Price: " + price);
				var playerDecision = PromptPlayerToBuyProperty(price);
				if (playerDecision)
				{
					currentTile.Owner = currentPlayerName;
					Console.WriteLine(currentPlayerName + " bought the property.");
				}
			}
			else if (owner != currentPlayerName)
			{
				// Tile is owned by another player, player pays rent
				Console.WriteLine(currentPlayerName + " landed on a property owned by " + owner + ". Rent: " + rent);
				PayRent(owner, rent);
			}
		}

		// Move to the next player
		playerQueue.Enqueue(playerQueue.Dequeue());

		// Check if the game is over
		if (CheckGameOver())
		{
			Console.WriteLine("Game over!");
			return currentPlayerName;
		}
		// Continue to the next player's turn from game.gdscript
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
		
		return false; // Replace with actual condition
	}
}

public struct Tile
{
	public string Type;
	public string Owner;
	public int Price;
	public int Rent;
}
