
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

        public GameObject startingTarget;

        private int tick = 0;


        // Start is called before the first frame update
        private void Start()
        {
            Application.runInBackground = true;

            JsonMessage blocksJSON = BlocklyGenerator.GenerateBlocks();

            websocket = WebsocketServer.Start(url, port);
            WebsocketServer.OnMessage += WebsocketServer_OnMessage;
            WebsocketServer.OnConnected += () =>
            {
                WebsocketServer.SendMessage(blocksJSON);

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SyncCode();
                    if (startingTarget)
                    {
                        startingTarget.GetComponent<Interpreter>().SetTarget();
                    }
                });
            };
        }

        private void SyncCode()
        {
            // TODO: Should probably only send relevant data
            WebsocketServer.SendMessage(new JsonMessage("SyncCode", GameState.Instance.Robots));
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
            UnityMainThreadDispatcher.Instance().Enqueue(() => HandleMessage(data));
        }

        private static void HandleMessage(JObject message)
        {
            string type = (string)message["type"];
            JObject data = (JObject)message["data"];
            switch (type)
            {
                case "call": RunMethod(data); break;
                case "save": SaveCode(data); break;
                default: Debug.LogWarning("Unknown type: " + type); break;
            }
        }

        private static void SaveCode(JObject data)
        {
            string targetID = (string)data["targetID"];
            string codeXML = (string)data["code"];
            GameState.Instance.GetRobot(targetID).Code = codeXML;
            GameState.SaveJSON();
        }

        private static void RunMethod(JObject data)
        {
            string methodName = (string)data["methodName"];
            string threadID = (string)data["threadID"];
            string targetID = (string)data["targetID"];
            JArray argsArray = (JArray)data["args"];
            object[] args = argsArray.ToObject<object[]>();

            Interpreter interpreter = Interpreter.GetInterpreter(targetID);
            if (interpreter == null)
            {
                Debug.LogWarning("Missing interpreter for targetID: " + targetID);
                return;
            }
            GameObject target = interpreter.gameObject;

            var method = BlocklyGenerator.Call(target, methodName, args);
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