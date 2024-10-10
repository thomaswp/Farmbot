using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.UIComponent
{
    public abstract class UIComponent : VisualElement
    {
        public UIComponent()
        {
#if UNITY_EDITOR
            RegisterCallback<AttachToPanelEvent>(e => LoadTemplate());
#endif
        }

        protected abstract void SetFields();
        protected abstract void LoadTemplate();
        protected virtual void OnTreeLoaded() { }
    }

}
