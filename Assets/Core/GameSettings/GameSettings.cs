using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] int frameRate = 165;
    private void Awake()
    {
        Application.targetFrameRate = frameRate;
    }
}
