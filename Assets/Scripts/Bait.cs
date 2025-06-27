using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour, INameable
{
    [SerializeField] Size size;
    [SerializeField] Vector2 scale;
    [SerializeField] string itemName;

    public void SetScale()
    {
        transform.localScale = new Vector3(scale.x, scale.y, transform.localScale.z);
    }

    public Size GetSize()
    {
        return size;
    }


    public string GetName()
    {
        return itemName;
    }

    public void SetName(string name)
    {
        this.itemName = name;
    }
}
