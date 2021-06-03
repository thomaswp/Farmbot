using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farmbot
{
    public class CameraControl : MonoBehaviour
    {
        public GameObject player;
        public float threshold = 3;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 target = transform.position;
            Vector3 focus = player.transform.position;
            focus.z = target.z;
            target.x = Mathf.Min(Mathf.Max(target.x, focus.x - threshold), focus.x + threshold);
            target.y = Mathf.Min(Mathf.Max(target.y, focus.y - threshold), focus.y + threshold);
            transform.position = Vector3.Lerp(transform.position, target, 0.01f);
        }
    }

}