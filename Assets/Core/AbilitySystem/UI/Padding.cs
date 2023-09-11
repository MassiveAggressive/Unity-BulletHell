using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct padding
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}

public class Padding : MonoBehaviour
{
    public padding padding;

    [SerializeField] RectTransform child;

    void Start()
    {
        child = transform.GetChild(0).GetComponent<RectTransform>();
    }

    void Update()
    {

    }
}
