using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] Vector2 bulletSize = new Vector2(0.1f, 0.25f);
    public float damage = 10f;

    private void Awake()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        Vector3 moveDelta = transform.forward * maxSpeed * Time.deltaTime;
        /*RaycastHit hit = Physics.BoxCast(transform.position, bulletSize, transform.rotation.eulerAngles.y, moveDelta.normalized, moveDelta.magnitude, LayerMask.GetMask("NPC"));
        if (hit) 
        {
            transform.position = hit.point;

            AttributesContainerComponent attribute = hit.collider.GetComponent<AttributesContainerComponent>();
            if(attribute)
            {
                attribute.SetAttribute("Health" , attribute.GetAttribute("Health").value - damage);
            }

            Destroy(gameObject);
        }
        else
        {
            transform.position += moveDelta;
        }*/

        transform.position += moveDelta;
    }
}
