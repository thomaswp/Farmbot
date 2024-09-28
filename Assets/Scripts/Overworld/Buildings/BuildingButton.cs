using Fambot.Overworld.Buildings;
using Farmbot.Resources;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farmbot.Overworld.Buildings
{
    public class BuildingButton : MonoBehaviour
    {
        public GameObject buildPanel;
        public Building building;
        private ResourceManager resourceManager;

        // Use this for initialization
        void Start()
        {
            resourceManager = SingletonManager.GetSingleton<ResourceManager>();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClick();
            });
        }

        void OnClick()
        {
            if (building.TryToStartBuilding())
            {
                buildPanel.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}