using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class FishManager : MonoBehaviour
{

    [SerializeField] bool initialize;
    [SerializeField] List<GameObject> fishSpawners;

    // Start is called before the first frame update
    void Start()
    {
        initialize = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(initialize)
        {

            Initialize();
            initialize = false;
        }

    }

    void Initialize()
    {
        if(fishSpawners == null)
            fishSpawners = new List<GameObject>();
        SpawnFish();
    }

    void SpawnFish()
    {
        
        CheckTypes();

        foreach(GameObject f in fishSpawners) {

            if (f == null)
                continue;
            ISpawner spawner = f.GetComponent<ISpawner>();
            spawner.SpawnAll();
        }
    }

    void CheckTypes()
    {
        if (fishSpawners == null)
            return;
        List<GameObject> correctFish = new List<GameObject>(fishSpawners.Count);
        foreach(GameObject f in fishSpawners)
        {
            if (f == null)
            {
                correctFish.Add(f);
                continue;
            }
            //check if there is a component on the gameobject of type ISpawnable
            if (f.GetComponents<Component>().ToList().Exists(x => x is ISpawner))
            {
                correctFish.Add(f);
                continue;
            }

            //if we did not continue, this object is not ISpawnable
            Debug.LogWarning($"GameObject {f} is in the FishManager fish list but is not type ISpawnable!");
        }

        fishSpawners = correctFish;
    }

    private void OnValidate()
    {
        CheckTypes();
    }
}
