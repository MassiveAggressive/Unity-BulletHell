using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    private void Start()
    {
        int randomNumber = Random.Range(0, 10);
        print(randomNumber);
    }
}
