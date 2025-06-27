using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingManager : MonoBehaviour
{
    public static CastingManager instance;

    [SerializeField] Vector3 rodOffset;
    [SerializeField] Vector3 hookOffset;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        EquipRod(StateManager.instance.equipedRod);
    }

    public void EquipRod(FishingRod rod)
    {
        FishingRod oldRod = GetComponentInChildren<FishingRod>();
        Hook hook = GetComponentInChildren<Hook>();
        FishingRod newRod = Instantiate(rod, rodOffset, Quaternion.identity, transform);

        newRod.transform.parent = transform;
        newRod.transform.localPosition = rodOffset;
        hook.transform.parent = newRod.transform;
        var pos = hook.transform.position;
        hook.transform.position = new Vector3(pos.x, pos.y, 0);

        Destroy(oldRod.gameObject);



        newRod.SetHook(hook.gameObject);
        hook.ResetHook(newRod);
        hook.transform.localPosition = new Vector3(hook.transform.localPosition.x, hook.transform.localPosition.y, -.1f);
    }


    public void EquipHook(Hook hook)
    {
        Hook oldHook = GetComponentInChildren<Hook>();
        FishingRod rod = GetComponentInChildren<FishingRod>();
        Bait bait = GetComponentInChildren<Bait>();

        Hook newHook = Instantiate(hook, hookOffset, Quaternion.identity, transform);

        newHook.transform.parent = rod.transform;
        var pos = newHook.transform.localPosition;
        newHook.transform.localPosition = new Vector3(pos.x, pos.y, -.1f);

        rod.SetHook(newHook.gameObject);
        newHook.ResetHook(rod);

        if(bait != null)
        {

            bait.transform.parent = newHook.transform;
            newHook.EquipBait(bait);
        }

        if(oldHook != null)
            Destroy(oldHook.gameObject);
    }

    public void EquipBait(Bait bait)
    {
        Bait oldBait = GetComponentInChildren<Bait>();
        Hook hook = GetComponentInChildren<Hook>();

        Bait newBait = Instantiate(bait, transform);
        if(oldBait != null)
            Destroy(oldBait.gameObject);

        newBait.transform.parent = hook.transform;
        newBait.transform.position = hook.transform.position;
        hook.EquipBait(newBait);
    }
}
