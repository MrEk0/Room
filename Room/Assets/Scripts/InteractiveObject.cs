using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveObject : MonoBehaviour
{
    [SerializeField] Sprite lightSprite;

    SpriteRenderer spriteRenderer;
    Sprite standardSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        standardSprite = spriteRenderer.sprite;
    }

    private void OnMouseOver()
    {
        spriteRenderer.sprite = lightSprite;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = standardSprite;
    }
}
