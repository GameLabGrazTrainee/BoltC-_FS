using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float Speed = 4.5f;
    public float Sped = 1f;
    public float atRight;
   
    private void Start()
    {
        var player = GameObject.Find("Player").GetComponent<PlayerController>().facingRight;
        if(player)
        atRight = 2;
        if(!player)
        atRight = -2;
    }


    // Update is called once per frame
    private void Update()
    {
        transform.position += new Vector3(atRight, 0f, 0f) * Speed * Time.deltaTime;
        transform.Rotate (new Vector3(0, 0, 40) * Sped * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Destroy(gameObject);
    }
}
