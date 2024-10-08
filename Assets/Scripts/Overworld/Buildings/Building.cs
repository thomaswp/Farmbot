﻿using Fambot.Overworld.Buildings;
using Farmbot.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Farmbot.Overworld.Buildings
{
    [CreateAssetMenu(fileName = "BuildingInfo", menuName = "BuildingInfo")]
    public class BuildingInfoTest : ScriptableObject
    {
        public string Name;
        public string Description;
        public ResourceAmount[] Cost;
    }

    public class Building : MonoBehaviour
    {
        public string Name;
        public string Description;
        public ResourceAmount[] Cost;
        public TimeSpan BuildTime;
        public int TileWidth;
        public int TileHeight;
        public Sprite Image;

        public void Start()
        {
            Debug.Log(transform.GetChild(0).name);
            GetComponentInChildren<SpriteRenderer>(true).sprite = Image;
            Vector3Int position = Vector3Int.FloorToInt(transform.position);
            SingletonManager.GetSingleton<BuildingObstructionsManager>()
                .MarkObstracted(position, TileWidth, TileHeight, true);


            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            collider.size = spriteRenderer.size;
            collider.offset = spriteRenderer.size / 2;
        }

        public bool TryToStartBuilding()
        {
            BuildingsManager buildingsManager = SingletonManager.GetSingleton<BuildingsManager>();
            ResourceSet cost = Cost;
            var resources = SingletonManager.GetSingleton<ResourceManager>().resources;
            if (!resources.Contains(cost))
            {
                var missing = resources.MissingResources(cost);
                ShowMissingResources(missing);
                return false;
            }
            resources.Remove(cost);
            buildingsManager.StartPreviewBuilding(this);

            return true;
        }

        private void ShowMissingResources(ResourceSet missing)
        {
            Debug.Log($"Missing resources: {missing}");
        }
    }
}
