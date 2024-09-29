
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Farmbot.Resources
{
    public enum Resource
    {
        Gold,
        Wheat,
        Wood,
    }

    public struct ResourceInfo
    {
        public Resource resource;
        public string name;
        public bool showInPanel;

        public static readonly ResourceInfo[] All = new ResourceInfo[]
        {
            new ResourceInfo() {
                resource = Resource.Gold,
                name = "Gold",
                showInPanel = true,
            },
            new ResourceInfo() {
                resource = Resource.Wheat,
                name = "Wheat",
                showInPanel = false,
            },
            new ResourceInfo() {
                resource = Resource.Wood,
                name = "Wood",
                showInPanel = false,
            },
        };

        private static Dictionary<Resource, ResourceInfo> map = new Dictionary<Resource, ResourceInfo>();
        static ResourceInfo()
        {
            foreach (var item in All)
            {
                map.Add(item.resource, item);
            }
        }

        public static ResourceInfo Get(Resource resouce)
        {
            return map[resouce];
        }

    }

    public static class ResourceExtensions
    {
        public static ResourceInfo Info(this Resource resource)
        {
            return ResourceInfo.Get(resource);
        }
    }

}