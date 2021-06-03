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
    }

    public class ScriptableMethod : Scriptable
    {
    }

    public class ScriptableProperty : Scriptable
    {
    }

}