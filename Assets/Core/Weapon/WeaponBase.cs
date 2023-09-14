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
    [SerializeField] float barrelDistance = 0.1f;
    [SerializeField] float barrelAngle = 2f;
    List<GameObject> barrels;

    [SerializeField] bool isShooting = true;
    [SerializeField] bool isShootAvailable = true;
    bool countThisFrame = false;

    float timePool = 0f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletMaxSpeed = 25f;

    [SerializeField] AttributesContainerComponent attributesContainerComponent;

    private void Start()
    {
        attributesContainerComponent.AttributeChanged += AttributeChanged;

        //fireRate = attributesContainerComponent.GetAttribute("FireRate").value;
        //fireRate = attributesContainerComponent.GetAttribute("BarrelCount").value;

        barrels = new List<GameObject>();
        fireDuration = 1 / fireRate;
        CreateBarrels();
        enabled = false;
    }

    private void AttributeChanged(object sender, AttributesContainerComponent.AttributeChangedArgs e)
    {
        switch (e.name) 
        {
            case "MaxFireRate":
                print(e.name + ": " + e.newValue);
                fireRate = e.newValue;
                print("fireRate: " + fireRate);
                fireDuration = 1 / fireRate;
                break;
            case "BarrelCount":
                barrelCount = (int)e.newValue;
                CreateBarrels();
                break;
            default: 
                break;
        }
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
            Destroy(barrel);
        }
        barrels.Clear();

        for (int i = 0; i < barrelCount; i++)
        {
            Vector3 barrelPosition = new Vector3(i * barrelDistance - ((barrelCount - 1) * barrelDistance) / 2, 0f, 0f);
            Quaternion barrelRotation = Quaternion.Euler(0f, i * barrelAngle - ((barrelCount - 1) * barrelAngle) / 2, 0f);
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
        if(fireRate > 0f) 
        {
            isShooting = true;
            if (isShootAvailable)
            {
                Shoot();
            }
            enabled = true;
        }
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
            Vector3 bulletPositionDelta = barrel.transform.forward * distance * bulletMaxSpeed;
            GameObject bulletInstance = Instantiate(bulletPrefab, barrel.transform.position + bulletPositionDelta, barrel.transform.rotation);
            bulletInstance.GetComponent<ProjectileMovement>().damage = attributesContainerComponent.GetAttribute("Damage").value;
        }
    }
}