using Farmbot.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.UI
{
    public partial class ResourceAmount : UIComponent.UIComponent
    {
        [UxmlAttribute]
        public Farmbot.Resources.ResourceAmount Amount { get; set; }

        protected override void OnTreeLoaded()
        {
            base.OnTreeLoaded();
            if (Amount == null)
            {
                return;
            }
            Fields.Name.text = Amount.resource.Info().name;
            Fields.Quantity.text = Amount.amount.ToString();
        }
    }
}
