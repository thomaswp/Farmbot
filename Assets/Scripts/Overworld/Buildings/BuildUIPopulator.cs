using Fambot.Overworld.Buildings;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farmbot.Overworld.Buildings
{
    public class BuildUIPopulator : MonoBehaviour
    {
        public GameObject buildingButtonPrefab;

        // Use this for initialization
        void Start()
        {
            var buildings = SingletonManager.GetSingleton<BuildingsManager>().BuildingPrefabs;
            foreach (Building building in buildings)
            {
                var button = Instantiate(buildingButtonPrefab);
                button.GetComponent<Image>().sprite = building.Image;
                button.transform.SetParent(buildingButtonPrefab.transform.parent);
                button.GetComponent<BuildingButton>().building = building;
            }

            buildingButtonPrefab.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}