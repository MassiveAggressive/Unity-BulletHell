using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] Vector2 bulletSize = new Vector2(0.1f, 0.25f);

    private void Awake()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        Vector3 moveDelta = transform.up * maxSpeed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, bulletSize, transform.rotation.eulerAngles.z, moveDelta.normalized, moveDelta.magnitude, LayerMask.GetMask("Wall"));
        if (hit) 
        {
            transform.position = hit.point;
            Destroy(gameObject);
        }
        else
        {
            transform.position += moveDelta;
        }
    }
}
