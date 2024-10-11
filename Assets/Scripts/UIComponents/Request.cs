using Farmbot.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.UI
{
    public partial class Request : UIComponent.UIComponent
    {
        [UxmlAttribute]
        public RequestInfo RequestInfo { get; set; }

        protected override void OnTreeLoaded()
        {
            base.OnTreeLoaded();
            if (RequestInfo == null) return;
            Fields.Requirements.resourceAmounts = RequestInfo.requestedResources;
            Fields.Rewards.resourceAmounts = RequestInfo.rewards;
        }
    }
}
