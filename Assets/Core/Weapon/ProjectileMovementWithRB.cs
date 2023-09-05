using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovementWithRB : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float maxSpeed = 20f;

    private void Awake()
    {
        Destroy(gameObject, 3f);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
