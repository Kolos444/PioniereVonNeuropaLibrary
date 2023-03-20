using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;

namespace PioniereVonNeuropaLibrary;

public class Game{
	private static Random _random = new(DateTime.Now.Nanosecond);

	/// <remarks>
	/// Only for JSONSerilization
	/// </remarks>
	public Game() {
		Tiles    = Array.Empty<Tile>();
		Nodes    = Array.Empty<Node>();
		Roads    = Array.Empty<Road>();
		Settings = new();
	}

	public Game(int width, int height) {
		Width  = width;
		Height = height;

		Tiles = new Tile[width * height];

		Nodes = new Node[Width * 2 * Height + Height * 2 + Width * 2];

		Roads = new Road[3 * Width * Height + 2 * Height + Width * 2 - 1];

		Settings = new();

		GenerateEmptyBoard();
	}

	//Objects for the Game
	public Tile[] Tiles { get; set; }
	public Node[] Nodes { get; set; }
	public Road[] Roads { get; set; }

	//Info to handle Tiles access
	public int Width  { get; set; }
	public int Height { get; set; }

	//Map settings
	public Settings Settings { get; set; }

	private void GenerateEmptyBoard() {
		int nodeId = 1;
		for (int y = 0; y < Height; y++){
			for (int x = 0; x < Width; x++){
				Tile tile = new(y * Width + x + 1);

				SetNeighbours(tile, x, y);

				CreateNodes(tile, ref nodeId, y, x);

				Tiles[y * Width + x] = tile;
			}
		}

		int roadId = 1;
		for (int y = 0; y < Height; y++){
			for (int x = 0; x < Width; x++){
				Tile tile = Tiles[y * Width + x];

				CreateTileRoads(y, x, ref roadId, tile);
			}
		}
	}

	private void SetNeighbours(Tile tile, int x, int y) {
		if (y % 2 == 0){
			if (y > 0){
				tile.Neighbours[0] = Tiles[(y - 1) * Width + x].ID; //Oben Rechts

				Tiles[(y - 1) * Width + x].Neighbours[3] = tile.ID;

				if (x > 0){
					tile.Neighbours[5] = Tiles[(y - 1) * Width + x - 1].ID; //Oben Links

					Tiles[(y - 1) * Width + x - 1].Neighbours[2] = tile.ID;
				}
			}

			if (x > 0){
				tile.Neighbours[4] = Tiles[y * Width + x - 1].ID; //links

				Tiles[y * Width + x - 1].Neighbours[1] = tile.ID;
			}
		} else{
			if (y > 0){
				if (x < Width - 1){
					tile.Neighbours[0] = Tiles[(y - 1) * Width + x + 1].ID; //Oben Rechts

					Tiles[(y - 1) * Width + x + 1].Neighbours[3] = tile.ID;
				}

				tile.Neighbours[5] = Tiles[(y - 1) * Width + x].ID; //Oben Links

				Tiles[(y - 1) * Width + x].Neighbours[2] = tile.ID;
			}


			if (x > 0){
				tile.Neighbours[4] = Tiles[y * Width + x - 1].ID; //Links

				Tiles[y * Width + x - 1].Neighbours[1] = tile.ID;
			}
		}
	}

	private void CreateNodes(Tile tile, ref int nodeId, int y, int x) {
		if (y % 2 == 0){
			//Wenn es das erste in der Reihe ist
			if (x == 0)
				EvenRowFirstColumnNodes(tile, ref nodeId);

			EvenRowBaseNodes(tile, ref nodeId, y, x);

			//Wenn es die letzte Reihe ist
			if (y == Height - 1)
				EvenRowLastRowNodes(tile, ref nodeId, x);
		} else{
			OddRowBaseNodes(tile, ref nodeId, x);

			//Wenn es das Letzte in der Reihe ist
			if (x == Width - 1){
				OddRowLastColumnNodes(tile, ref nodeId);
			}


			//Wenn es die letzte Reihe ist
			if (y == Height - 1){
				OddRowLastRowNodes(tile, ref nodeId, x);
			}
		}
	}

