using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveObject : MonoBehaviour
{
    [SerializeField] Sprite lightSprite;
    [SerializeField] GameObject onClickPanel;

    SpriteRenderer spriteRenderer;
    Sprite standardSprite;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        standardSprite = spriteRenderer.sprite;
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
        //onClickPanel.SetActive(true);
    }



}
