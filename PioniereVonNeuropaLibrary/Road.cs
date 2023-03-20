using System;

namespace PioniereVonNeuropaLibrary;

public class Road{
	private Road() {
		Nodes  = Array.Empty<Node>();
		player = 0;
	}

	public Road(int ID) : this() {
		this.ID = ID;
	}

	public int    ID     { get; set; }
	public Node[] Nodes  { get; set; }
	public int    player { get; set; }
}