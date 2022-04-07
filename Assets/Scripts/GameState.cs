using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Farmbot
{
    [Serializable]
    public class GameState 
    {
        private const string FILE = "data.json";
        private const int VERSION = 1;

        private static GameState _instance;
        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadJSON();
                }
                return _instance;
            }
        }

        public static GameState LoadJSON()
        {
            if (!File.Exists(FILE)) return new GameState();
            string json = File.ReadAllText(FILE);
            return JsonUtility.FromJson<GameState>(json);
        }

        public static void SaveJSON()
        {
            string json = JsonUtility.ToJson(Instance);
            Debug.Log(Instance.Robots.Count);
            Debug.Log(json);
            File.WriteAllText(FILE, json);
        }

        public List<Robot> Robots = new List<Robot>();
        public int Version = VERSION;

        public Robot GetRobot(string guid)
        {
            Robot r = Robots.Where(r => r.Guid == guid).FirstOrDefault();
            if (r != null) return r;
            r = new Robot() { Guid = guid };
            Robots.Add(r);
            return r;
        }
    }

    [Serializable]
    public class Robot
    {
        public string Guid;
        public string Code;
    }
}