	private void EvenRowFirstColumnNodes(Tile tile, ref int nodeId) {
		Node nodeSouthWest = new Node(nodeId++);
		tile.Nodes[4] = nodeSouthWest.ID;
		Node nodeNorthWest = new Node(nodeId++);
		tile.Nodes[5] = nodeNorthWest.ID;

		nodeSouthWest.Tiles[0] = tile.ID;
		nodeNorthWest.Tiles[1] = tile.ID;

		Nodes[-1 + nodeSouthWest.ID] = nodeSouthWest;
		Nodes[-1 + nodeNorthWest.ID] = nodeNorthWest;
	}

	private void EvenRowBaseNodes(Tile tile, ref int nodeId, int y, int x) {
		Node nodeNorth = new Node(nodeId++);
		tile.Nodes[0] = nodeNorth.ID;
		Node nodeNorthEast = new Node(nodeId++);
		tile.Nodes[1] = nodeNorthEast.ID;

		nodeNorth.Tiles[1]     = tile.ID;
		nodeNorthEast.Tiles[2] = tile.ID;


		if (x > 0){
			Nodes[-1 + Tiles[tile.ID - 2].Nodes[1]].Tiles[1] = tile.ID;
			tile.Nodes[5]                                    = Tiles[tile.ID - 2].Nodes[1];
		}

		if (y > 0){
			nodeNorthEast.Tiles[0] = Tiles[tile.ID - Width - 1].ID; //Oben

			if (x < Width - 1){
				nodeNorth.Tiles[0] = Tiles[tile.ID - Width - 1].ID; //Oben Rechts
			}

			if (x > 0){
				nodeNorth.Tiles[2] = Tiles[tile.ID - Width - 2].ID; //Oben links
			}

			Tiles[tile.ID - Width - 1].Nodes[3] = nodeNorthEast.ID;
			Tiles[tile.ID - Width - 1].Nodes[4] = nodeNorth.ID;

			Tiles[tile.ID - Width - 2].Nodes[2] = nodeNorth.ID;
		}

		Nodes[-1 + nodeNorth.ID]     = nodeNorth;
		Nodes[-1 + nodeNorthEast.ID] = nodeNorthEast;
	}

	private void EvenRowLastRowNodes(Tile tile, ref int nodeId, int x) {
		Node nodeSouth = new Node(nodeId++);
		tile.Nodes[3] = nodeSouth.ID;
		Node nodeSouthEast = new Node(nodeId++);
		tile.Nodes[2] = nodeSouthEast.ID;

		nodeSouth.Tiles[0]     = tile.ID;
		nodeSouthEast.Tiles[2] = tile.ID;

		//Wenn es nicht das letzte in der Reihe ist
		if (x > 0)
			tile.Nodes[4] = Tiles[tile.ID - 2].Nodes[2];

		Nodes[-1 + nodeSouth.ID]     = nodeSouth;
		Nodes[-1 + nodeSouthEast.ID] = nodeSouthEast;
	}

	private void OddRowBaseNodes(Tile tile, ref int nodeId, int x) {
		Node nodeNorthWest = new Node(nodeId++);
		tile.Nodes[5] = nodeNorthWest.ID;
		Node nodeNorth = new Node(nodeId++);
		tile.Nodes[0] = nodeNorth.ID;


		nodeNorth.Tiles[1]     = tile.ID; //Unten
		nodeNorthWest.Tiles[1] = tile.ID; //Unten Links

		tile.Nodes[5] = nodeNorthWest.ID;


		Tiles[tile.ID - Width - 1].Nodes[2] = nodeNorth.ID;
		Tiles[tile.ID - Width - 1].Nodes[3] = nodeNorthWest.ID;
		if (x < Width - 1)
			Tiles[tile.ID - Width].Nodes[4] = nodeNorth.ID;

		if (x > 0){
			nodeNorthWest.Tiles[2]      = tile.ID - 1; //Unten links
			Tiles[tile.ID - 2].Nodes[1] = nodeNorthWest.ID;
		}

		if (x < Width - 1){
			nodeNorth.Tiles[0] = Tiles[tile.ID - Width].ID; //Oben rechts
		}


		nodeNorth.Tiles[2]     = Tiles[tile.ID - Width - 1].ID; //Oben links
		nodeNorthWest.Tiles[0] = Tiles[tile.ID - Width - 1].ID; //Oben

		Nodes[-1 + nodeNorth.ID]     = nodeNorth;
		Nodes[-1 + nodeNorthWest.ID] = nodeNorthWest;
	}

