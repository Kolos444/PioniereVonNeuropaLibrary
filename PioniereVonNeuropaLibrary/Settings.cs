namespace PioniereVonNeuropaLibrary;

public class Settings{
	public int             Players         { get; set; }
	public int             Deserts         { get; set; }
	public HarbourSettings HarbourSettings { get; set; }

	//Max Resources
	public int        Wheat      { get; set; }
	public int        Sheep      { get; set; }
	public int        Wood       { get; set; }
	public int        Brick      { get; set; }
	public int        Ore        { get; set; }
	public DiceValues DiceValues { get; set; }


	public Settings() {
		DiceValues      = new DiceValues();
		HarbourSettings = new HarbourSettings();
	}

	public Settings(Settings settings) {
		Players         = settings.Players;
		Deserts         = settings.Deserts;
		HarbourSettings = settings.HarbourSettings;

		//Max Resources
		Wheat = settings.Wheat;
		Sheep = settings.Sheep;
		Wood  = settings.Wood;
		Brick = settings.Brick;
		Ore   = settings.Ore;

		DiceValues = settings.DiceValues;
	}
}

public class DiceValues{
	public DiceValues() { }
	public DiceValues(int two, int three, int four, int five, int six, int eight, int nine, int ten, int eleven, int twelve) {
		Two    = two;
		Three  = three;
		Four   = four;
		Five   = five;
		Six    = six;
		//7 Hat hier nichts zu suchen
		Eight  = eight;
		Nine   = nine;
		Ten    = ten;
		Eleven = eleven;
		Twelve = twelve;
	}
	public int Two    { get; set; }
	public int Three  { get; set; }
	public int Four   { get; set; }
	public int Five   { get; set; }
	public int Six    { get; set; }
	public int Seven  { get; set; }
	public int Eight  { get; set; }
	public int Nine   { get; set; }
	public int Ten    { get; set; }
	public int Eleven { get; set; }
	public int Twelve { get; set; }
}