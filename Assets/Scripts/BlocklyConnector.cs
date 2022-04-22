
using BlocklyBridge;
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

        private void Start()
        {
            Application.runInBackground = true;
            BlocklyBridge.Logger.Implementation = new UnityLogger();

            Dispatcher = new Dispatcher(url, port, 
                action => UnityMainThreadDispatcher.Instance().Enqueue(action));

            Dispatcher.State = GameState.Instance.ProgramState;

            Dispatcher.OnSave += Dispatcher_OnSave;

            Dispatcher.Start(() =>
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