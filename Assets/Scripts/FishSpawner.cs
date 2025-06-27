using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class FishSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] int zCoord = 1;
    [SerializeField, Range(.1f, 100)] float minWeight = .1f;
    [SerializeField, Range(.1f, 10f)] float maxWeight = 1f;
    [SerializeField, Range(.01f, 10)] float weightMultiplier = 1f;
    [SerializeField] GameObject fishPrefab;
    [SerializeField, Range(1, 100)] int minAmount = 1;
    [SerializeField, Range(1, 100)] int maxAmount = 1;
    [SerializeField] Vector2 minSpawnCoord;
    [SerializeField] Vector2 maxSpawnCoord;
    [SerializeField] float moveSpeed;
    [SerializeField] float fleeSpeed;
    [SerializeField] float maxFleeSpeed;
    [SerializeField] float value;
    [SerializeField, Range(.1f, 10f)] float weightMoveSpeedMultiplier = 1;
    [SerializeField] bool drawGizmos;
    [SerializeField] Color gizmoColor = new Color(1, 1, 1, 1);
    [SerializeField] float minSpawnTime, maxSpawnTime, spawnTime;
    System.Diagnostics.Stopwatch spawnTimer;


    GameObject[] fish;

    public void DespawnAll()
    {
        Transform[] kids = GetComponentsInChildren<Transform>();
        for(int i = 0; i < kids.Length; i++)
        {
            if (kids[i].transform.Equals(transform))
                continue;

            if (!Application.isPlaying)
                DestroyImmediate(kids[i].gameObject);

            else
                Destroy(kids[i].gameObject);
        }
    }

    public void SpawnAll()
    {

        DespawnAll();

        if (fishPrefab == null) {

            Debug.LogWarning("Trying to spawn a Fish that doesn't have a prefab!");
            return;
        }

        //get amount of fish
        Random.InitState(System.DateTime.Now.Millisecond);
        int amount = minAmount;

        fish = new GameObject[amount];

        //instantiate all the fish
        for(int i = 0; i < amount; i++)
        {
            fish[i] = SpawnOne();
        }
    }


    float ScaleBasedOnWeightMultiplier(float speed, float weight)
    {
        return speed + speed * weight * weightMoveSpeedMultiplier;
    }

    public GameObject SpawnOne()
    {

        float weight = Random.Range(minWeight * weightMultiplier, maxWeight * weightMultiplier);

        Vector3 coord = new Vector3(Random.Range(minSpawnCoord.x, maxSpawnCoord.x), Random.Range(minSpawnCoord.y, maxSpawnCoord.y), zCoord);

        var fishy = Instantiate(fishPrefab, coord, Quaternion.identity, transform);

        

        fishy.transform.localScale *= weight;
        fishy.GetComponent<IFishable>().Initialize(minSpawnCoord.x, maxSpawnCoord.x,
            ScaleBasedOnWeightMultiplier(moveSpeed, weight),
             ScaleBasedOnWeightMultiplier(fleeSpeed, weight),
             ScaleBasedOnWeightMultiplier(maxFleeSpeed, weight),
             ScaleBasedOnWeightMultiplier(value, weight),
             weight);
        return fishy;
    }

    void Start()
    {
        spawnTimer = System.Diagnostics.Stopwatch.StartNew();
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void Update()
    {

        if (!Application.isPlaying)
            return;

        if (spawnTimer.ElapsedMilliseconds < spawnTime * 1000)
            return;

        float fishCount = GetComponentsInChildren<Fish>().Length;
        if (fishCount == maxAmount)
            return;

        SpawnOne();
        spawnTimer = System.Diagnostics.Stopwatch.StartNew();
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);

    }

    private void OnValidate()
    {
        if(maxSpawnCoord.x  < minSpawnCoord.x)
            maxSpawnCoord.x = minSpawnCoord.x;
        if(minSpawnCoord.x > maxSpawnCoord.x)
            minSpawnCoord.x = maxSpawnCoord.x;

        if (maxSpawnCoord.y < minSpawnCoord.y)
            maxSpawnCoord.y = minSpawnCoord.y;
        if (minSpawnCoord.y > maxSpawnCoord.y)
            minSpawnCoord.y = maxSpawnCoord.y;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Vector2 avg = (minSpawnCoord + maxSpawnCoord) * new Vector2(.5f, .5f);
        Vector2 size = new Vector2(Mathf.Abs(minSpawnCoord.x - maxSpawnCoord.x),
            Mathf.Abs(minSpawnCoord.y - maxSpawnCoord.y));

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(avg, size);
    }
}
