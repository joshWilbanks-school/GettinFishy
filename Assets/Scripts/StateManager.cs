using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    [SerializeField] Dictionary<string, bool> unlockables = new();
    [SerializeField] public FishingRod equipedRod;
    [SerializeField] public Hook equipedHook;
    [SerializeField] public Bait equipedBait;

    private void Awake()
    {
        if(instance == null)
            instance = this;

    }



    public bool IsUnlocked(string name)
    {
        return unlockables.GetValueOrDefault(name);
    }

    public void SetUnlockedState(string name, bool value)
    {
        unlockables[name] = value;
    }

    public Dictionary<string, bool> GetCopy()
    {
        Dictionary<string, bool> copy = new Dictionary<string, bool>();
        foreach(KeyValuePair<string, bool> kvp in unlockables)
            copy[kvp.Key] = kvp.Value;

        return copy;
    }

}
