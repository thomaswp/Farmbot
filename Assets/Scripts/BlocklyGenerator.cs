using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace Farmbot
{
    public class BlocklyGenerator
    {
        public static void GenerateBlocks()
        {
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == "Farmbot"
                    select t;
            var scriptables = q.Where(t => Attribute.IsDefined(t, typeof(Scriptable)));

            List<BlocklyMethod> methodDefinitions = new List<BlocklyMethod>();
            foreach (Type scriptable in scriptables)
            {
                string category = scriptable.Name;
                var methods = scriptable.GetMethods()
                    .Where(method => Attribute.IsDefined(method, typeof(ScriptableMethod)));
                foreach (MethodInfo method in methods)
                {
                    BlocklyMethod blocklyMethod = new BlocklyMethod(method.Name, category);
                    if (!typeof(AsyncMethod).IsAssignableFrom(method.ReturnType))
                    {
                        blocklyMethod.returnType = new BlocklyType(method.ReturnType);
                    } 
                    else if (method.ReturnType.IsGenericType && 
                        typeof(AsyncFunction<>).IsAssignableFrom(method.ReturnType.GetGenericTypeDefinition()))
                    {
                        blocklyMethod.returnType = new BlocklyType(method.ReturnType.GetGenericArguments()[0]);
                    }
                    if (blocklyMethod.returnType != null && blocklyMethod.returnType.type == null)
                    {
                        Debug.LogWarning("Unknown return for method: " + method.Name);
                    }

                    foreach (var param in method.GetParameters())
                    {
                        blocklyMethod.parameters.Add(new BlocklyParameter(param.Name, new BlocklyType(param.ParameterType)));
                    }
                    //Debug.Log(scriptable.Name + "." + method.Name);
                    methodDefinitions.Add(blocklyMethod);
                }
            }

            Debug.Log(JsonConvert.SerializeObject(methodDefinitions));
        }
    }

    [Serializable]
    class BlocklyMethod
    {
        public string name;
        public string category;
        public BlocklyType returnType;
        public List<BlocklyParameter> parameters = new List<BlocklyParameter>();

        public BlocklyMethod(string name, string category)
        {
            this.name = name;
            this.category = category;
        }
    }

    [Serializable]
    class BlocklyParameter
    {
        public BlocklyType type;
        public string name;

        public BlocklyParameter(string name, BlocklyType type)
        {
            this.name = name;
            this.type = type;
        }
    }

    [Serializable]
    class BlocklyType
    {
        public string type;
        public bool isEnum;
        public string[] options;

        public BlocklyType(Type type)
        {
            this.type = type.Name;

            if (type.IsPrimitive)
            {
                isEnum = false;
                options = null;
            }
            else if (type.IsEnum)
            {
                isEnum = true;
                options = type.GetEnumNames();
            }
            else
            {
                Debug.LogWarning("Unknown type: " + type.Name);
                this.type = null;
                isEnum = false;
                options = null;
            }
        }
    }

    public class AsyncMethod
    {
        protected List<Func<bool>> todo = new List<Func<bool>>();

        public AsyncMethod UpdateUntil(Func<bool> until)
        {
            todo.Add(until);
            return this;
        }

        public AsyncMethod Do(Action action)
        {
            todo.Add(() =>
            {
                action();
                return true;
            });
            return this;
        }

        public bool Update()
        {
            while (todo.Count > 0 && todo[0]()) todo.RemoveAt(0);
            return todo.Count == 0;
        }
    }

    public class AsyncFunction<T> : AsyncMethod
    {
        private T value;

        public T GetReturnValue()
        {
            return value;
        }

        protected void SetReturnValue(T value)
        {
            this.value = value;
        }

        public AsyncFunction<T> Return(Func<T> returner)
        {
            todo.Add(() =>
            {
                value = returner();
                return true;
            });
            return this;
        }
    }

}