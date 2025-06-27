using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FishingRod : MonoBehaviour, IReeler, INameable
{
    [SerializeField, Range(1, 1000)] float castPower;
    [SerializeField, Range(1, 100)] float reelPower;
    [SerializeField, Range(1, 100)] float catchPower;
    [SerializeField] GameObject hookObj, rodEnd;
    [SerializeField] IHooker hook;
    [SerializeField] bool isCatching;
    [SerializeField] bool autoCatch;
    [SerializeField] bool isCasted;
    [SerializeField] TextMeshPro degreeText;
    [SerializeField] string itemName;

    private void Start()
    {
        degreeText = GameObject.Find("Degrees").GetComponent<TextMeshPro>();
    }

    public void GetHook()
    {
        if (hookObj == null)
            return;
        hook = hookObj.GetComponent<IHooker>();
        hook.Initialize();
    }

    public void SetHook(GameObject hook)
    {
        hookObj = hook;
        GetHook();
    }

    // Update is called once per frame
    void Update()
    {

        if (Settings.Paused)
            return;


        float degree = transform.rotation.eulerAngles.z;
        if (degree > 45.1)
            degree = degree - 360;
        degree += 45;
        degreeText.text = $"{degree:f1}°";

        RotateRod(Input.GetKey(Settings.RotateLeft), Input.GetKey(Settings.RotateRight));

        if (Input.GetKeyDown(Settings.Cast) && hook != null)
        {
            hook.Cast(castPower, 45 + transform.rotation.eulerAngles.z);

        }

        if(hook != null)
        {


            //reel continuously if there is not a fish on the hook
            if (Input.GetKey(Settings.Reel) && !isCatching)
                hook.Reel(reelPower, rodEnd);

            //require button spam if there is a fish on the hook
            bool reelIn = Input.GetKeyDown(Settings.Catch) || (Input.GetKey(Settings.Catch) && autoCatch);
            if ( reelIn && isCatching)
                hook.Reel(catchPower + catchPower * 10 * (autoCatch ? 0 : 1), rodEnd);
        }

        
    }

    private void OnValidate()
    {
        GetHook();
    }

    void RotateRod(bool left, bool right)
    {

        if (left == right || isCasted)
            return;

        int direction = left ? 1 : -1;

        Vector3 rotation = transform.rotation.eulerAngles;

        rotation.z += direction * Settings.RotateSpeed * Time.deltaTime;

        if (rotation.z > 45 && rotation.z < 90)
            rotation.z = 45;

        else if (rotation.z < 360 - 45 && rotation.z > 90)
            rotation.z = 360 - 45;

        transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetIsCatching(bool isCatching)
    {
        this.isCatching = isCatching;
    }

    public void ResetForNextCast()
    {
        isCatching = false;
        isCasted = false;
    }

    public void SetIsCasted(bool isCasted)
    {
        this.isCasted = isCasted;
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