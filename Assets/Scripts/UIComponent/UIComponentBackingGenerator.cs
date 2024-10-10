using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.UIComponent
{
    struct Field
    {
        public Type type;
        public string name;
    }

    internal class UIComponentBackingGenerator
    {
        private readonly VisualElement root;
        private readonly string assetPath;
        private readonly string name;

        private List<Field> fields = new List<Field>();
        private HashSet<string> ucssClasses = new HashSet<string>();

        public UIComponentBackingGenerator(string assetPath, VisualElement root)
        {
            this.assetPath = assetPath;
            name = GetAssetName(assetPath);
            this.root = root;
        }

        private static string GetAssetName(string assetPath)
        {
            // Remove the file extension
            string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

            // Remove the path
            assetName = System.IO.Path.GetFileName(assetName);

            return assetName;
        }

        public string Generate()
        {
            ProcessElementAndChildren(root);
            return GenerateBackingClass();
        }

        private static HashSet<string> nameBlacklist = new HashSet<string>()
        {

        };

        private void ProcessElementAndChildren(VisualElement element)
        {
            ProcessElement(element);

            // Don't process UIComponents' children
            // since their children can be accessed as fields
            if (element is UIComponent) return;
            //UnityEngine.Debug.Log(element.GetType().Name);

            foreach (var child in element.hierarchy.Children())
            {
                ProcessElementAndChildren(child);
            }
        }

        private void ProcessElement(VisualElement element)
        {
            if (element.name == null) return;
            if (nameBlacklist.Contains(element.name)) return;
            string name = element.name.Trim();
            if (name.Length == 0) return;
            if (name.StartsWith("unity-")) return;

            fields.Add(new Field()
            {
                name = name,
                type = element.GetType()
            });

            foreach (var cls in element.GetClasses())
            {
                ucssClasses.Add(cls);
            }
        }

        //private Dictionary<string, List<Field>> FindDuplicates()
        //{

        //foreach (var field in fields)
        //{
        //    if (names.Add(field.name))
        //    {

        //    }
        //}
        //}

        private const string HEADER =
@"
using UnityEngine.UIElements;
namespace Assets.UI
{
";

        private const string TEMPLATE =
@"
        private VisualTreeAsset _template;
        [UxmlAttribute]
        public VisualTreeAsset template
        {
            get => _template;
            set
            {
                _template = value;
                Clear();
                if (_template != null)
                {
                    _template.CloneTree(this);
                    SetFields();
                    OnTreeLoaded();
                }
            }
        }
";

        private const string LOAD_TEMPLATE =
@"

";

        private string GenerateBackingClass()
        {
            HashSet<string> fieldNames = new HashSet<string>();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(HEADER);
            sb.AppendLine(@"[UxmlElement]");
            sb.AppendLine(@$"public partial class {name} : UIComponent.UIComponent");
            sb.AppendLine(@"{");

            GenerateClassEnum(sb);

            sb.AppendLine(TEMPLATE);

            for (int i = 0; i < fields.Count; i++)
            {
                Field field = fields[i];
                if (fieldNames.Contains(field.name))
                {
                    fields.RemoveAt(i--);
                    UnityEngine.Debug.LogWarning($"Duplicate Element name: {field.name}");
                    continue;
                }
                fieldNames.Add(field.name);
                GenerateField(sb, field);
            }
            GenerateLoadTemplate(sb);
            GenerateSetFields(sb);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }

        private void GenerateLoadTemplate(StringBuilder sb)
        {
            sb.AppendLine(@"protected override void LoadTemplate()");
            sb.AppendLine(@"{");
            sb.AppendLine(@"if (template != null) return;");
            sb.AppendLine($"template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(\"{assetPath}\");");
            sb.AppendLine(@"}");
        }

        private void GenerateClassEnum(StringBuilder sb)
        {
            sb.AppendLine(@"protected static class Classes");
            sb.AppendLine(@"{");
            foreach (string className in ucssClasses)
            {
                string name = ConvertToValidFieldName(className);
                sb.AppendLine($"const string {name} = \"{className}\";");
            }
            sb.AppendLine(@"}");
        }

        private void GenerateSetFields(StringBuilder sb)
        {
            sb.AppendLine(@"protected override void SetFields()");
            sb.AppendLine(@"{");
            foreach (Field field in fields)
            {
                string name = ConvertToValidFieldName(field.name);
                sb.AppendLine($"{name} = this.Q<{field.type.Name}>(\"{field.name}\");");
            }
            sb.AppendLine(@"}");
        }

        private void GenerateField(StringBuilder sb, Field field)
        {
            string name = ConvertToValidFieldName(field.name);
            sb.AppendLine($@"public {field.type.Name} {name} {{ get; private set; }}");
        }

        public string ConvertToValidFieldName(string input)
        {
            // Step 1: Remove invalid characters and replace them with underscores
            string cleanedInput = Regex.Replace(input, @"[^a-zA-Z0-9_]", "_");

            // Step 2: Split the string by underscores or digits (to capitalize individual words)
            string[] words = cleanedInput.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Step 3: Apply PascalCase (capitalize each word)
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(words[i].ToLower());
            }

            // Step 4: Combine the words into a single valid field name
            string pascalCaseName = string.Join(string.Empty, words);

            // Step 5: Ensure the name doesn't start with a number (prepend an underscore if needed)
            if (char.IsDigit(pascalCaseName[0]))
            {
                pascalCaseName = "_" + pascalCaseName;
            }

            if (pascalCaseName == name)
            {
                pascalCaseName += "_";
            }

            // Step 6: Return the valid PascalCase C# field name
            return pascalCaseName;
        }
    }
}
