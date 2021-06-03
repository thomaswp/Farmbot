
using UnityEngine;

namespace Farmbot
{
    public class BlocklyBridge : MonoBehaviour
    {
        public string url;
        public int port;
        public int runSpeed = 10;
        WebsocketServer websocket;

        private int tick = 0;


        // Start is called before the first frame update
        private void Start()
        {
            BlocklyGenerator.GenerateBlocks();
            websocket = WebsocketServer.Start(url, port);
        }


        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (tick++ % runSpeed == 0)
            {
                WebsocketServer.SendMessage("stepCode");
            }

        }

        void OnApplicationQuit()
        {
        }
    }

}