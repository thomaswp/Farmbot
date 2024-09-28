using Fambot.Overworld.Buildings;
using Farmbot.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farmbot.Overworld.Buildings
{
    public class BuildingObstructionsManager : MonoBehaviour
    {
        // One day may need to track multiple obstructions at a time
        HashSet<Vector2Int> obstructed = new HashSet<Vector2Int>();

        public void MarkObstracted(Vector3Int bottomRightCorner, int cellWidth, int cellHeight, bool isObstructed)
        {
            for (int x = 0; x < cellWidth; x++)
            {
                for (int y = 0; y < cellHeight; y++)
                {
                    Vector2Int position = new Vector2Int(bottomRightCorner.x + x, bottomRightCorner.y + y);
                    if (isObstructed)
                    {
                        obstructed.Add(position);
                    }
                    else
                    {
                        obstructed.Remove(position);
                    }
                }
            }
        }

        public bool IsObstructed(Vector3Int position, int cellWidth = 1, int cellHeight = 1)
        {
            for (int x = 0; x < cellWidth; x++)
            {
                for (int y = 0; y < cellHeight; y++)
                {
                    Vector2Int cellPosition = new Vector2Int(position.x + x, position.y + y);
                    if (obstructed.Contains(cellPosition))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}