
using UnityEditor;
using UnityEngine.UIElements;
namespace Assets.UI
{

[UxmlElement]
public partial class ResourceAmount : UIComponent.UIComponent
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
const string ResourceName = "resource-name";
}
protected class ResourceAmountFields
{
public Label Name { get; set; }
public Label Quantity { get; set; }
}
protected ResourceAmountFields Fields { get; private set; } = new ResourceAmountFields();
protected override void LoadTemplate()
{
if (template != null) return;
template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/ResourceAmount.uxml");
}
protected override void SetFields()
{
Fields.Name = this.Q<Label>("name");
Fields.Quantity = this.Q<Label>("quantity");
}
}}