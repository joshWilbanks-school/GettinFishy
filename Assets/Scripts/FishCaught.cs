using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishCaught : MonoBehaviour
{
    public static FishCaught instance;
    [SerializeField] bool caught;
    [SerializeField] float caughtTime;
    [SerializeField] float delayFadeTime;
    [SerializeField] TextMeshProUGUI mainText, weightText, moneyText;
    System.Diagnostics.Stopwatch timer;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void Caught(float weight, float money)
    {
        timer = System.Diagnostics.Stopwatch.StartNew();
        caught = true;
        mainText.gameObject.SetActive(true);
        weightText.gameObject.SetActive(true);
        moneyText.gameObject.SetActive(true);
        weightText.text = $"{weight:f2}lbs";
        moneyText.text = $"{money:c2}";

        ResetColor(weightText);
        ResetColor(moneyText);
        ResetColor(mainText);
    }

    void ResetColor(TextMeshProUGUI textObj)
    {

        Color color = textObj.color;
        color.a = 1;
        textObj.color = color;
    }

    void DecreaseColor(TextMeshProUGUI textObj)
    {
        Color color = textObj.color;
        float elapsed = (timer.ElapsedMilliseconds - delayFadeTime * 1000);
        float remaining = (caughtTime - delayFadeTime) * 1000;
        color.a = 1 - elapsed / remaining;
        textObj.color = color;
    }

    public bool IsDone()
    {
        return caught;
    }

    private void Update()
    {
        if(!caught)
            return;

        if(timer.ElapsedMilliseconds > caughtTime * 1000)
        {
            timer.Stop();
            caught = false;
            mainText.gameObject.SetActive(false);
            weightText.gameObject.SetActive(false);
            moneyText.gameObject.SetActive(false);
            return;
        }

        if(timer.ElapsedMilliseconds > delayFadeTime * 1000)
        {
            DecreaseColor(weightText);
            DecreaseColor(moneyText);
            DecreaseColor(mainText);
        }

    }
}
