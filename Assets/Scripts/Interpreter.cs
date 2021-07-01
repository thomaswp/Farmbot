using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Farmbot
{
    public class Interpreter : MonoBehaviour, IPointerClickHandler
    {
        private static Dictionary<int, Interpreter> interpreterMap = new Dictionary<int, Interpreter>();

        public static Interpreter GetInterpreter(int instanceID)
        {
            if (!interpreterMap.ContainsKey(instanceID))
                return null;
            return interpreterMap[instanceID];
        }

        private List<AsyncMethod> executingMethods = new List<AsyncMethod>();

        public void Start()
        {
            interpreterMap.Add(gameObject.GetInstanceID(), this);
        }

        public T ExecuteMethod<T>(T method) where T : AsyncMethod
        {
            executingMethods.Add(method);
            return method;
        }

        public AsyncMethod ExecuteMethod(string category)
        {
            return ExecuteMethod(new AsyncMethod().SetBlockingCategory(category));
        }

        public AsyncMethod ExecuteMethod()
        {
            return ExecuteMethod(null);
        }

        public int CountCategory(string category)
        {
            return executingMethods.Where(m => m.BlockingCategory == category).Count();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Setting target to: " + gameObject.name);
                SetTarget();
            }
        }

        public void SetTarget()
        {
            WebsocketServer.SendMessage(new JsonMessage("SetTarget", new
            {
                targetID = gameObject.GetInstanceID(),
                targetName = gameObject.name,
                // TODO: add code
            }));
        }

        void Update()
        {

            HashSet<string> blockingCategories = new HashSet<string>();
            for (int i = 0; i < executingMethods.Count; i++)
            {
                AsyncMethod method = executingMethods[i];
                if (blockingCategories.Contains(method.BlockingCategory)) continue;
                if (method.Update())
                {
                    executingMethods.RemoveAt(i--);
                } else if (method.BlockingCategory != null)
                {
                    blockingCategories.Add(method.BlockingCategory);
                }
            }
        }
    }
}
