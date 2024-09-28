using Farmbot.Overworld.Buildings;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Fambot.Overworld.Buildings
{
    public class BuildingsManager : MonoBehaviour
    {
        public GameObject buildingsPrefabsParent;
        public GameObject buildingsPreview;
        
        private List<Building> buildingPrefabs = new List<Building>();
        public ReadOnlyCollection<Building> BuildingPrefabs { get; private set; }


        // Use this for initialization
        void Awake()
        {
            for (int i = 0; i < buildingsPrefabsParent.transform.childCount; i++)
            {
                var prefab = buildingsPrefabsParent.transform.GetChild(i).GetComponent<Building>();
                buildingPrefabs.Add(prefab);
            }
            BuildingPrefabs = buildingPrefabs.AsReadOnly(); ;
        }

        public void StartPreviewBuilding(Building building)
        {
            buildingsPreview.GetComponent<BuildingPreview>().StartPlacement(building);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}