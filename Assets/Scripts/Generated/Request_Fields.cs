
using UnityEditor;
using UnityEngine.UIElements;
namespace Assets.UI
{

[UxmlElement]
public partial class Request : UIComponent.UIComponent
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
const string UnityButton = "unity-button";
}
protected class RequestFields
{
public Label RequestName { get; set; }
public ResourceList Requirements { get; set; }
public ResourceList Rewards { get; set; }
public Button TurnInButton { get; set; }
}
protected RequestFields Fields { get; private set; } = new RequestFields();
protected override void LoadTemplate()
{
if (template != null) return;
template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Request.uxml");
}
protected override void SetFields()
{
Fields.RequestName = this.Q<Label>("request-name");
Fields.Requirements = this.Q<ResourceList>("requirements");
Fields.Rewards = this.Q<ResourceList>("rewards");
Fields.TurnInButton = this.Q<Button>("turn-in-button");
}
}}