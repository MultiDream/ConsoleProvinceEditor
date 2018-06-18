using System;
using System.IO;

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

		/* Reads a File. */
		public static void readFile(String path, params object[] args) {
			System.Console.Write("Reading...\n\n");
			try {
				using (FileStream fs = File.Open(path, FileMode.Open))
				using (TextReader reader = new StreamReader(fs)) {
					while(reader.Peek() > -1) {
						Console.WriteLine(reader.ReadLine());
					}
				}
				System.Console.WriteLine("\nDone Reading.\n");
			} catch (Exception specifics) {
				Console.WriteLine("Failed!");
				Console.WriteLine("Unknown error occured when reading {0}", path);
				throw specifics;
			}
		}
	}
}
