using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] float fireRate = 1f;
    float fireDuration = 1f;

    [SerializeField] int barrelCount = 1;
    [SerializeField] float barrelDistance = 1f;
    [SerializeField] float barrelAngle = 5f;
    List<GameObject> barrels;

    [SerializeField] bool isShooting = true;
    [SerializeField] bool isShootAvailable = true;
    bool countThisFrame = false;

    float timePool = 0f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletMaxSpeed = 20f;

    private AttributesContainerComponent attributesComponent;

    private void Start()
    {
        attributesComponent = GetComponent<AttributesContainerComponent>();

        barrels = new List<GameObject>();
        fireDuration = 1 / fireRate;
        CreateBarrels();
        enabled = false;
    }

    private void Update()
    {
        if (countThisFrame)
        {
            timePool += Time.deltaTime;
            if(timePool >= fireDuration) 
            {
                if(isShooting)
                {
                    HandleTimePool();
                }
                else
                {
                    isShootAvailable = true;
                    countThisFrame = false;
                    timePool = 0f;
                    enabled = false;
                }
            }
        }
        else
        {
            countThisFrame = true;
        }
    }

    void CreateBarrels()
    {
        foreach(GameObject barrel in barrels) 
        {
            barrels.Remove(barrel);
            Destroy(barrel);
        }

        for (int i = 0; i < barrelCount; i++)
        {
            Vector3 barrelPosition = new Vector3(i * barrelDistance - ((barrelCount - 1) * barrelDistance) / 2, 0f, 0f);
            Quaternion barrelRotation = Quaternion.Euler(0f, 0f, ((barrelCount - 1) * barrelAngle) / 2 - i * barrelAngle);
            GameObject barrelInstance = new GameObject();
            barrelInstance.transform.parent = transform;
            barrelInstance.transform.localPosition = barrelPosition;
            barrelInstance.transform.localRotation = barrelRotation;
            barrels.Add(barrelInstance);
        }
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public void StartShooting()
    {
        isShooting = true;
        if(isShootAvailable)
        {
            Shoot();
        }
        enabled = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    void HandleTimePool()
    {
        while(timePool >= fireDuration)
        {
            timePool -= fireDuration;
            Shoot(timePool);
        }
    }

    void Shoot(float distance = 0f)
    {
        isShootAvailable = false;
        foreach(GameObject barrel in barrels) 
        {
            Vector3 bulletPositionDelta = barrel.transform.up * distance * bulletMaxSpeed;
            GameObject bulletInstance = Instantiate(bulletPrefab, barrel.transform.position + bulletPositionDelta, barrel.transform.rotation);
        }
    }
}