
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
            
            int targetID = target.GetInstanceID();
            string targetName = target.name;

            websocket = WebsocketServer.Start(url, port);
            WebsocketServer.OnConnected += () =>
            {
                WebsocketServer.SendMessage(blocksJSON);
                WebsocketServer.OnMessage += WebsocketServer_OnMessage;

                WebsocketServer.SendMessage(new JsonMessage("SetTarget", new { 
                    targetID = targetID,
                    targetName = targetName,
                    // TODO: add code
                }));
            };
        }

        private void WebsocketServer_OnMessage(string dataString)
        {
            Debug.Log("Receiving: " + dataString);
            JObject data = null;
            try
            {
                data = JObject.Parse(dataString);
            }
            catch { }
            if (data == null)
            {
                Debug.Log("Cannot parse message!");
                return;
            }
            string methodName = (string)data["methodName"];
            string threadID = (string)data["threadID"];
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                // TODO: Get and use target
                var method = BlocklyGenerator.Call(target, methodName);
                object returnValue = method == null ? null : method.GetReturnValue();
                Action onFinished = () =>
                {
                    WebsocketServer.SendMessage(new JsonMessage("BlockFinished", new
                    {
                        targetID = target.GetInstanceID(),
                        threadID = threadID,
                        returnValue = returnValue,
                    }));
                };
                if (method == null)
                {
                    onFinished();
                }
                else
                {
                    method.Do(onFinished);
                }
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