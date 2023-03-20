using System;

namespace PioniereVonNeuropaLibrary;

public class Node{
	private Node() {
		Tiles = new int[3];
		Roads = new int[3];
	}

	public Node(int ID) : this() {
		this.ID = ID;
	}

	public int   ID    { get; set; }
	public int   Player    { get; set; }
	public bool  City  { get; set; }
	public int[] Tiles { get; set; }
	public int[] Roads { get; set; }
}