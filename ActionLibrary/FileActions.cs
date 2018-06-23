using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FileActions
{
    public class Command{

		/* Writes to a location. */
		public static void writeFile(String path,params object[] args) {
			System.Console.Write("Writing...");
			try {
				String toWrite = (String) args[0];//Converts args[0] into a string.
				using (FileStream fs = File.Open(path, FileMode.Append ))
				using (TextWriter writer = new StreamWriter(fs)) {
					writer.WriteLine(toWrite);
				}
				System.Console.WriteLine("Successful.\n");
			} catch (Exception specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when dating {0}",path);
				throw specifics;
			}
		}

		/* Clears a file. */
		public static void clearFile(String path, params object[] args) {
			System.Console.Write("Clearing...");
			try {
				File.WriteAllText(path,null);
				System.Console.WriteLine("Successful.\n");
			} 
			/* File Opened. */
			catch (System.IO.IOException specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("File in use. Make sure the file is not open anywhere, and try again!");
				throw specifics;
			}
			/* Abort. */
			catch (Exception specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when clearing {0}\n", path);
				throw specifics;
			}
		}

		/* Reads a File. By adding more arguements, you can change the output. */
		public static void readFile(String path, params object[] args) {
			System.Console.Write("Reading...\n\n");
			try {
				String find = (String) args[0];
				using (FileStream fs = File.Open(path, FileMode.Open))
				using (TextReader reader = new StreamReader(fs)) {
					while(reader.Peek() > -1) {
						String line = reader.ReadLine();
						System.Console.WriteLine(line);
					}
				}
				System.Console.WriteLine("\nDone Reading.\n");
			} catch (Exception specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}

		/* Removes a phrase from a file. Arguement 0 is the phrase to remove. 
		 * Note that every apearence of that phrase will be removed. 
		 * Also removes the \r\n at the end of each tab.*/
		public static void remove(String path, params object[] args)
		{
			try
			{
				String find = (String) args[0];
				System.Console.Write("Removing phrase {0}...\n\n", find);
				String buffer = File.ReadAllText(path);
				buffer = buffer.Replace(find+"\r\n","");
				File.WriteAllText(path,buffer);
				System.Console.WriteLine("\nDone.\n");
			}
			catch (Exception specifics)
			{
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}

		/* Replaces a phrase from a file. Arguement 0 is the phrase to remove. 
		 * Note that every apearence of that phrase will be removed.
		 * Arguement 1 is the new phrase to insert.
		 * Also removes the \r\n at the end of each tab.*/
		public static void replace(String path, params object[] args)
		{
			try
			{
				String find = (String)args[0];
				String insert = (String)args[1];
				System.Console.Write("Removing phrase {0}...\n\n", find);
				String buffer = File.ReadAllText(path);
				System.Console.Write("Inserting phrase {0}...\n\n", insert);
				buffer = buffer.Replace(find + "\r\n", insert+"\r\n");
				File.WriteAllText(path, buffer);
				System.Console.WriteLine("\nDone.\n");
			}
			catch (Exception specifics)
			{
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}
		//HELPER FUNCTIONS. These cannot be plans passed to Go.

		/* Find a phrase. Phrase is arguement 0. Return by setting arg 1*/
		public static bool searchFile(String path, params object[] args)
		{
			//System.Console.WriteLine("Searching...");
			try
			{
				String goalPattern = (String)args[0];
				using (FileStream fs = File.Open(path, FileMode.Open))
				using (TextReader reader = new StreamReader(fs))
				{
					while (reader.Peek() > -1)
					{
						String line = reader.ReadLine();
						if (Regex.IsMatch(line, "\\s*" + goalPattern + "\\s+"))
						{
							//System.Console.WriteLine("Phrase: '{0}' found.", goalPattern);
							return true;
						}
					}
				}
				//System.Console.WriteLine("Does not contain phrase: {0}.",(String) args[0]);
				return false;
			}
			catch (Exception specifics)
			{
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}

		/* Find a region, get it's members.*/
		public static int[] getMembers(String path, String targetRegion)
		{
			List<int> members = new List<int>();
			//System.Console.WriteLine("Searching...");
			try
			{
				String goalPattern = targetRegion;
				Boolean found = false;
				using (FileStream fs = File.Open(path, FileMode.Open))
				using (TextReader reader = new StreamReader(fs))
				{
					while (reader.Peek() > -1 && found == false)
					{
						String line = reader.ReadLine(); //moves the reader to the next line by the way.
						if (!Regex.IsMatch(line,"^\\s*#")) //Check that a hashtag does not procede everything.
						if (Regex.IsMatch(line, "^\\s*" + goalPattern + "\\s*=\\s*{")) //Does line contain region name with { after it?
						{
							System.Console.WriteLine("Phrase: '{0}' found.", goalPattern);
							found = true;
						}
					}

					if (found == false)
					{
						System.Console.WriteLine("Region not found, or improperly defined.");
						return null;
					}
					else
					{
						while (reader.Peek() > -1)
						{
							String line = reader.ReadLine(); //moves the reader to the next line by the way.
							if (!Regex.IsMatch(line, "^\\s*#")) //Check that a hashtag does not procede everything.
							if (Regex.IsMatch(line, "^}")) //Does line end now?
							{
								System.Console.WriteLine("Region '{0}' Closed", goalPattern);
								return members.ToArray();
							} else {
								line.Trim();
								while (line != ""){
									Match member = Regex.Match(line, "\\s*\\d+\\s*");
									if (member.Success){
										int newMem = Convert.ToInt32(line.Substring(member.Index, member.Length).Trim());
										members.Add(newMem);
										System.Console.WriteLine(newMem);
										line = line.Substring(member.Length);
									} else {
										member = Regex.Match(line, "^\\s*#");
										if (member.Success) {
											break;
										}
									}
								}
							}
						}
					}
					throw new System.Exception("Badly formatted Region. Regions should have the open brackets" +
								"on the line with their name, and the close bracket alone after the last line containing members.");
				}
			}
			catch (Exception specifics)
			{
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}
	}
}
