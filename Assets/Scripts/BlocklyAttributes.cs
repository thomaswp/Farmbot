using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmbot
{
    public class Scriptable : Attribute
    {

    }

    public class ScriptableBehavior : Scriptable
    {
        public readonly string name;
        public readonly int color;
        public ScriptableBehavior(string name, int color)
        {
            this.name = name;
            this.color = color;
        }
    }

    public class ScriptableMethod : Scriptable
    {
    }

    public class ScriptableProperty : Scriptable
    {
    }

    public class ScriptableEvent : Scriptable
    {
    }

}