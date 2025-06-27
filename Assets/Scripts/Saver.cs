using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Saver : MonoBehaviour
{
    [SerializeField] GameManager manager;
    [SerializeField] StateManager stateManager;
    [SerializeField] bool save;

    [Serializable]
    struct JsonVersion
    {
        public string name;
        public bool val;

        public JsonVersion(string name, bool val)
        {
            this.name = name;
            this.val = val;
        }
    }

    public void Save()
    {
        Dictionary<string, bool> unlocks = stateManager.GetCopy();
        List<(string, bool)> jVersion = new();
        foreach (KeyValuePair<string, bool> kvp in unlocks)
            jVersion.Add((kvp.Key, kvp.Value));

        string unlocksStr = JsonUtility.ToJson(unlocks.Keys.ToArray(), true);
    }

    private void OnValidate()
    {
    }
}
