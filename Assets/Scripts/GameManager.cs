using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField, Min(0)] float money = 0;
    [SerializeField] TextMeshProUGUI moneyUI;
    [SerializeField] FishingRod[] ownedRods;
    [SerializeField] Hook[] ownedHooks;

    // Start is called before the first frame update
    void Start()
    {
        Load();
        Save();
        moneyUI.SetText($"{money:c2}");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddMoney(float amount)
    {
        money += amount;
        moneyUI.SetText($"{money:c2}");
        Save();
    }

    public void RemoveMoney(float amount)
    {
        money -= amount;
        moneyUI.SetText($"{money:c2}");
        Save();
    }

    void Save()
    {
        PlayerPrefs.SetFloat("money", money);
        PlayerPrefs.Save();
    }

    void Load()
    {
        money = PlayerPrefs.GetFloat("money", 0);
    }

    private void OnValidate()
    {

        moneyUI.SetText($"{money:c2}");
    }

    public float GetMoney()
    {
        return money;
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
