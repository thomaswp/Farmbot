using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using BlocklyBridge;

namespace Farmbot
{
    public class Interpreter : MonoBehaviour, IPointerClickHandler, IProgrammable
    {
        public string Guid = System.Guid.NewGuid().ToString();

        public string GetGuid()
        {
            return Guid;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public object GetObjectForType(Type type)
        {
            return gameObject.GetComponent(type);
        }

        private List<AsyncMethod> executingMethods = new List<AsyncMethod>();

        public void Start()
        {
            BlocklyConnector.Dispatcher.Register(this);
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
            BlocklyConnector.Dispatcher.SetTarget(this);
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
