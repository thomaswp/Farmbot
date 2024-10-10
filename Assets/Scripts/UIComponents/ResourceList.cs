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

        public ResourceList()
        {
        }

        protected override void OnTreeLoaded()
        {
            Title.text = title;
        }
    }
}
