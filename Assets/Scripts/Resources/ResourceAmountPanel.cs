using Farmbot.Overworld.Buildings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Farmbot.Resources
{
    public class ResourceAmountPanel : MonoBehaviour
    {
        public Resource resource;

        private ResourceManager resources;

        void Start()
        {
            resources = SingletonManager.GetSingleton<ResourceManager>();
            resources.resources.OnChanged += OnChange;
            OnChange();
        }

        private void OnChange()
        {
            transform.GetComponentInChildren<Text>().text = $"{resource.Info().name}: {resources.resources[resource]}";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}