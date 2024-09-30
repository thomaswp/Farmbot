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

        private MethodQueue methodQueue = new MethodQueue();

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

        public int CountCategory(string category)
        {
            return methodQueue.CountCategory(category);
        }

        public void Start()
        {
            Debug.Log("Registering " + Guid);
            BlocklyConnector.Dispatcher.Register(this);
        }

        public AsyncMethod ExecuteMethod(string category)
        {
            AsyncMethod method = new AsyncMethod().SetBlockingCategory(category);
            EnqueueMethod(method);
            return method;
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
            methodQueue.Update();
        }

        public void EnqueueMethod(AsyncMethod method)
        {
            methodQueue.Enqueue(method);
        }

        public bool TryTestCode()
        {
            Debug.Log("Testing...");
            return true;
        }
    }
}
