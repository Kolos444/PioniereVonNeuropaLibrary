using System;

namespace PioniereVonNeuropaLibrary;

public class Node{
	private Node() {
		Tiles = Array.Empty<Tile>();
		Roads = Array.Empty<Road>();
	}

	public Node(int ID) : this() {
		this.ID = ID;
	}

	public int    ID    { get; set; }
	public bool   City  { get; set; }
	public Tile[] Tiles { get; set; }
	public Road[] Roads { get; set; }
}