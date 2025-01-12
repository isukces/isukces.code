namespace iSukces.Code
{
    /*
     public class DataObjectMaker: System.Object {
         #region Methods
         private bool AllPropertiesValue() {
             foreach (PropertyScannerItem item in scaner.Items.Values)
                 if (!IsValueOrString(item.PropertyType)) return false;
             return true;
         }

         private void FindRequiredNamespacePrefixes() {
             nsPrefixes = new Dictionary<string, string>();

             foreach (KeyValuePair<string, List<string>> pair in typesToNamespaces.Dictionary)
             {
                 if (pair.Value.Count < 2)
                 {
                     if (pair.Value.Count == 1)
                     {
                         string ns = pair.Value[0];
                         if (!nsPrefixes.ContainsKey(ns))
                             nsPrefixes[ns] = "";
                     }

                 }
                 else
                   foreach (string ns in pair.Value)
                         nsPrefixes[ns] = WellKnownNamespaces(ns);

             }
         }

         private string GetType(Type t) {
             TypeName n = TypeName.FromType(t);
             if (nsPrefixes.ContainsKey(n.Domain))
                 return nsPrefixes[n.Domain] + "." + n.Name;
             return n.Name;
         }

         private static bool IsValueOrString(Type type) {
             return type.IsValueType || type.Equals(typeof(string));
         }

         private bool Make() {
             List<EntityAttribute> a = type.GetAttributes<EntityAttribute>(false);
             if (a == (object)null || a.Count == 0) return false;
             #region Wyszukiwanie pól w typach
             scaner.scanType(type);
             #endregion
             #region Namespacesy
             foreach (PropertyInfo fi in  scaner.pola)
                 RegisterType(fi.PropertyType);

             foreach (PropertyScannerItem item in scaner.Items.Values)
             {
                 if (item.DeclaringPropertyInfo.Count == 0) continue;
                 foreach (PropertyInfo pi in item.DeclaringPropertyInfo)
                 {
                     object[] objs = pi.GetCustomAttributes(false);
                     foreach (object obj in objs)
                         RegisterType(obj.GetType());
                 }
             }
             List<string> sortedNamespaceList = new List<string>(namespacesToTypes.Dictionary.Keys);
             sortedNamespaceList.Sort();

             FindRequiredNamespacePrefixes();
             #endregion
             code = new CodeFormatter();
             code.LangInfo = new CSLangInfo();

             {

                 foreach (string nameSpace in sortedNamespaceList)
                 {
                     string namedPrefix;
                     nsPrefixes.TryGetValue(nameSpace, out namedPrefix);
                     code.Writeln("using {0}{2}{1};", namedPrefix, nameSpace, string.IsNullOrEmpty(namedPrefix) ? "" : "=");
                 }
                 code.Writeln();
             }

             code.Open("namespace {0}", myNamespace);
             code.Open("public class {0}", type.Name);
             code.Writeln("#region Properties");
             foreach(PropertyScannerItem item in scaner.Items.Values) {
                 WriteSingleProperty(item);
             }
             code.Writeln("#endregion");
             #region Podstawowe metody
             code.Writeln("#region Methods");
             _Equals();
             _GetHashCode();
             code.Writeln("#endregion");

             code.Writeln("#region Operators");
             _Operator(true);
             _Operator(false);
             code.Writeln("#endregion");
             #endregion
             code.Close(true);
             code.Close();
             return true;
         }

         private void RegisterType(Type t) {
             if (namespacesToTypes == (object)null) namespacesToTypes = new DictionaryList<string, string>();
             if (typesToNamespaces == (object)null) typesToNamespaces = new DictionaryList<string, string>();
             TypeName fulname = TypeName.FromString(t.FullName);
             if (t.IsGenericType)
             {
                 foreach (Type tt in t.GetGenericArguments())
                     RegisterType(tt);
                 fulname = TypeName.FromType(t);
             }
             namespacesToTypes.AddItem(fulname.Domain, fulname.Name);
             typesToNamespaces.AddItem(fulname.Name, fulname.Domain);
         }

         private string WellKnownNamespaces(string ns) {
             if (ns == "System.Windows") return "_syswin";
             if (ns == "System.Drawing") return "_sysdrawing";
             return ns;
         }

         private void WriteSingleProperty(PropertyScannerItem item) {
             if (item.DeclaringPropertyInfo.Count == 0) return;
             foreach (PropertyInfo pi in item.DeclaringPropertyInfo)
             {
                 // List<object> obj = pi.GetCustomAttributes(false);
             }
             PropertyInfo fi = item.DeclaringPropertyInfo[0];
             string type = TypeName.FromType(item.PropertyType, nsPrefixes).FullName;
             string rType = type;
             if (item.PreferredType != null)
                 rType = TypeName.FromType(item.PropertyType, nsPrefixes).FullName;


             string init = "";
             if (item.NotNull || (item.Trim && item.PropertyType == typeof(string)))
             {
                 if (item.PropertyType == typeof(string))
                     init = " = string.Empty";
                 else
                     init = string.Format(" = new {0}()", rType);
             }

             code.Writeln("private {0} {1}{2};", type, item.FieldName, init);

             if (!String.IsNullOrEmpty(item.Description))
             {
                 code.Writeln("/// <summary>");
                 code.Writeln("/// {0}", item.Description);
                 code.Writeln("/// </summary>");
             }
             code.Open("public {0} {1}", type, fi.Name);
             code.Open("get");
             code.Writeln("return {0};", item.FieldName);
             code.Close();
             code.Open("set");
             if (item.NotNull || (item.Trim && item.PropertyType == typeof(string)))
             {
                 code.Writeln("if ((object)value == (object)null) value {0};", init);
             }
             if (item.Trim)
                 code.Writeln("value = value.Trim();");
             code.Writeln("{0} = value;", item.FieldName);
             code.Close();
             code.Close(true);
         }

         private void _Equals() {
             code.Writeln("/// <summary>");
             code.Writeln("/// Sprawdza, czy wskazany obiekt jest równy bieżącemu");
             code.Writeln("/// </summary>");
             code.Writeln("/// <param name=\"obj\">obiekt do porównania z obiektem bieżącym</param>");
             code.Writeln("/// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>");
             code.Open("public override bool Equals(object obj)");
             code.Writeln("{0} c = obj as {0};", type.Name);
             code.Writeln("if (c == (Object)null) return false;");
             code.Writeln("return c == this;");
             code.Close();
             code.Writeln();
         }

         private void _GetHashCode() {
             code.Writeln("/// <summary>");
             code.Writeln("/// Zwraca kod HASH obiektu");
             code.Writeln("/// </summary>");
             code.Writeln("/// <returns>kod HASH obiektu</returns>");

             code.Open("public override int GetHashCode()");

             bool allPropertiesNotNull = true;
             Dictionary<string, bool> nienulowe = new Dictionary<string, bool>();

             List<PropertyScannerItem> lista = new List<PropertyScannerItem>(scaner.Items.Values);

             foreach (PropertyScannerItem item in lista)
             {
                 nienulowe[item.Name]  = true;
                 if (item.PropertyType.IsValueType) continue;
                 if (item.NotNull) continue;
                 nienulowe[item.Name] = false;
                 allPropertiesNotNull = false;

             }
             if (allPropertiesNotNull)
             {
                 StringBuilder b = new StringBuilder();
                 int i = 0;
                 foreach (PropertyScannerItem item in lista)
                 {
                     i++;
                     string x = String.Format("{0}.GetHashCode()", item.Name);
                     if (i == lista.Count) x += ";";
                     x = ((i == 1) ? "return " : "^ ") + x;
                     code.Writeln(x);
                     if (i==1) code.IncIndent();
                 }
                 if (i > 0)
                     code.DecIndent();
                 else
                     code.Writeln("return 0;");
             }
             else
             {
                 StringBuilder b = new StringBuilder();
                 foreach (PropertyScannerItem item in lista)
                 {
                     if (!nienulowe[item.Name]) continue;
                     if (b.Length > 0) b.Append(" ^ ");
                     b.Append(String.Format("{0}.GetHashCode()", item.Name));
                 }
                 b.Append(b.Length > 0 ? ";" : "0;");
                 code.Writeln("int result = " + b.ToString());
                 foreach (PropertyScannerItem item in scaner.Items.Values)
                 {
                     if (nienulowe[item.Name]) continue;
                     code.Writeln("if ((object){0} != (object)null) result ^= {0}.GetHashCode();", item.Name);
                 }
                 code.Writeln("return result;");
             }
             code.Close();
             code.Writeln();
         }

         private void _Operator(bool equal) {
             code.Writeln("/// <summary>");
             code.Writeln("/// Realizuje operator " + (equal ? "==" : "!="));
             code.Writeln("/// </summary>");
             code.Writeln("/// <param name=\"left\">lewa strona porównania</param>");
             code.Writeln("/// <param name=\"right\">prawa strona porównania</param>");
             code.Writeln("/// <returns><c>true</c> jeśli obiekty są " + (equal ? "równe" : "różne") + "</returns>");

             code.Open("public static bool operator " + (equal ? "==" : "!=") + "(" + type.Name + " left, " + type.Name + " right)");
             if (equal)
             {
                 code.Writeln("if ((object)left == (object)null && (object)right == (object)null) return " + (equal ? "true" : "false") + ";");
                 code.Writeln("if ((object)left == (object)null || (object)right == (object)null) return " + (equal ? "false" : "true") + ";");

                 StringBuilder r = new StringBuilder();
                 foreach (PropertyScannerItem pi in scaner.Items.Values)
                 {
                     if (r.Length > 0) r.Append(equal ? " && " : " || ");
                     r.Append(string.Format("left.{0} == right.{0}", pi.Name, equal ? " == " : " != "));
                 }
                 r.Append(r.Length > 0 ? ";" : "true;");
                 code.Writeln("return " + r.ToString());
             }
             else
             {
                 code.Writeln("return (!(left == rigth));");
             }
             code.Close();
         }
         #endregion

         #region Static Methods
         public static void Make(Assembly a, string Namespace, string dir) {
             Type[] ta = a.GetTypes();
             foreach (Type t in ta)
                 Make(t, Namespace, dir);
         }

         public static void Make(Type t, string Namespace, string dir) {
             DataObjectMaker m = new DataObjectMaker();
             m.type = t;
             m.myNamespace = Namespace;
             if (!m.Make()) return ;

             m.code.Save(dir + "\\" + TypeName.FromString(t.FullName).Name + ".cs");
         }
         #endregion

         #region Fields
         CodeFormatter code;
         string myNamespace;
         DictionaryList<string, string> namespacesToTypes;
         Dictionary<string, string> nsPrefixes = new Dictionary<string, string>();
         PropertyScanner scaner = new PropertyScanner();
         Type type;
         DictionaryList<string, string> typesToNamespaces;
         #endregion

     }


     class PropertyScannerItem: System.Object {
         #region Properties
         /// <summary>
         /// lista propertyInfo, które deklarują własność
         /// </summary>
         public List<PropertyInfo> DeclaringPropertyInfo {
             get {
                 return declaringPropertyInfo;
             }
             set {
                 declaringPropertyInfo = value;
             }
         }
         private List<PropertyInfo> declaringPropertyInfo = new List<PropertyInfo>();

         /// <summary>
         /// lista propertyInfo, które deklarują własność
         /// </summary>
         public string Description {
             get {
                 foreach (PropertyInfo pi in DeclaringPropertyInfo)
                     foreach (DataPropertyInfoAttribute l in pi.GetAttributes<DataPropertyInfoAttribute>(true))
                         if (!string.IsNullOrEmpty(l.Description)) return l.Description;
                 return string.Empty;
             }
         }

         public string FieldName {
             get {
                 string x = Name.Substring(0, 1).ToLower() + Name.Substring(1);
                 if (x == Name || x == "value") return x + "_";
                 return x;
             }
         }

         /// <summary>
         /// nazwa własności
         /// </summary>
         public string Name {
             get {
                 return name;
             }
             set {
                 if (value == (object)null) value = string.Empty; name = value;
             }
         }
         private string name = string.Empty;

         public bool NotNull {
             get {
                 foreach (PropertyInfo pi in DeclaringPropertyInfo)
                     foreach (DataPropertyInfoAttribute l in pi.GetAttributes<DataPropertyInfoAttribute>(true))
                         if (l.NotNull) return true;
                 return false;
             }
         }

         public Type PreferredType {
             get {
                 foreach (PropertyInfo pi in DeclaringPropertyInfo)
                     foreach (DataPropertyInfoAttribute l in pi.GetAttributes<DataPropertyInfoAttribute>(true))
                         if (l.PreferredRealType != null) return l.PreferredRealType;
                 return null;
             }
         }

         public Type PropertyType {
             get {
                 if (declaringPropertyInfo == (object)null) return null;
                 return declaringPropertyInfo[0].PropertyType;
             }
         }

         public bool Trim {
             get {
                 foreach (PropertyInfo pi in DeclaringPropertyInfo)
                     foreach (DataPropertyInfoAttribute l in pi.GetAttributes<DataPropertyInfoAttribute>(true))
                         if (l.Trim) return true;
                 return false;
             }
         }
         #endregion

         #region Methods
         public bool IsDeclaredInBaseOf(Type t) {
             if (t.BaseType == (object)null) return false;
             foreach (PropertyInfo pi in DeclaringPropertyInfo)
             {
                 if (pi.DeclaringType == t.BaseType) return true;
             }
             return IsDeclaredInBaseOf(t.BaseType);
         }
         #endregion

     }
     class PropertyScanner: System.Object {
         #region Properties
         public Dictionary<string, PropertyScannerItem> Items {
             get {
                 Dictionary<string, PropertyScannerItem> items = new Dictionary<string, PropertyScannerItem>();
                 foreach (PropertyInfo pi in pola)
                 {
                     PropertyScannerItem item;
                     if (!items.TryGetValue(pi.Name, out item))
                         items[pi.Name] = item = new PropertyScannerItem() { Name = pi.Name };
                     item.DeclaringPropertyInfo.Add(pi);
                 }
                 return items;
             }
         }
         #endregion

         #region Methods
         public void scanType(Type t) {
             if (t == (object)null) return;
             if (scannedTypes.Contains(t.FullName)) return;
             scannedTypes.Add(t.FullName);

             // skanowanie typu
             foreach (PropertyInfo fi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                 pola.Add(fi);
             // Parent
             scanType(t.BaseType);
             // interfejsy
             foreach (Type tt in t.GetInterfaces())
                 scanType(tt);
             // interfejsy z atrybutu
             List<AddInterfaceAttribute> aia = t.GetAttributes<AddInterfaceAttribute>(true);
             foreach (AddInterfaceAttribute i in aia)
                 scanType(i.Interface);
         }
         #endregion

         #region Fields
         List<string> scannedTypes = new List<string>();
         public List<PropertyInfo> pola = new List<PropertyInfo>();
         #endregion

     }
     */
}

