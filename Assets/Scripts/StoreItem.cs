using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    [SerializeField] StoreManager storeManager;
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] float cost;
    [SerializeField] bool purchased;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] EquipmentType equipmentType;


    private void Start()
    {
        int stored = PlayerPrefs.GetInt(itemPrefab.GetComponent<INameable>().GetName(), 0);
        purchased = stored == 1;

        if (cost < .01f)
            purchased = true;

        Save();

        StateManager.instance.SetUnlockedState(name, purchased);

        Initialize();
    }


    void Save()
    {

        PlayerPrefs.SetInt(itemPrefab.GetComponent<INameable>().GetName(), purchased ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Initialize()
    {
        storeManager = GameObject.FindGameObjectWithTag("StoreManager").GetComponent<StoreManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();




        if (!purchased)
        {
            


            if (gameManager.GetMoney() >= cost)
            {

                buyButton.image.color = storeManager.GetAffordableColor();
            }

            else
                buyButton.image.color = storeManager.GetUnAffordableColor();

            costText.SetText($"{cost:c0}");
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
            costText.gameObject.SetActive(true);

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(Buy);
        }
        else
        {

            bool equiped = false;
            if(StateManager.instance != null)
            {

                if (equipmentType.Equals(EquipmentType.FishingRod))
                    equiped = StateManager.instance.equipedRod.GetName().Equals(itemPrefab.GetComponent<INameable>().GetName());
                else if (equipmentType.Equals(EquipmentType.Hook))
                    equiped = StateManager.instance.equipedHook.GetName().Equals(itemPrefab.GetComponent<INameable>().GetName());
                else if (equipmentType.Equals(EquipmentType.Bait))
                    equiped = StateManager.instance.equipedBait.GetName().Equals(itemPrefab.GetComponent<INameable>().GetName());
            }

            if (!equiped)
            {

                buyButton.image.color = storeManager.GetAffordableColor();
                buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
            else
            {
                buyButton.image.color = storeManager.GetEquipedColor();
                buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equiped";

            }

            costText.gameObject.SetActive(false);
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(Equip);
        }



    }

    void Buy()
    {
        if (gameManager.GetMoney() < cost || purchased)
            return;

        gameManager.RemoveMoney(cost);
        purchased = true;
        StoreManager.instance.InitializeAllStoreItems();
    }

    void Equip()
    {
        CastingManager cm = GameObject.FindGameObjectWithTag("CastingManager").GetComponent<CastingManager>();

        if (equipmentType.Equals(EquipmentType.FishingRod))
        {
            FishingRod rod = itemPrefab.GetComponent<FishingRod>();
            cm.EquipRod(rod);
            StateManager.instance.equipedRod = rod;
        }

        else if (equipmentType.Equals(EquipmentType.Hook))
        {
            Hook hook = itemPrefab.GetComponent<Hook>();
            cm.EquipHook(hook);
            StateManager.instance.equipedHook = hook;

        }
        else
        {
            Bait bait = itemPrefab.GetComponent<Bait>();
            cm.EquipBait(bait);
            StateManager.instance.equipedBait = bait;

        }

        StoreManager.instance.InitializeAllStoreItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnValidate()
    {
        Initialize();
    }


    private void OnApplicationQuit()
    {
        Save();
    }
}
