
using UnityEditor;
using UnityEngine.UIElements;
namespace Assets.UI
{

[UxmlElement]
public partial class ResourceList : UIComponent.UIComponent
{

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

protected static class Classes
{
const string UnityTextElement = "unity-text-element";
const string UnityLabel = "unity-label";
const string Resources = "resources";
}
protected class ResourceListFields
{
public Label Title { get; set; }
public VisualElement ResourceList { get; set; }
public ResourceAmount Resource { get; set; }
}
protected ResourceListFields Fields { get; private set; } = new ResourceListFields();
protected override void LoadTemplate()
{
if (template != null) return;
template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/ResourceList.uxml");
}
protected override void SetFields()
{
Fields.Title = this.Q<Label>("title");
Fields.ResourceList = this.Q<VisualElement>("resource-list");
Fields.Resource = this.Q<ResourceAmount>("resource");
}
}}