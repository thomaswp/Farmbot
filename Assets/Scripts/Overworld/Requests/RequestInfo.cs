using Farmbot.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Farmbot.Data.Requests
{
    public class RequestInfo : ScriptableObject
    {
        [SerializeField]
        public ResourceAmount[] requestedResources;
        [SerializeField]
        public ResourceAmount[] rewards;
        [SerializeField]
        public int experienceReward;
    }
}
