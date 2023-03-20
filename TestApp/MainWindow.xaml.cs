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
using Microsoft.Win32;
using PioniereVonNeuropaLibrary;

namespace TestApp{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window{
		public MainWindow() {
			InitializeComponent();

			OpenFileDialog fileDialog = new OpenFileDialog();

			if (fileDialog.ShowDialog() == true){
				Game deserialize = JsonSerializer.Deserialize<Game>(fileDialog.OpenFile()) ?? throw new InvalidOperationException();
				Game.MakePlayable(deserialize);
				Console.Out.WriteLine("");
			}


		}
	}
}