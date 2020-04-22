using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveObject : MonoBehaviour
{
    [SerializeField] Sprite lightSprite;
    [SerializeField] GameObject onClickPanel;

    SpriteRenderer spriteRenderer;
    Sprite standardSprite;
    private bool isTrigger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        standardSprite = spriteRenderer.sprite;
        Collider2D[] colliders = GetComponents<Collider2D>();
        for(int i=0; i<colliders.Length; i++)
        {
            if(colliders[i].isTrigger)
            {
                isTrigger = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onClickPanel.SetActive(true);
    }

    private void OnMouseOver()
    {
        spriteRenderer.sprite = lightSprite;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = standardSprite;
    }

    private void OnMouseDown()
    {
        if (!isTrigger)
        {
            onClickPanel.SetActive(true);
        }
    }



}
