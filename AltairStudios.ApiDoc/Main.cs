using System;
using System.IO;


namespace AltairStudios.ApiDoc {
	class MainClass {
		protected static string[] arguments = new string[] {"help"};
		
		public static void Main (string[] args) {
			if(args.Length == 0 || args[0] == "help" || args[0] == "--help" || args[0] == "/help") {
				Help(args);
			} else {
				if(args.Length >= 2 && args[0] == "build" && args[1] == "eagle") {
					Eagle();
				} else if(args.Length >= 2 && args[0] == "build") {
					Build(args);
				}
			}
		}
		
		protected static void Build(string[] args) {
			string xmlPackage = args[1];
			
			
		}
		
		protected static void Help(string[] args) {
			Console.WriteLine(GetResource("AltairStudios.ApiDoc.man.en.help.txt"));
		}
		
		
		protected static void Eagle() {
			Console.WriteLine(GetResource("AltairStudios.ApiDoc.man.eagle.txt"));
		}
		
		protected static string GetResource(string resource) {
			StreamReader reader = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resource));	
			return reader.ReadToEnd();
		}
	}
}