using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour, IHooker, INameable
{

    [SerializeField] GameObject rod;
    [SerializeField] GameObject rodEnd;
    [SerializeField, Range(1, 1000)] float weight;
    [SerializeField, Range(1, 1000)] float drag;
    [SerializeField, Range(1, 1000)] float gravityEffect;
    [SerializeField] Vector2 castForceMult;
    [SerializeField] Vector3 startPosition = new Vector3(0, 0, 0);
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool isCasted;
    [SerializeField] System.Diagnostics.Stopwatch castTimer;
    [SerializeField] bool hasHitWater;
    [SerializeField] bool isCatching;
    [SerializeField] GameObject fishOnHook;
    [SerializeField] bool animateReset;
    [SerializeField] Bait bait;
    [SerializeField] Vector3 baitOffset = new Vector3(2, -3, -.1f);
    [SerializeField] float baitScale;
    [SerializeField] System.Diagnostics.Stopwatch gotAwayTimer;
    [SerializeField] bool gotAway;
    [SerializeField] float getAwayTime;
    [SerializeField] TextMeshProUGUI gotAwayText;
    [SerializeField] FishCaught fishCaught;
    [SerializeField] string itemName;

    public void Awake()
    {

        if (rb == null)
            TryGetComponent(out rb);
        if (rb == null)
            gameObject.AddComponent<Rigidbody>();

        gotAwayText = GameObject.FindGameObjectWithTag("GotAwayText").GetComponent<TextMeshProUGUI>();
        rodEnd = GameObject.FindGameObjectWithTag("RodTip");
    }


    public void ResetHook(FishingRod rod)
    {
        this.rod = rod.gameObject;
        rodEnd = rod.gameObject.GetComponentsInChildren<Transform>()
            .First(x => x.tag.Equals("RodTip")).gameObject;

        startPosition = rodEnd.transform.localPosition;

        startPosition.z = -.1f;

        SetScale();
        Initialize();
    }

    public void SetScale()
    {
        float scale = .015f / rod.transform.localScale.x;
        transform.localScale = new Vector3(scale, scale, 0);
    }

    public void Initialize()
    {
        SetSettings();
        ResetForCast();
    }

    void SetSettings()
    {

        rb.mass = weight / 100;
        rb.drag = drag / 100;
        rb.angularDrag = drag / 100;
        rb.gravityScale = Settings.AirGravity * gravityEffect / 100;
        castTimer = new System.Diagnostics.Stopwatch();
    }

    public void EquipBait(Bait bait)
    {
        this.bait = bait;

        bait.gameObject.transform.localPosition = baitOffset;
        bait.SetScale();
    }

    public void Cast(float power, float angle)
    {
        if (isCasted)
            return;


        SoundManager.instance.PlayCastingSound(transform);

        SetSettings();
        castTimer.Start();
        float x = (float)Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = (float)Mathf.Sin(Mathf.Deg2Rad * angle);
        Vector2 force = new Vector2( x * power, y * power);
        var additive = force * Get45DegreeMult(y) * castForceMult.y;
        force += additive;
        rb.AddForce(force);
        isCasted = true;
        rod.GetComponent<IReeler>().SetIsCasted(true);
    }

    float Get45DegreeMult(float mult)
    {
        //angle is between 0 and 1. We want max power at .5. This sets it to between 0 and 2, now max power is at 1
        mult *= 2;

        //now between -1 and 1
        mult -= 1;

        //now between 0 and 1, where 0 is max power
        mult = Mathf.Abs(mult);

        //now between 0 and 1, where 1 is max power
        mult = 1 - mult;

        return mult;


    }

    public void Reel(float power, GameObject start)
    {
        if (!isCasted)
            return;

        SoundManager.instance.PlayReelingSound(transform);

        Vector2 difference = start.transform.position - transform.position;
        difference.Normalize();
        difference *= power;
        rb.AddForce(difference);


        if (isCatching && fishOnHook != null)
            fishOnHook.GetComponent<Rigidbody2D>().velocity = rb.velocity;
    }
    
    void ResetForCast()
    {
        if (animateReset)
            return;

        fishOnHook = null;

        if (rod == null)
            rod = GameObject.FindGameObjectWithTag("Rod");

        rod.GetComponent<IReeler>().ResetForNextCast();

        isCasted = false;
        isCatching = false;
        transform.localPosition = new Vector3(startPosition.x, startPosition.y, -.1f);
        hasHitWater = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.excludeLayers = 0;

        if (gotAwayText != null)
            gotAwayText.SetText("");
    }


    bool CastFinished()
    {
        bool finished = hasHitWater;
        finished &= transform.position.y > Settings.SurfaceLevel;
        finished &= castTimer.Elapsed.TotalSeconds > 4;

        return finished;
    }


    void FishGotCaught()
    {


        animateReset = true;
        if(fishOnHook == null)
        {

            ResetForCast();
            return;
        }
        IFishable fishable = fishOnHook.GetComponent<IFishable>();
        float fishMoney = fishable.GetValue();
        float weight = fishable.GetWeight();
        
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AddMoney(fishMoney);


        FishCaught.instance.Caught(weight, fishMoney);

        Destroy(fishOnHook);

        ResetForCast();
    }


    void AnimateReset()
    {
        rb.gravityScale = 0;
        Vector3 diff = transform.localPosition - startPosition;

        bool reset = Mathf.Abs(diff.x) > .04;
        reset &= Mathf.Abs(diff.y) > .04;

        if (!reset)
        {

            animateReset = false;
            ResetForCast();
            return;
        }

        diff.Normalize();

        diff *= Settings.ResetSpeed * transform.localScale.x;
        


        diff = transform.localPosition - diff;



        transform.localPosition = new Vector3(diff.x, diff.y, -.1f);
    }

    public void SetRod(GameObject rod)
    {
        this.rod = rod;
    }

    private void Update()
    {

        if (Settings.Paused)
            return;

        if(isCasted && transform.position.y < Settings.SurfaceLevel && !hasHitWater)
        {

            hasHitWater = true;
            rb.gravityScale = Settings.Gravity * gravityEffect / 100;
        }



        if (animateReset)
            AnimateReset();

        if (CastFinished())
        {
            FishGotCaught();
            animateReset = true;
        }

        else if (Input.GetKeyDown(Settings.ResetCast))
        {
            animateReset = true;
            ResetForCast();
        }



        if (isCatching && fishOnHook != null)
            transform.position = fishOnHook.transform.position;

        if(gotAway)
        {
            if(gotAwayTimer.ElapsedMilliseconds > Settings.GetAwayTime * 1000)
            {
                gotAwayTimer.Stop();
                gotAway = false;
                animateReset = true;
                ResetForCast();
            }
        }

        if(transform.position.x < rodEnd.transform.position.x && fishOnHook != null)
        {
            transform.position = new Vector3(rodEnd.transform.position.x, transform.position.y, transform.position.z);
            fishOnHook.GetComponent<Fish>().ForcePosition(transform.position);

        }
    }

    void FishGotAway()
    {
        if (fishOnHook == null)
            return;

        isCatching = false;
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
        gotAwayTimer = System.Diagnostics.Stopwatch.StartNew();
        gotAway = true;
        rb.gravityScale = 0;
        gotAwayText.SetText("Fish Got Away!");
        fishOnHook = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (animateReset)
            return;

        if (collision.gameObject.tag != "Fish")
        {

            if (collision.gameObject.tag != "Background")
                return;
            FishGotAway();

            return;
        }

        if (isCatching)
            return;


        IFishable fish = collision.gameObject.GetComponentInChildren<IFishable>();
        if (!fish.GetSize().Equals(bait.GetSize()))
            return;

        fish.Flee();
        fishOnHook = collision.gameObject;
        isCatching = true;
        rb.excludeLayers = LayerMask.NameToLayer("Fish");

        if (rod == null)
            rod = GameObject.FindGameObjectWithTag("Rod");

        rod.GetComponent<IReeler>().SetIsCatching(true);
        transform.position = collision.gameObject.transform.position;
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
