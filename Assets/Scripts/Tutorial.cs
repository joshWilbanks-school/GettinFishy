using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    [SerializeField] TextMeshPro left, right, bindsText;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {

        string binds =
            $"Shop = {Settings.Shop}\n" +
            $"Cast = {Settings.Cast}\n" +
            $"Reel = {Settings.Reel}\n" +
            $"Catch = {Settings.Catch}\n" +
            $"Reset = {Settings.ResetCast}\n" +
            $"Settings = {Settings.SettingsPage}\n";

        left.text = Settings.RotateLeft.ToString();
        right.text = Settings.RotateRight.ToString();
        bindsText.text = binds;
    }

    public void ToggleAll()
    {
        Toggle(left);
        Toggle(right);
        Toggle(bindsText);
    }

    void Toggle(TextMeshPro item)
    {
        item.gameObject.SetActive(!item.gameObject.activeSelf);
    }
}
