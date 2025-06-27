using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;
    [SerializeField] GameObject ui;
    [SerializeField] Color affordable, notAffordable, equiped;


    public void Awake()
    {
        if(instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (Settings.Paused)
            return;

        if (Input.GetKeyDown(Settings.Shop) || (Input.GetKeyDown(KeyCode.Escape) && ui.activeSelf))
        {
            ui.SetActive(!ui.activeSelf);
            ui.GetComponentsInChildren<StoreItem>().ToList().ForEach(item =>
            {
                item.Initialize();
            });
        }
    }

    public Color GetAffordableColor()
    {
        return affordable;
    }

    public Color GetUnAffordableColor()
    {
        return notAffordable;
    }

    public Color GetEquipedColor()
    {
        return equiped;
    }
    public bool IsOpen()
    {
        return ui.activeSelf;
    }

    public void InitializeAllStoreItems()
    {
        ui.GetComponentsInChildren<StoreItem>().ToList().ForEach(item =>
        {
            item.Initialize();
        });
    }
}
