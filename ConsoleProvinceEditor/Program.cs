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
		#region Run
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
		#endregion
	}

	/*
	 * Console always need to grab from the same place, all that changes
	 * is what happens once they find the right file.
	 * 
	 * A Console can perform a command.
	 * */
	public class CommandConsole {
		public String ActDir;
		public String resourceDir;

		public delegate void Plan(String path, params object[] arguments); //What to do once you get to a file.
		#region CommandConsole
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
		}
		#endregion
		#region Interpret
		/*Huge method where all commands are interpretted by the console. 
		 * I should note at some point, this needs to be broken up.
		 * It violates single responsibility policy, and should
		 * fire an event based on the first string.
		 *
		 */
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
								System.Console.WriteLine("{0} Members", members.Length);
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
					case "kill":
						if (input.Length > 1)
						{
							int[] members = FileActions.Command.getMembers(resourceDir + "\\regions.txt", input[1]);
							if (members != null)
							{
								Plan kill = Command.clearFile; 
								Go(kill,members);//no other arguements necessary
							}
							else
							{
								Console.WriteLine("Need a second arguement <region> for delete command.");
							}
						}
						else
						{
							Console.WriteLine("Need a second arguement <region> for delete command.");
						}
						break;
					case "write":
						if (input.Length > 2)
						{
							int[] members = FileActions.Command.getMembers(resourceDir + "\\regions.txt", input[1]);
							if (members != null)
							{
								Plan append = Command.writeFile;
								String noQuotes = input[2].Substring(1, input[2].Length - 2);
								Go(append, members,noQuotes);
							}
							else
							{
								Console.WriteLine("Need three arguements for the write command.");
							}
						}
						else
						{
							Console.WriteLine("Need three arguements for the write command.");
						}
						break;
					case "remove":
						if (input.Length > 2)
						{
							int[] members = FileActions.Command.getMembers(resourceDir + "\\regions.txt", input[1]);
							if (members != null)
							{
								Plan remove= Command.remove;
								String noQuotes = input[2].Substring(1, input[2].Length - 2);
								Go(remove, members, noQuotes);
							}
							else
							{
								Console.WriteLine("Need three arguements for the remove command.");
							}
						}
						else
						{
							Console.WriteLine("Need three arguements for the remove command.");
						}
						break;
					case "removeline":
						if (input.Length > 2) {
							int[] members = FileActions.Command.getMembers(resourceDir + "\\regions.txt", input[1]);
							if (members != null) {
								Plan append = Command.removeAttribute;
								String noQuotes = input[2].Substring(1, input[2].Length - 2);
								Go(append, members, noQuotes);
							} else {
								Console.WriteLine("Region is Empty, or not found.");
							}
						} else {
							Console.WriteLine("Need three arguements for the removeLine command.");
						}
						break;
					case "replace":
						if (input.Length > 3)
						{
							int[] members = FileActions.Command.getMembers(resourceDir + "\\regions.txt", input[1]);
							if (members != null)
							{
								Plan replace = Command.replace;
								String noQuoteOut = input[2].Substring(1, input[2].Length - 2);
								String noQuoteIn = input[3].Substring(1, input[3].Length - 2);
								Go(replace, members, noQuoteOut, noQuoteIn);
							}
							else
							{
								Console.WriteLine("Need four arguements for the replace command.");
							}
						}
						else
						{
							Console.WriteLine("Need three arguements for the replace command.");
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
		#endregion
		#region MakeCommand
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
					break;
				}

				//Make sure to capture a square brackets as ONE COMMAND
				nextWord = Regex.Match(remaining, "^\\s*\\{(.*?)\\}\\s*"); // Look for phrase of Square brackets containing ANYTHING.
				if (nextWord.Success) {
					String hold = remaining.Substring(nextWord.Index, nextWord.Length).Trim();
					words.Add(hold);
					break;
				}

				//Catch Phrases with the right characters
				nextWord = Regex.Match(remaining, "^\\s*[a-zA-Z]+\\s*");
				if (nextWord.Success) {
					String hold = remaining.Substring(nextWord.Index, nextWord.Length).Trim();
					words.Add(hold);
					remaining = remaining.Substring(nextWord.Length).Trim();
				}
				
			}
			while (nextWord.Success && remaining != ""); //nothing caught.
			return words.ToArray();
		}
		#endregion
		#region Go
		/* Pass a function to Go to perform it on all members of a region.*/
		public void Go(Plan function, int[] members, params object[] args) {
			//wraps around call.
			String[] files = Directory.GetFiles(ActDir);
			int[] targets = members;
			int countMiss = 0;
			foreach (int target in targets) {
				String pattern = @"\\" + target.ToString() + "[\\s\\-]";
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
		#endregion
	}
}
