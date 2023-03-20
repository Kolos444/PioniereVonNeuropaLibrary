using System;

namespace PioniereVonNeuropaLibrary;

public class Road{
	private Road() {
		Nodes  = new int[2];
		Player = 0;
	}

	public Road(int ID) : this() {
		this.ID = ID;
	}

	public int ID { get; set; }
	public int[] Nodes  { get; set; }
	public int    Player { get; set; }
}