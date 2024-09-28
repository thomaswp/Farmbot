using Farmbot.Overworld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Farmbot.Resources
{
    [Serializable]
    public class ResourceAmount
    {
        [SerializeField]
        public Resource resource;
        [SerializeField]
        public int amount;
    }

    public class ResourceSet
    {
        private Dictionary<Resource, int> resources = new Dictionary<Resource, int>();

        public event Action OnChanged;

        public int this[Resource resource]
        {
            get
            {
                if (!resources.ContainsKey(resource))
                {
                    resources[resource] = 0;
                }
                return resources[resource];
            }
            set
            {
                resources[resource] = value;
            }
        }

        public void Add(ResourceSet resources)
        {
            foreach (Resource resource in resources.resources.Keys)
            {
                this[resource] += resources[resource];
            }
            OnChanged();
        }

        public void Remove(ResourceSet resources)
        {
            foreach (Resource resource in resources.resources.Keys)
            {
                if (this[resource] < resources.resources[resource])
                {
                    throw new Exception($"Insufficient resources of type {resource}.");
                }
                this[resource] -= resources[resource];
            }
            OnChanged();
        }

        public bool Contains(ResourceSet resources)
        {
            foreach (Resource resource in resources.resources.Keys)
            {
                if (this[resource] < resources.resources[resource])
                {
                    return false;
                }
            }
            return true;
        }

        public ResourceSet MissingResources(ResourceSet cost)
        {
            ResourceSet missing = new ResourceSet();
            foreach (Resource resource in cost.resources.Keys)
            {
                if (this[resource] < cost[resource])
                {
                    missing[resource] = cost[resource] - this[resource];
                }
            }
            return missing;
        }

        public static implicit operator ResourceSet(ResourceAmount[] resourceAmounts)
        {
            ResourceSet resourceSet = new ResourceSet();
            foreach (ResourceAmount resourceAmount in resourceAmounts)
            {
                resourceSet[resourceAmount.resource] = resourceAmount.amount;
            }
            return resourceSet;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Resource resource in resources.Keys)
            {
                if (resources[resource] == 0) continue;
                sb.Append($"{resource}: {resources[resource]}; ");
            }
            return sb.ToString();
        }
    }
}
