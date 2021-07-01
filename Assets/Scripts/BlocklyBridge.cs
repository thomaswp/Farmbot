
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

        private int tick = 0;


        // Start is called before the first frame update
        private void Start()
        {
            Application.runInBackground = true;

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
            UnityMainThreadDispatcher.Instance().Enqueue(() => RunMethod(data));
        }

        private static void RunMethod(JObject data)
        {
            string methodName = (string)data["methodName"];
            string threadID = (string)data["threadID"];
            int targetID = (int)data["targetID"];

            Interpreter interpreter = Interpreter.GetInterpreter(targetID);
            if (interpreter == null)
            {
                Debug.LogWarning("Missing interpreter for targetID: " + targetID);
                return;
            }
            GameObject target = interpreter.gameObject;

            var method = BlocklyGenerator.Call(target, methodName);
            object returnValue = method == null ? null : method.GetReturnValue();
            Action onFinished = () =>
            {
                WebsocketServer.SendMessage(new JsonMessage("BlockFinished", new
                {
                    targetID = targetID,
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