	private void OddRowLastColumnNodes(Tile tile, ref int nodeId) {
		Node nodeSouthEast = new Node(nodeId++);
		tile.Nodes[1] = nodeSouthEast.ID;
		Node nodeNorthEast = new Node(nodeId++);
		tile.Nodes[2] = nodeNorthEast.ID;

		nodeSouthEast.Tiles[2] = tile.ID;
		nodeNorthEast.Tiles[2] = tile.ID;

		Nodes[-1 + nodeSouthEast.ID] = nodeSouthEast;
		Nodes[-1 + nodeNorthEast.ID] = nodeNorthEast;
	}

	private void OddRowLastRowNodes(Tile tile, ref int nodeId, int x) {
		Node nodeSouthWest = new Node(nodeId++);
		tile.Nodes[4] = nodeSouthWest.ID;
		Node nodeSouth = new Node(nodeId++);
		tile.Nodes[3] = nodeSouth.ID;

		nodeSouthWest.Tiles[0] = tile.ID;
		nodeSouth.Tiles[0]     = tile.ID;

		//Wenn es nicht das letzte in der Reihe ist
		if (x > 0){
			nodeSouthWest.Tiles[2]      = tile.ID - 1;
			Tiles[tile.ID - 2].Nodes[2] = nodeSouthWest.ID;
		}

		Nodes[-1 + nodeSouthWest.ID] = nodeSouthWest;
		Nodes[-1 + nodeSouth.ID]     = nodeSouth;
	}


	private void CreateTileRoads(int y, int x, ref int roadId, Tile tile) {
		//Wenn es eine gerade Reihe ist (start ist 0)
		if (y % 2 == 0){
			//Wenn es die erste Reihe ist
			if (x == 0)
				EvenRowFirstColumnRoads(ref roadId, tile);

			OddRowBaseRoads(y, x, ref roadId, tile);

			if (y == Height - 1)
				EvenRowLastRowRoads(x, ref roadId, tile);
		} else{
			OddRowBaseRoads(x, ref roadId, tile);

			if (x == Width - 1)
				OddRowLastColumnRoads(ref roadId, tile);

			if (y == Height - 1)
				OddRowLastRowRoads(x, ref roadId, tile);
		}
	}

