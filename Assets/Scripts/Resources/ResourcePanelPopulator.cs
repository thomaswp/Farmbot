using Farmbot.Overworld.Buildings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Farmbot.Resources
{
    public class ResourcePanelPopulator : MonoBehaviour
    {
        public GameObject resourcePanelPrefab;

        // Use this for initialization
        void Start()
        {
            foreach (Resource resource in System.Enum.GetValues(typeof(Resource)))
            {
                GameObject resourcePanel = Instantiate(resourcePanelPrefab, transform);
                resourcePanel.GetComponent<ResourceAmountPanel>().resource = resource;
                resourcePanel.transform.SetParent(resourcePanelPrefab.transform.parent);
            }
            resourcePanelPrefab.SetActive(false);
        }
    }
}