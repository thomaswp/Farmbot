using Farmbot.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Farmbot.Overworld.Buildings
{
    public class Building : MonoBehaviour
    {
        public string Name;
        public string Description;
        public ResourceAmount[] Cost;
        public TimeSpan BuildTime;
        public int TileWidth;
        public int TileHeight;
        public Sprite Image;


        public void Awake()
        {
            
        }

        public bool TryToStartBuilding()
        {
            ResourceSet cost = Cost;
            var resources = SingletonManager.GetSingleton<ResourceManager>().resources;
            if (!resources.Contains(cost))
            {
                var missing = resources.MissingResources(cost);
                ShowMissingResources(missing);
                return false;
            }
            resources.Remove(cost);

            return true;
        }

        private void ShowMissingResources(ResourceSet missing)
        {
            Debug.Log($"Missing resources: {missing}");
        }
    }
}
