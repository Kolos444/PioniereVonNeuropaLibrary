using System;

namespace PioniereVonNeuropaLibrary;

public class Tile{
	private Tile() {
		Neighbours = new int[6];
		Nodes      = new int[6];
		Roads      = new int[6];
	}

	public Tile(int ID) : this() {
		this.ID = ID;
	}

	public int      ID       { get; set; }
	public Resource Resource { get; set; }
	public int      Value    { get; set; }
	public bool     Harbour  { get; set; }
	public bool     Land  { get; set; }

	public int[] Neighbours { get; set; }
	public int[] Nodes      { get; set; }
	public int[] Roads      { get; set; }
}