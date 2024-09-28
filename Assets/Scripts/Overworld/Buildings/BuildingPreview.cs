using Fambot.Overworld.Buildings;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

namespace Farmbot.Overworld.Buildings
{
    public class BuildingPreview : MonoBehaviour
    {
        public const int tilePixelSize = 32;

        public Tilemap buildAvailability;
        public GameObject buildingPrefab;

        private Grid grid;
        private Building building;
        private Vector3Int lastPosition;
        private Vector3 centeringOffset;
        private BuildingObstructionsManager obstructionsManager;

        void Start()
        {
            Debug.Log("!!");
            grid = GetComponentInParent<Grid>();
            obstructionsManager = SingletonManager.GetSingleton<BuildingObstructionsManager>();
            UpdatePosition();
            Debug.Log(obstructionsManager);
        }

        public void StartPlacement(Building building)
        {
            Debug.Log("Starting placement");
            this.building = building;
            gameObject.SetActive(true);

            grid = GetComponentInParent<Grid>();
            var sprite = building.Image;
            GetComponentInChildren<SpriteRenderer>().sprite = sprite;
            var rect = sprite.rect;
            // Account for images that are smaller than their tile footprint
            centeringOffset = new Vector3(
                building.TileWidth - rect.width / tilePixelSize,
                building.TileHeight - rect.height / tilePixelSize,
                0) / 2;
        }

        void UpdatePosition()
        {
            if (building == null) return;
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.x -= building.TileWidth / 2;
            worldPos.y -= building.TileHeight / 2;
            var tilePos = grid.WorldToCell(worldPos);
            tilePos.z = 0;
            Vector3 newPos = tilePos + centeringOffset;
            transform.position = newPos;
            UpdateColors(tilePos);
        }

        private Color transparent = new Color(0, 0, 0, 0);
        void UpdateColors(Vector3Int cornerTile)
        {
            ColorTilesUnder(lastPosition, (Vector3Int pos) => transparent);
            lastPosition = cornerTile;
            if (!gameObject.activeSelf) return;
            ColorTilesUnder(cornerTile, (Vector3Int pos) =>
            {
                return obstructionsManager.IsObstructed(pos) ? Color.red : Color.green;
            });
        }

        private void ColorTilesUnder(Vector3Int corner, Func<Vector3Int, Color> colorFunc)
        {
            for (int i = 0; i < building.TileWidth; i++)
            {
                for (int j = 0; j < building.TileHeight; j++)
                {
                    Vector3Int tile = corner + new Vector3Int(i, j, 0);
                    buildAvailability.SetColor(tile, colorFunc(tile));
                }
            }
        }

        private Vector3Int GetTileWithOffset(Vector3Int centerTile, int offX, int offY)
        {
            int halfWidth = building.TileWidth / 2;
            int halfHeight = building.TileHeight / 2;
            return new Vector3Int(
                centerTile.x + offX - halfWidth,
                centerTile.y + offY - halfHeight,
                0
            );
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition();
            if (Input.GetMouseButtonDown(0))
            {
                TryPlace();
            }

        }

        private void TryPlace()
        {
            bool isObstructed = SingletonManager.GetSingleton<BuildingObstructionsManager>()
                    .IsObstructed(lastPosition, building.TileWidth, building.TileHeight);

            if (isObstructed)
            {
                // TODO: UI
                Debug.Log("Cannot place here!");
                return;
            }

            gameObject.SetActive(false);
            // Clear the last highlight
            UpdatePosition();
            // TODO: Check if allowed here

            // Top-level object has the Building component
            GameObject buildingObject = Instantiate(building.gameObject, buildingPrefab.transform.parent);
            // Child has the renderer and other reusable components
            GameObject buildingChild = Instantiate(buildingPrefab, buildingObject.transform);
            buildingObject.transform.position = transform.position;
            buildingChild.transform.SetParent(buildingObject.transform, false);

            buildingObject.SetActive(true);
            buildingChild.SetActive(true);
        }
    }
}