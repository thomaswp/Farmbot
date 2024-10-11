
using UnityEditor;
using UnityEngine.UIElements;
namespace Assets.UI
{

[UxmlElement]
public partial class RequestsWindow : UIComponent.UIComponent
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
}
protected class RequestsWindowFields
{
public VisualElement RequestsList { get; set; }
public Request Request { get; set; }
}
protected RequestsWindowFields Fields { get; private set; } = new RequestsWindowFields();
protected override void LoadTemplate()
{
if (template != null) return;
template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/RequestsWindow.uxml");
}
protected override void SetFields()
{
Fields.RequestsList = this.Q<VisualElement>("requests-list");
Fields.Request = this.Q<Request>("request");
}
}}