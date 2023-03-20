using System;

namespace PioniereVonNeuropaLibrary;

public class Game{
	/// <remarks>
	/// Only for JSONSerilization
	/// </remarks>
	public Game() {
		Tiles = Array.Empty<Tile>();
		Nodes = Array.Empty<Node>();
		Roads = Array.Empty<Road>();
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
}