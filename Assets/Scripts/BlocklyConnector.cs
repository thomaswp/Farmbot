
using BlocklyBridge;
using System;
using UnityEngine;

namespace Farmbot
{
    public class BlocklyConnector : MonoBehaviour
    {
        public string url;
        public int port;

        public GameObject startingTarget;

        // TODO: Don't use singleton...
        public static Dispatcher Dispatcher;

        private class UnityLogger : BlocklyBridge.ILogger
        {
            public void Log(object message)
            {
                Debug.Log(message);
            }

            public void Warn(object message)
            {
                Debug.LogWarning(message);
            }
        }

        private void Awake()
        {
            Application.runInBackground = true;
            BlocklyBridge.Logger.Implementation = new UnityLogger();

            Dispatcher = new Dispatcher(url, port, 
                action => UnityMainThreadDispatcher.Instance().Enqueue(action));

            Dispatcher.State = GameState.Instance.ProgramState;

            Dispatcher.OnSave += Dispatcher_OnSave;

            Type[] types = new Type[]
            {
                typeof(ActorActions)
            };

            Dispatcher.Start(types, () =>
            {
                if (startingTarget)
                {
                    Dispatcher.SetTarget(startingTarget.GetComponent<Interpreter>());
                }
            });
        }

        private void Dispatcher_OnSave(ProgramState obj)
        {
            GameState.Instance.ProgramState = obj;
            GameState.SaveJSON();
        }
    }
}