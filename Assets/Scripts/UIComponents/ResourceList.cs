using Assets.UIComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI
{
    public partial class ResourceList : UIComponent.UIComponent
    {

        [UxmlAttribute]
        public string title { get; set; }

        [UxmlAttribute]
        public Farmbot.Resources.ResourceAmount[] resourceAmounts { get; set; }

        public ResourceList()
        {
        }

        protected override void OnTreeLoaded()
        {
            Fields.Title.text = title;
            if (resourceAmounts == null)
            {
                return;
            }
            Fields.Resource.visible = false;
            foreach (var amount in resourceAmounts)
            {
                var panel = new ResourceAmount();
                panel.Amount = amount;
                panel.template = Fields.Resource.template;
                Fields.ResourceList.Add(panel);
            }
        }
    }
}
