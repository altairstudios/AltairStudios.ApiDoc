using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;


namespace AltairStudios.ApiDoc.Builder {
	public class DocumentBuilder {
		protected string path;
		protected string package;
		protected string output;
		protected string baseOutput;
		protected XmlDocument document;

		public string Path {
			get {
				return this.path;
			}
			set {
				path = value;
			}
		}

		public string Package {
			get {
				return this.package;
			}
			set {
				package = value;
			}
		}
		
		public string Output {
			get {
				return this.output;
			}
			set {
				output = value;
			}
		}
		
		public DocumentBuilder() {
			
		}
				
		public void build() {
			this.baseOutput = this.path + "/" + this.output;
			
			this.document = new XmlDocument();
			this.document.Load(this.path + "/" + this.package);
			
			if(!Directory.Exists(this.baseOutput)) {
				Directory.CreateDirectory(this.baseOutput);
			}
			
			this.createAssets();
			this.createHomeTemplate();
		}
		
		
		protected void createHomeTemplate() {
			this.createTemplate("index.html");
		}
		
		
		protected string getMenuAPI() {
			StringBuilder menu = new StringBuilder();
			XmlNodeList nodelist = this.document.SelectNodes("/doc/members/member/@name");
			List<string> namespaces = new List<string>();
			
			for(int i = 0; i < nodelist.Count; i++) {
				string member = nodelist[i].Value;
				string[] members = member.Split(":".ToCharArray());
				string currentNamespace = System.IO.Path.GetDirectoryName(members[1].Replace(".", "/"));
				
				if(members[0] == "T" && !namespaces.Contains(currentNamespace)) {
					namespaces.Add(currentNamespace);
					menu.Append("<li><a href='#'>" + currentNamespace + "</a></li>");
				}
			}
			
			return menu.ToString();
		}
		
		
		protected void createAssets() {
			if(!Directory.Exists(this.baseOutput + "/assets")) {
				Directory.CreateDirectory(this.baseOutput + "/assets");
			}
			
			File.WriteAllText(this.baseOutput + "/assets/bootstrap.css", this.getResourceContent("AltairStudios.Core.resources.css.bootstrap.css"));
			File.WriteAllText(this.baseOutput + "/assets/bootstrap-responsive.css", this.getResourceContent("AltairStudios.Core.resources.css.bootstrap-responsive.css"));
			File.WriteAllText(this.baseOutput + "/assets/jquery.js", this.getResourceContent("AltairStudios.Core.resources.js.jquery.js"));
			File.WriteAllText(this.baseOutput + "/assets/bootstrap.js", this.getResourceContent("AltairStudios.Core.resources.js.bootstrap.js"));
			File.WriteAllText(this.baseOutput + "/assets/bootstrap-dropdown.js", this.getResourceContent("AltairStudios.Core.resources.js.bootstrap-dropdown.js"));
		}
		
		protected string getResourceContent(string resource) {
			string content = this.getResourceContentApi(resource);
			
			if(content == null) {
				content = this.getResourceContentCore(resource);
				
				if(content == null) {
					throw new IOException("Resource not found: " + resource);
				}
			}
			
			return content;
		}
		
		protected string getResourceContentApi(string resource) {
			string content;
			
			Assembly assembly = Assembly.GetExecutingAssembly();
			
			if(assembly.GetManifestResourceStream(resource) == null) {
				return null;
			} else {
				StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resource));
				content = reader.ReadToEnd();
				reader.Close();
				return content;
			}
		}
		
		protected string getResourceContentCore(string resource) {
			string content;
			
			Assembly ascore = Assembly.LoadFrom(this.path + "/AltairStudios.Core.dll");
			
			if(ascore.GetManifestResourceStream(resource) == null) {
				return null;
			} else {
				StreamReader reader = new StreamReader(ascore.GetManifestResourceStream(resource));
				content = reader.ReadToEnd();
				reader.Close();
				return content;
			}
		}
		
		
		protected void createTemplate(string page) {
			string content = this.getResourceContent("AltairStudios.ApiDoc.templates.master.html");
			
			content = content.Replace("{@path}", ".");
			content = content.Replace("{@year}", DateTime.Now.Year.ToString());
			content = content.Replace("{@menuAPI}", this.getMenuAPI());
			
			
			File.WriteAllText(this.baseOutput + "/" + page, content);
		}
	}
}