	private void OddRowLastRowRoads(int x, ref int roadId, Tile tile) {
		Road southWest = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[4],
				[1] = tile.Nodes[3]
			}
		};
		Nodes[-1 + tile.Nodes[4]].Roads[1] = southWest.ID;
		Nodes[-1 + tile.Nodes[3]].Roads[2] = southWest.ID;

		tile.Roads[3] = southWest.ID;

		Roads[-1 + southWest.ID] = southWest;

		//Wenn es nicht das Letzte in der Reihe ist
		if (x < Width - 1){
			Road southEast = new Road(roadId++) {
				Nodes = {
					[0] = tile.Nodes[2],
					[1] = tile.Nodes[3]
				}
			};
			Nodes[-1 + tile.Nodes[2]].Roads[2] = southWest.ID;
			Nodes[-1 + tile.Nodes[3]].Roads[0] = southWest.ID;

			tile.Roads[2] = southEast.ID;

			Roads[-1 + southEast.ID] = southEast;
		}
	}

	private void OddRowLastColumnRoads(ref int roadId, Tile tile) {
		Road east = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[1],
				[1] = tile.Nodes[2]
			}
		};
		Nodes[-1 + tile.Nodes[1]].Roads[1] = east.ID;
		Nodes[-1 + tile.Nodes[2]].Roads[0] = east.ID;
		Road southEast = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[2],
				[1] = tile.Nodes[3]
			}
		};
		Nodes[-1 + tile.Nodes[2]].Roads[2] = southEast.ID;
		Nodes[-1 + tile.Nodes[3]].Roads[0] = southEast.ID;

		tile.Roads[1] = east.ID;
		tile.Roads[2] = southEast.ID;

		Roads[-1 + east.ID]      = east;
		Roads[-1 + southEast.ID] = southEast;
	}

	private void OddRowBaseRoads(int x, ref int roadId, Tile tile) {
		Road west = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[5],
				[1] = tile.Nodes[4]
			}
		};
		Nodes[-1 + tile.Nodes[5]].Roads[1] = west.ID;
		Nodes[-1 + tile.Nodes[4]].Roads[0] = west.ID;
		Road northWest = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[0],
				[1] = tile.Nodes[5]
			}
		};
		Nodes[-1 + tile.Nodes[0]].Roads[2] = northWest.ID;
		Nodes[-1 + tile.Nodes[5]].Roads[0] = northWest.ID;
		Road northEast = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[0],
				[1] = tile.Nodes[1]
			}
		};
		Nodes[-1 + tile.Nodes[0]].Roads[1] = northEast.ID;
		Nodes[-1 + tile.Nodes[1]].Roads[2] = northEast.ID;

		tile.Roads[4] = west.ID;
		tile.Roads[5] = northWest.ID;
		tile.Roads[0] = northEast.ID;
		tile.Roads[1] = northEast.ID + 1; //Spezialfall da wir dies immer definitiv wissen

		Roads[-1 + west.ID]      = west;
		Roads[-1 + northWest.ID] = northWest;
		Roads[-1 + northEast.ID] = northEast;


		Tiles[tile.ID - Width - 1].Roads[2] = northWest.ID;

		//Wenn es nicht das letzte in der Reihe ist
		if (x < Width - 1)
			Tiles[tile.ID - Width].Roads[3] = northEast.ID;
	}

	private void EvenRowLastRowRoads(int x, ref int roadId, Tile tile) { //Wenn es nicht das Erste in der Reihe ist
		if (x > 0){
			Road southWest = new Road(roadId++) {
				Nodes = {
					[0] = tile.Nodes[4],
					[1] = tile.Nodes[3]
				}
			};
			Nodes[-1 + tile.Nodes[4]].Roads[1] = southWest.ID;
			Nodes[-1 + tile.Nodes[3]].Roads[2] = southWest.ID;

			tile.Roads[3] = southWest.ID;

			Roads[-1 + southWest.ID] = southWest;
		}

		Road southEast = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[2],
				[1] = tile.Nodes[3]
			}
		};
		Nodes[-1 + tile.Nodes[2]].Roads[2] = southEast.ID;
		Nodes[-1 + tile.Nodes[3]].Roads[0] = southEast.ID;

		tile.Roads[2] = southEast.ID;

		Roads[-1 + southEast.ID] = southEast;
	}

	private void OddRowBaseRoads(int y, int x, ref int roadId, Tile tile) {
		Road northWest = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[0],
				[1] = tile.Nodes[5]
			}
		};
		Nodes[-1 + tile.Nodes[0]].Roads[2] = northWest.ID;
		Nodes[-1 + tile.Nodes[5]].Roads[0] = northWest.ID;
		Road northEast = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[0],
				[1] = tile.Nodes[1]
			}
		};
		Nodes[-1 + tile.Nodes[0]].Roads[1] = northEast.ID;
		Nodes[-1 + tile.Nodes[1]].Roads[2] = northEast.ID;
		Road east = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[1],
				[1] = tile.Nodes[2]
			}
		};
		Nodes[-1 + tile.Nodes[1]].Roads[2] = east.ID;
		Nodes[-1 + tile.Nodes[2]].Roads[0] = east.ID;

		tile.Roads[5] = northWest.ID;
		tile.Roads[0] = northEast.ID;
		tile.Roads[1] = east.ID;

		Roads[-1 + northWest.ID] = northWest;
		Roads[-1 + northEast.ID] = northEast;
		Roads[-1 + east.ID]      = east;

		//Wenn es nicht die erste Reihe ist
		if (x > 0){
			tile.Roads[4] = northWest.ID - 1;
		}


		//Wenn es die Erste Reihe ist wird es geskippt
		if (y == 0)
			return;

		Tiles[tile.ID - Width - 1].Roads[3] = northEast.ID;

		//Wenn es nicht die Erste Reihe ist
		if (x > 0){
			Tiles[tile.ID - Width - 2].Roads[2] = northWest.ID;
		}
	}

	private void EvenRowFirstColumnRoads(ref int roadId, Tile tile) {
		Road southWest = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[4],
				[1] = tile.Nodes[3]
			}
		};

		Nodes[-1 + tile.Nodes[4]].Roads[1] = southWest.ID;
		Nodes[-1 + tile.Nodes[3]].Roads[2] = southWest.ID;

		Road west = new Road(roadId++) {
			Nodes = {
				[0] = tile.Nodes[5],
				[1] = tile.Nodes[4]
			}
		};

		Nodes[-1 + tile.Nodes[5]].Roads[1] = west.ID;
		Nodes[-1 + tile.Nodes[3]].Roads[0] = west.ID;

		tile.Roads[3] = southWest.ID;
		tile.Roads[4] = west.ID;


		Roads[-1 + southWest.ID] = southWest;
		Roads[-1 + west.ID]      = west;
	}

	public static void MakePlayable(Game game) {
		Settings logicSettings = new(game.Settings);

		foreach (Tile tile in game.Tiles){
			if (tile is { Land: true, Resource: Resource.None }){
				tile.Resource = GetRandomResource(logicSettings);
				tile.Value    = GetRandomValue(logicSettings.DiceValues);
			} else if (tile is { Harbour: true, Resource: Resource.None }){
				if (HasGeneratedNeighbourHarbour(tile, game))
					MakeWaterTile(tile);
				else if (game.Settings.HarbourSettings.Harbours >= 0){
					tile.Resource = GetRandomHarbourResource(logicSettings.HarbourSettings);
					MakeHarbourConnections(tile, game);
				} else{
					MakeWaterTile(tile);
				}
			}
		}
	}

	private static void MakeHarbourConnections(Tile tile, Game game) {
		foreach (int tileNeighbour in tile.Neighbours){
			if (tileNeighbour == 0)
				continue;

			if (!game.Tiles[tileNeighbour].Land)
				continue;

			List<int> connections = new List<int>(2);
			foreach (int node in game.Tiles[tileNeighbour].Nodes)
				foreach (int tileNode in tile.Nodes)
					if (node == tileNode)
						connections.Add(node);

			for (int index = 0; index < tile.Nodes.Length; index++)
				if (!connections.Contains(tile.Nodes[index]))
					tile.Nodes[index] = 0;
		}
	}

	public static void MakeWaterTile(Tile tile) {
		tile.Resource = Resource.None;
		tile.Land     = false;
		tile.Harbour  = false;
		tile.Value    = 0;
	}

	public static void MakeHarbourTile(Tile tile, Resource resource = Resource.None) {
		tile.Resource = resource;
		tile.Land     = false;
		tile.Harbour  = true;
		tile.Value    = 0;
	}

	public static void MakeLandTile(Tile tile, Resource resource = Resource.None) {
		tile.Resource = resource;
		tile.Land     = true;
		tile.Harbour  = false;
	}

	private static bool HasGeneratedNeighbourHarbour(Tile tile, Game game) {
		foreach (int tileNeighbour in tile.Neighbours){
			if (tileNeighbour == 0){
				continue;
			}

			if (game.Tiles[tileNeighbour - 1].Harbour && game.Tiles[tileNeighbour - 1].ID < tile.ID){
				return true;
			}
		}

		return false;
	}

	private static Resource GetRandomResource(Settings settings) {
		if (settings.Brick + settings.Wood + settings.Wheat + settings.Sheep +
		    settings.Ore   + settings.Deserts == 0)
			settings = new() { Brick = 1, Ore = 1, Wood = 1, Wheat = 1, Sheep = 1 };

		int next = _random.Next(settings.Brick + settings.Wood + settings.Wheat + settings.Sheep +
								settings.Ore   + settings.Deserts);

		if (next < settings.Brick){
			settings.Brick--;
			return Resource.Brick;
		}

		if (next < settings.Brick + settings.Wood){
			settings.Wood--;
			return Resource.Wood;
		}

		if (next < settings.Brick + settings.Wood + settings.Wheat){
			settings.Wheat--;
			return Resource.Wheat;
		}

		if (next < settings.Brick + settings.Wood + settings.Wheat + settings.Sheep){
			settings.Sheep--;
			return Resource.Sheep;
		}

		if (next < settings.Brick + settings.Wood + settings.Wheat + settings.Sheep +
		    settings.Ore){
			settings.Ore--;
			return Resource.Ore;
		}

		settings.Deserts--;
		return Resource.Desert;
	}

	private static int GetRandomValue(DiceValues diceValue) {
		if (diceValue.Two    + diceValue.Three + diceValue.Four  + diceValue.Five +
		    diceValue.Six    + diceValue.Seven + diceValue.Eight + diceValue.Nine + diceValue.Ten +
		    diceValue.Eleven + diceValue.Twelve == 0){
			diceValue = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
		}

		int next = _random.Next(diceValue.Two    + diceValue.Three + diceValue.Four  + diceValue.Five +
								diceValue.Six    + diceValue.Seven + diceValue.Eight + diceValue.Nine + diceValue.Ten +
								diceValue.Eleven + diceValue.Twelve
		);


		if (next < diceValue.Two){
			diceValue.Two--;
			return 2;
		}

		if (next < diceValue.Two + diceValue.Three){
			diceValue.Three--;
			return 3;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four){
			diceValue.Four--;
			return 4;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four + diceValue.Five){
			diceValue.Five--;
			return 5;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four + diceValue.Five +
		    diceValue.Six){
			diceValue.Six--;
			return 6;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four + diceValue.Five +
		    diceValue.Six        + diceValue.Seven){
			diceValue.Seven--;
			return 7;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four + diceValue.Five +
		    diceValue.Six        + diceValue.Seven + diceValue.Eight){
			diceValue.Eight--;
			return 8;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four  + diceValue.Five +
		    diceValue.Six        + diceValue.Seven + diceValue.Eight + diceValue.Nine){
			diceValue.Nine--;
			return 9;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four  + diceValue.Five +
		    diceValue.Six        + diceValue.Seven + diceValue.Eight + diceValue.Nine + diceValue.Ten){
			diceValue.Ten--;
			return 10;
		}

		if (next < diceValue.Two + diceValue.Three + diceValue.Four + diceValue.Five +
		    diceValue.Six + diceValue.Seven + diceValue.Eight + diceValue.Nine + diceValue.Ten + diceValue.Eleven){
			diceValue.Eleven--;
			return 11;
		}

		diceValue.Twelve--;
		return 12;
	}

	private static Resource GetRandomHarbourResource(HarbourSettings settings) {
		int next = _random.Next(settings.Brick + settings.Wood + settings.Wheat + settings.Sheep +
								settings.Ore);
		if (next < settings.Brick){
			settings.Brick--;
			settings.Harbours--;

			return Resource.Brick;
		}

		if (next < settings.Brick + settings.Wood){
			settings.Wood--;
			settings.Harbours--;

			return Resource.Wood;
		}

		if (next < settings.Brick + settings.Wood + settings.Wheat){
			settings.Wheat--;
			settings.Harbours--;

			return Resource.Wheat;
		}

		if (next < settings.Brick + settings.Wood + settings.Wheat + settings.Sheep){
			settings.Sheep--;
			settings.Harbours--;

			return Resource.Sheep;
		}

		if (next < settings.Brick + settings.Wood + settings.Wheat + settings.Sheep +
		    settings.Ore){
			settings.Ore--;
			settings.Harbours--;

			return Resource.Ore;
		}

		settings.Harbours--;
		return Resource.Desert;
	}
}