using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PioniereVonNeuropaLibrary;

namespace TestApp{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window{
		public MainWindow() {
			InitializeComponent();

			Game   game      = new Game(5, 5);
			string serialize = JsonSerializer.Serialize(game, new JsonSerializerOptions { WriteIndented = true });
			Console.Out.WriteLine(serialize);

			Game deserialize = JsonSerializer.Deserialize<Game>(serialize) ?? throw new InvalidOperationException();
			Console.Out.WriteLine("");
		}
	}
}