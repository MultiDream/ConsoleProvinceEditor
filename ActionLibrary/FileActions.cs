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
				String toWrite = (String) args[0]; //Converts args[0] into a string.
				List<String> allText = new List<string>();
				String[] allLines = File.ReadAllLines(path);

				/* I need some logic to determine whether or to post on a line.
				 * I believe that each file comes with 1 line for a comment,
				 * and then the 1444 data starts. When Editting new info,
				 * we want to add there, and since that will always be the secondline,
				 * that's where we'll put it automatically.
				 * */
				if (allLines.Length <= 0) {
					allText.Add(toWrite);
				}
				for(int i = 0; i < allLines.Length;i++){
					if (i == 0) {
						allText.Add(allLines[i]);
						allText.Add(toWrite);
					} else {
						allText.Add(allLines[i]);
					}
				}
				File.WriteAllLines(path,allText);
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

		/* RemoveLine removes the WHOLE LINE if a matching attribute if found. */
		public static void removeAttribute(String path, params object[] args) {
		System.Console.Write("Writing...");
			try {
				String toFind = (String)args[0]; //Converts args[0] into a string.
				List<String> allText = new List<string>();
				String[] allLines = File.ReadAllLines(path);
				
				//Don't Add the lines that contain a match.
				for(int i = 0; i < allLines.Length;i++){
					String pattern = "\\s*" + toFind+"\\s*";
					if (!Regex.IsMatch(allLines[i],pattern)) {
						allText.Add(allLines[i]);
					}
				}
				File.WriteAllLines(path, allText);
				System.Console.WriteLine("Successful.\n");
			} catch (Exception specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when dating {0}",path);
				throw specifics;
			}
		}

		/* Given a path, verify that the file meets a condition.*/
		public static void condition(String path, params object[] args){
			System.Console.Write("Verifying Condition...\n\n");
			try {
				String find = (String) args[0];
				int condition = (int)args[1];
				//Conditions must be converted into an int.
				using (FileStream fs = File.Open(path, FileMode.Open))
				using (TextReader reader = new StreamReader(fs)) {
					while (reader.Peek() > -1) {
						String line = reader.ReadLine();
						if (Regex.IsMatch(find,line)){	//If the value is contained, evalute the condition.
							// Now, take the phrase after the equal sign.
						}
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
			List<int> members;
			//Do a quick check to see if the targetRegion is actually anonymous region.
			if (Regex.IsMatch(targetRegion, "^\\s*{")){
				//Get all internal members.
				members = getLineMembers(targetRegion,path,null);
				return members.ToArray();
			}
			members = new List<int>();
			//System.Console.WriteLine("Searching...");
			try {
				Boolean found = false;
				using (FileStream fs = File.Open(path, FileMode.Open,FileAccess.Read,FileShare.Read))
				using (TextReader reader = new StreamReader(fs)) {
					while (reader.Peek() > -1 && found == false) {
						String line = reader.ReadLine(); //moves the reader to the next line by the way.
						if (!Regex.IsMatch(line, "^\\s*#")) //Check that a hashtag does not procede everything.
							if (Regex.IsMatch(line, "^\\s*" + targetRegion + "\\s*=\\s*{")) //Does line contain region name with { after it?
							{
								System.Console.WriteLine("Phrase: '{0}' found.", targetRegion);
								found = true;
							}
					}

					if (found == false) {
						System.Console.WriteLine("Region {0} not found, or improperly defined.", targetRegion);
						return null;
					} else {
						while (reader.Peek() > -1) {
							String line = reader.ReadLine(); //moves the reader to the next line by the way.
							if (!Regex.IsMatch(line, "^\\s*#")) //Check that a hashtag does not procede everything.
								if (Regex.IsMatch(line, "^}")) //Does line end now?
								{
									System.Console.WriteLine("Region '{0}' Closed", targetRegion);
									return members.ToArray();
								} else {
									line = line.Trim();
									List<int> lineMembers = getLineMembers(line, path, targetRegion);
									members.AddRange(lineMembers);
								}
						}
						throw new System.Exception("Badly formatted Region. Regions should have the open brackets" +
									"on the line with their name, and the close bracket alone after the last line containing members.");
					}
				}
			} catch (Exception specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}

		private static List<int> getLineMembers(String line, String path, String targetRegion) {
			//remove
			List<int> members = new List<int>();
			while (line != "") {

				Match member = Regex.Match(line,"^\\s*{");
				if (member.Success){
					line = line.Substring(member.Length);
				}
				member = Regex.Match(line, "^\\s*\\d+\\s*");
				if (member.Success) {
					int newMem = Convert.ToInt32(line.Substring(member.Index, member.Length).Trim());
					members.Add(newMem);
					line = line.Substring(member.Length);
				}

				//End Early if next char is '#' or '}'
				member = Regex.Match(line, "^\\s*#");
				if (member.Success) {
					break;
				}
				member = Regex.Match(line, "^\\s*}");
				if (member.Success) {
					break;
				}

				// If another alias is found, get it's members.
				member = Regex.Match(line, "^\\s*[a-zA-Z_]+\\s*");
				if (member.Success) {
					// If the region recursively contains itself, throw an error.
					if (member.Value == targetRegion) {
						throw new FormatException(
						"You must not include a region within itself. That is recursive, and problematic.");
					}
					string subTarget = member.Value.Trim();
					int[] otherMembers = getMembers(path, subTarget);
					if (otherMembers != null) {
						foreach (int i in otherMembers) {
							members.Add(i);
						}
					}
					line = line.Substring(member.Length);
				}
				line = line.Trim();
			}

			return members;
		}
	}
}
