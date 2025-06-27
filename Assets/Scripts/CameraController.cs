using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject hook;
    [SerializeField] Vector2 minBounds;
    [SerializeField] Vector2 maxBounds;
    [SerializeField] float minSize, maxSize;
    [SerializeField] Vector2 calculatedBounds;
    [SerializeField] Slider sizeSlider;


    private void Awake()
    {
        CalculateBounds(sizeSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        hook = GameObject.FindGameObjectWithTag("Hook");
        Vector3 pos = new Vector3(hook.transform.position.x, hook.transform.position.y, -30);

        if(pos.x < calculatedBounds.x)
            pos.x = calculatedBounds.x;
        if(pos.y < calculatedBounds.y)
            pos.y = calculatedBounds.y;


        pos = Vector3.Slerp(transform.position, pos, .1f);

        transform.position = pos;
        
    }

    public void CalculateBounds(float size)
    {
        cam.orthographicSize = size;
        Vector2 range = maxBounds - minBounds;
        range = new Vector2(Mathf.Abs(range.x), Mathf.Abs(range.y));

        float scale = maxSize - minSize;
        float adjustedSize = size- minSize;
        adjustedSize /= scale;

        calculatedBounds.x = minBounds.x + range.x * adjustedSize;
        calculatedBounds.y = minBounds.y + range.y * adjustedSize;

    }

    private void OnValidate()
    {
        CalculateBounds(sizeSlider.value);
    }
}
