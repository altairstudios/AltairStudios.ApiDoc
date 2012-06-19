using System;
using System.Collections.Generic;
using System.Xml;


namespace AltairStudios.ApiDoc.Builder {
	public class DocumentHelper {
		protected XmlDocument document;
		protected bool verbose = false;

		public XmlDocument Document {
			get {
				return this.document;
			}
			set {
				document = value;
			}
		}

		public bool Verbose {
			get {
				return this.verbose;
			}
			set {
				verbose = value;
			}
		}
		
		public DocumentHelper() {
		}
		
		public List<string> getNamespaces() {
			XmlNodeList nodelist = this.document.SelectNodes("/doc/members/member/@name");
			List<string> namespaces = new List<string>();
			
			for(int i = 0; i < nodelist.Count; i++) {
				string member = nodelist[i].Value;
				string[] members = member.Split(":".ToCharArray());
				string currentNamespace = System.IO.Path.GetDirectoryName(members[1].Replace(".", "/")).Replace("/", ".");
				
				if(members[0] == "T" && !namespaces.Contains(currentNamespace)) {
					namespaces.Add(currentNamespace);
				}
			}
			
			if(this.verbose == true) {
				Console.WriteLine("NAMESPACES:");
				for(int i = 0; i < namespaces.Count; i++) {
					Console.WriteLine("\t" + namespaces[i]);
				}
				Console.WriteLine("\n\n");
			}
			
			return namespaces;
		}
		
		
		public List<string> getClasses(string classNamespace) {
			XmlNodeList nodelist = this.document.SelectNodes("/doc/members/member/@name");
			List<string> classes = new List<string>();
			
			for(int i = 0; i < nodelist.Count; i++) {
				string member = nodelist[i].Value;
				string[] members = member.Split(":".ToCharArray());
				string currentNamespace = System.IO.Path.GetDirectoryName(members[1].Replace(".", "/")).Replace("/", ".");
				
				if(members[0] == "T" && currentNamespace == classNamespace) {
					if(members[1].Substring(members[1].Length - 2) == "`1") {
						members[1] = members[1].Replace("`1", "<T>");
					}
					classes.Add(members[1]);
				}
			}
			
			if(this.verbose == true) {
				Console.WriteLine("CLASSES:");
				for(int i = 0; i < classes.Count; i++) {
					Console.WriteLine("\t" + classes[i]);
				}
				Console.WriteLine("\n\n");
			}
			
			return classes;
		}
		
		
		
		public List<string> getFields(string className) {
			XmlNodeList nodelist = this.document.SelectNodes("/doc/members/member/@name");
			List<string> fields = new List<string>();
			
			for(int i = 0; i < nodelist.Count; i++) {
				string member = nodelist[i].Value;
				string[] members = member.Split(":".ToCharArray());
				string currentClass = System.IO.Path.GetDirectoryName(members[1].Replace(".", "/")).Replace("/", ".");
				
				if(members[0] == "F" && currentClass == className) {
					if(members[1].Substring(members[1].Length - 2) == "`1") {
						members[1] = members[1].Replace("`1", "<T>");
					}
					fields.Add(members[1]);
				}
			}
			
			if(this.verbose == true) {
				Console.WriteLine("FIELDS:");
				for(int i = 0; i < fields.Count; i++) {
					Console.WriteLine("\t" + fields[i]);
				}
				Console.WriteLine("\n\n");
			}
			
			return fields;
		}
		
		
		public List<string> getProperties(string className) {
			XmlNodeList nodelist = this.document.SelectNodes("/doc/members/member/@name");
			List<string> properties = new List<string>();
			
			for(int i = 0; i < nodelist.Count; i++) {
				string member = nodelist[i].Value;
				string[] members = member.Split(":".ToCharArray());
				string currentClass = System.IO.Path.GetDirectoryName(members[1].Replace(".", "/")).Replace("/", ".");
				
				if(members[0] == "P" && currentClass == className) {
					if(members[1].Substring(members[1].Length - 2) == "`1") {
						members[1] = members[1].Replace("`1", "<T>");
					}
					properties.Add(members[1]);
				}
			}
			
			if(this.verbose == true) {
				Console.WriteLine("PROPERTIES:");
				for(int i = 0; i < properties.Count; i++) {
					Console.WriteLine("\t" + properties[i]);
				}
				Console.WriteLine("\n\n");
			}
			
			return properties;
		}
		
		
		
		public List<string> getMethods(string className) {
			XmlNodeList nodelist = this.document.SelectNodes("/doc/members/member/@name");
			List<string> methods = new List<string>();
			
			for(int i = 0; i < nodelist.Count; i++) {
				string member = nodelist[i].Value;
				string[] members = member.Split(":".ToCharArray());
				string currentClass = System.IO.Path.GetDirectoryName(members[1].Replace(".", "/")).Replace("/", ".");
				
				if(members[0] == "M" && currentClass == className) {
					if(members[1].Substring(members[1].Length - 2) == "`1") {
						members[1] = members[1].Replace("`1", "<T>");
					}
					methods.Add(members[1]);
				}
			}
			
			if(this.verbose == true) {
				Console.WriteLine("METHODS:");
				for(int i = 0; i < methods.Count; i++) {
					Console.WriteLine("\t" + methods[i]);
				}
				Console.WriteLine("\n\n");
			}
			
			return methods;
		}
		
		
		public string formatUrl(string url) {
			url = url.ToLower();
			url = url.Replace("<T>", "__T__");
			
			return url;
		}
		
		public string getNamespaceLink(string namespaces) {
			return this.getNamespaceLink(namespaces, true);
		}
		
		public string getNamespaceLink(string namespaces, bool format) {
			if(format) namespaces = this.formatUrl(namespaces);
			return "api-namespace-" + namespaces + ".html";
		}
		
		public string getClassLink(string classes) {
			return this.getClassLink(classes, true);
		}
		
		public string getClassLink(string classes, bool format) {
			if(format) classes = this.formatUrl(classes);
			return "api-class-" + classes + ".html";
		}
	}
}