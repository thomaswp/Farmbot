using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farmbot.Resources
{

    public enum Resource
    {
        Gold
    }

    public class ResourceManager : MonoBehaviour
    {
        public ResourceAmount[] startingResources;

        public readonly ResourceSet resources = new ResourceSet();

        // Use this for initialization
        void Start()
        {
            resources.Add(startingResources);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}