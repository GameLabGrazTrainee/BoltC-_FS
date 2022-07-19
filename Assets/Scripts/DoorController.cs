using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite DoorClosed;
    [SerializeField] private Sprite DoorOpen;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = DoorClosed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerController>();

            if (player.HasKey == true)
            {
                spriteRenderer.sprite = DoorOpen;
                var collider = GetComponent<BoxCollider2D>();
                collider.enabled = false;
                Debug.Log("open");
            }
        }
    }
}
