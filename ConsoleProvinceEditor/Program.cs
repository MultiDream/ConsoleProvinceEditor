using System;
using System.IO;
using System.Text.RegularExpressions;
using FileActions;

namespace ConsoleProvinceEditor {
	class Program {
		//Directory containing province files.
		public const String dir = @"C:\Users\lcderado\Documents\TextPlayground";
		static void Main(string[] args) {
			bool running = true;
			String command = "";
			while (running) {
				//Retrieve User Command
				Console.WriteLine(">> Enter a command...");
				String input = Console.ReadLine();
				Console.WriteLine("your command was:\n{0}\n",input);

				//Interpret Command.
				inp
				//Switch statement selects command.
				switch (input) {
					case "quit":
						Console.WriteLine("Quiting.");
						running = false;
						break;
				}
			}
		}
		public static void testBuild(String buildDir) {
			for (int i = 1; i < 1000; i++) {
				String path = buildDir + "\\" + i + " - TestFile_" + i + ".txt";
				File.Create(path);
			}
		}
	}

	/*
	 * Console always need to grab from the same place, all that changes
	 * is what happens once they find the right file.
	 * 
	 * A Console can perform a command.
	 * */
	public class CommandConsole {
		public String ActDir {get; set;}
		private int[] members;
		public delegate void Plan(String path, params object[] arguments); //What to do once you get to a file.
		public CommandConsole(String actDir,int[] members) {
			ActDir = actDir;
			this.members = members;
		}
		public void Go(Plan function, params object[] args) {
			//wraps around call.
			String[] files = Directory.GetFiles(ActDir);
			int[] targets = members;
			int countMiss = 0;
			foreach (int target in targets) {
				String pattern = @"\\" + target.ToString() + @"\s";
				bool exists = false;
				foreach (string path in files) {
					if (Regex.Match(path, pattern).Success) {
						Console.WriteLine("Member {0} found...",path);
						function(path, args);
						exists = true;
						break;
					}
				}
				if (!exists) {
					Console.WriteLine("Member {0} not found.", target);
					countMiss++;
				}
			}
			if (countMiss > 0) {
				Console.WriteLine("Missing {0} files!", countMiss);
			}
		}
	}
}
