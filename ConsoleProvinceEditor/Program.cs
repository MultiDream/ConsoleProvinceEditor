using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using FileActions;

namespace ConsoleProvinceEditor {
	class Program {
		static void Main(string[] args) {
			Run();
		}
		public static void Run()
		{
			System.Console.WriteLine("Enter the directory. Do not inclue the / . BECAREFUL TO GET THIS RIGHT!");
			String dir = Console.ReadLine(); //Directory containing province files.
			CommandConsole console = new CommandConsole(dir);
			bool running = true;
			while (running)
			{
				//Retrieve User Command
				Console.WriteLine(">> Enter a command...");
				String input = Console.ReadLine();
				//Interpret Command.
				running = console.Interpret(console.MakeCommand(input));
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
		public String ActDir;
		public int[] members;
		public String resourceDir;

		public delegate void Plan(String path, params object[] arguments); //What to do once you get to a file.
		public CommandConsole(String actDir) {
			//Setup procedure:

			//1. Make sure file structure is proper.
			ActDir = actDir;
			resourceDir = actDir + "\\resources";

			//Do not create files for now. Only edit them.
			if (Directory.Exists(resourceDir))
			{
				Console.WriteLine("Resource Directory Found");
				if (File.Exists(resourceDir + "\\regions.txt"))
				{
					Console.WriteLine("Regions file found.");
					Console.WriteLine("Your directory entry is probably correct.");
				} else
				{
					Console.WriteLine("Regions file needs to be created at "+ resourceDir + "\\regions.txt");
				}
			} else
			{
				Console.WriteLine("Resource Directory Needs to be created at " + resourceDir + " .");
			}

			//2. Build regions
		}

		/*Huge method where all commands are interpretted by the console. */
		public Boolean Interpret(String[] input)
		{
			if (input.Length > 0) {
				String word = input[0].ToLower();
				switch (word)
				{
					case "region":
						//Do that method.
						if (input.Length > 1)
						{
							int[] members = FileActions.Command.getMembers(resourceDir+"\\regions.txt",input[1]);
							if (members != null){
								System.Console.WriteLine("{0} Members", input[1]);
								foreach (int member in members)
								{
									System.Console.WriteLine(member.ToString());
								}
							}
						}
						else
						{
							Console.WriteLine("Need a second arguement <region> for region command.");
						}
						break;
					case "exit":
						return false; //stop
					default:
						Console.WriteLine("Did not recognize command '{0}'.",word);
						break;
				}
			}
			return true; //keep running
		}

		public String[] MakeCommand(String input)
		{
			List<String> words = new List<string>();
			String remaining = input.Trim();
			Match nextWord;
			do
			{
				//Make sure to capture a phrase as ONE string.
				nextWord = Regex.Match(remaining,"^\\s*\\\"(.*?)\\\"\\s*"); // Look for phrase of double quotes containing ANYTHING.
				if (nextWord.Success)
				{
					String hold = remaining.Substring(nextWord.Index, nextWord.Length).Trim();
					words.Add(hold);
				}
				else
				{
					nextWord = Regex.Match(remaining, "^\\s*[a-zA-Z0-9{}]+\\s*");
					String hold = remaining.Substring(nextWord.Index, nextWord.Length).Trim();
					words.Add(hold);
				}
				remaining = remaining.Substring(nextWord.Length).Trim();
			}
			while (nextWord.Success && remaining != ""); //nothing caught.
			return words.ToArray();
		}

		/* Pass a function to Go to perform it on all members of a region.*/
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
