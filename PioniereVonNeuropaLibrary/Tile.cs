using System;

namespace PioniereVonNeuropaLibrary;

public class Tile{
	private Tile() {
		Neighbours = Array.Empty<Tile>();
		Roads      = Array.Empty<Road>();
		Nodes      = Array.Empty<Node>();
	}

	public Tile(int ID) : this() {
		this.ID = ID;
	}

	public int      ID       { get; set; }
	public Resource Resource { get; set; }
	public int      Value    { get; set; }
	public bool     Harbour  { get; set; }

	public Tile[] Neighbours { get; set; }
	public Node[] Nodes      { get; set; }
	public Road[] Roads      { get; set; }
}