
using UnityEngine.UIElements;
namespace Assets.UI
{

[UxmlElement]
public partial class ResourceList : UIComponent.UIComponent
{
protected static class Classes
{
const string UnityTextElement = "unity-text-element";
const string UnityLabel = "unity-label";
const string Resources = "resources";
const string ResourceName = "resource-name";
}

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

public Label Title { get; private set; }
public VisualElement ResourceList_ { get; private set; }
public VisualElement Resource { get; private set; }
public Label Name { get; private set; }
public Label Quantity { get; private set; }
protected override void LoadTemplate()
{
if (template != null) return;
template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/ResourceList.uxml");
}
protected override void SetFields()
{
Title = this.Q<Label>("title");
ResourceList_ = this.Q<VisualElement>("resource-list");
Resource = this.Q<VisualElement>("resource");
Name = this.Q<Label>("name");
Quantity = this.Q<Label>("quantity");
}
}}