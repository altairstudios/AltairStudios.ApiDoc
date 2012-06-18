using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;


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
			string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			string output = "output";
			
			Builder.DocumentBuilder builder = new Builder.DocumentBuilder();
			
			builder.Path = path;
			builder.Package = xmlPackage;
			builder.Output = output;
			
			builder.build();
		}
		
		protected static void Help(string[] args) {
			Console.WriteLine(GetResource("AltairStudios.ApiDoc.man.en.help.txt"));
		}
		
		
		protected static void Eagle() {
			Console.WriteLine(GetResource("AltairStudios.ApiDoc.man.eagle.txt"));
		}
		
		protected static string GetResource(string resource) {
			Assembly assembly = Assembly.GetExecutingAssembly();
			StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resource));
			
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			
			string content = reader.ReadToEnd();
			reader.Close();
			
			content = content.Replace("{version}", fvi.FileVersion);
			
			return content;
		}
	}
}