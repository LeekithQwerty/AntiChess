using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonChessPiece : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite spriteArray;

    void ChangeSprite()
    {
        spriteRenderer.sprite = spriteArray;
    }
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Dialogue.FindObjectOfType<Dialogue>().killCartoonChessPiece)
        {
            ChangeSprite();
        }
    }
}
