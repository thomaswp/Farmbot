using Farmbot.Resources;
using Farmbot.Overworld.Buildings;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farmbot.Overworld.Buildings
{
    public class ProductionBuilding : MonoBehaviour
    {

        public Resource resource;
        public float productionTimeSeconds;
        public int productionAmount = 1;

        private float timeUntilComplete;
        private TextMesh timer;

        // Use this for initialization
        void Start()
        {
            timeUntilComplete = productionTimeSeconds;

            // TODO: UI is probably better for scaling and resolution
            timer = gameObject.AddComponent<TextMesh>();
            timer.text = "";
            timer.fontSize = 36;
            timer.characterSize = 0.25f;
        }

        private void OnMouseDown()
        {
            Debug.Log("Mouse Click!");
            if (timeUntilComplete <= 0)
            {
                this.GetSingleton<ResourceManager>().resources.Add(resource, productionAmount);
                timeUntilComplete = productionTimeSeconds;
            }
        }


        // Update is called once per frame
        void Update()
        {
            timeUntilComplete -= Time.deltaTime;
            if (timeUntilComplete > 0)
            {
                timer.text = SecondsToString(timeUntilComplete);
            }
            else
            {
                timer.text = "Ready!";
            }
        }

        private static string SecondsToString(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }
    }
}