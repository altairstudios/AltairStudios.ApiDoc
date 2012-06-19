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
		protected DocumentHelper helper;

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
			
			this.helper = new DocumentHelper();
			helper.Document = this.document;
			helper.Verbose = true;
			
			helper.getNamespaces();
			helper.getClasses("AltairStudios.Core.Orm.Models");
			helper.getClasses("AltairStudios.Core.Orm.Models.Admin");
			
			
			if(!Directory.Exists(this.baseOutput)) {
				Directory.CreateDirectory(this.baseOutput);
			}
			
			this.createAssets();
			this.createHomeTemplate();
			this.createNamespacesTemplates();
			/*this.createClassesTemplates();*/
		}
		
		
		protected void createHomeTemplate() {
			this.createTemplate("index.html");
		}
		
		
		protected void createNamespacesTemplates() {
			List<string> namespaces = this.helper.getNamespaces();
			
			for(int i = 0; i < namespaces.Count; i++) {
				List<string> classes = this.helper.getClasses(namespaces[i]);
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("namespace", namespaces[i]);
				
				StringBuilder classesList = new StringBuilder();
				
				for(int j = 0; j < classes.Count; j++) {
					classesList.Append("<li><a href='" + this.helper.getClassLink(classes[j]) + "'>" + classes[j] + "</a></li>");
					this.createClassTemplates(classes[j]);
				}
				
				parameters.Add("classesList", classesList.ToString());

				this.createTemplate(this.helper.getNamespaceLink(namespaces[i]), "AltairStudios.ApiDoc.templates.api.api-namespace.html", parameters);
			}
		}
		
		
		protected void createClassTemplates(string classes) {
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			StringBuilder fieldList = new StringBuilder();
			StringBuilder propertyList = new StringBuilder();
			StringBuilder methodList = new StringBuilder();
			List<string> fields = this.helper.getFields(classes);
			List<string> properties = this.helper.getProperties(classes);
			List<string> methods = this.helper.getMethods(classes);
			
			for(int i = 0; i < fields.Count; i++) {
				fieldList.Append("<li>" + fields[i] + "</li>");
			}
			
			for(int i = 0; i < properties.Count; i++) {
				propertyList.Append("<li>" + properties[i] + "</li>");
			}
			
			for(int i = 0; i < methods.Count; i++) {
				methodList.Append("<li>" + methods[i] + "</li>");
			}
			
			parameters.Add("class", classes);
			parameters.Add("fieldList", fieldList.ToString());
			parameters.Add("propertyList", propertyList.ToString());
			parameters.Add("methodList", methodList.ToString());
			
			this.createTemplate(this.helper.getClassLink(classes), "AltairStudios.ApiDoc.templates.api.api-class.html", parameters);
		}
		
		
		protected string getMenuAPI() {
			List<string> namespaces = this.helper.getNamespaces();
			StringBuilder menu = new StringBuilder();
			
			for(int i = 0; i < namespaces.Count; i++) {
				menu.Append("<li><a href='" + this.helper.getNamespaceLink(namespaces[i]) + "'>" + namespaces[i] + "</a></li>");
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
			
			content = content.Replace("{@content}", "");
			
			content = content.Replace("{@path}", ".");
			content = content.Replace("{@year}", DateTime.Now.Year.ToString());
			content = content.Replace("{@menuAPI}", this.getMenuAPI());
			
			
			File.WriteAllText(this.baseOutput + "/" + page, content);
		}
		
		
		
		protected void createTemplate(string page, string section, Dictionary<string, string> parameters) {
			string content = this.getResourceContent("AltairStudios.ApiDoc.templates.master.html");
			
			content = content.Replace("{@content}", this.getResourceContent(section));
			
			content = content.Replace("{@path}", ".");
			content = content.Replace("{@year}", DateTime.Now.Year.ToString());
			content = content.Replace("{@menuAPI}", this.getMenuAPI());
			
			foreach(string key in parameters.Keys) {
				content = content.Replace("{@" + key + "}", parameters[key]);	
			}
			
			File.WriteAllText(this.baseOutput + "/" + page, content);
		}
	}
}