
using UnityEngine;

namespace Farmbot
{
    public class BlocklyBridge : MonoBehaviour
    {
        public string url;
        public int port;
        public int runSpeed = 10;
        WebsocketServer websocket;

        public GameObject target;
        // TODO: Have a way to invoke all this
        private ActorActions eventRunner;

        private int tick = 0;


        // Start is called before the first frame update
        private void Start()
        {
            Application.runInBackground = true;
            eventRunner = (ActorActions)target.GetComponent(typeof(ActorActions));


            JsonMessage blocksJSON = BlocklyGenerator.GenerateBlocks();

            websocket = WebsocketServer.Start(url, port);
            WebsocketServer.OnConnected += () =>
            {
                WebsocketServer.SendMessage(blocksJSON);
                WebsocketServer.OnMessage += WebsocketServer_OnMessage;
            };
        }

        private void WebsocketServer_OnMessage(string data)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var method = BlocklyGenerator.Call(target, data);
                if (method == null) return;
                eventRunner.ExecuteMethod(method);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (tick++ % runSpeed == 0)
            {
                //WebsocketServer.SendMessage("stepCode");
            }

        }

        void OnApplicationQuit()
        {
        }
    }

}