
using UnityEngine.UIElements;
namespace Assets.UI
{

[UxmlElement]
public partial class RequestsWindow : UIComponent.UIComponent
{
protected static class Classes
{
const string UnityTextElement = "unity-text-element";
const string UnityLabel = "unity-label";
const string UnityButton = "unity-button";
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

public Label Requestname { get; private set; }
public ResourceList Requirements { get; private set; }
public ResourceList Rewards { get; private set; }
public Button Turninbutton { get; private set; }
protected override void LoadTemplate()
{
if (template != null) return;
template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/RequestsWindow.uxml");
}
protected override void SetFields()
{
Requestname = this.Q<Label>("requestName");
Requirements = this.Q<ResourceList>("requirements");
Rewards = this.Q<ResourceList>("rewards");
Turninbutton = this.Q<Button>("turnInButton");
}
}}