using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerObject : MonoBehaviour
{
    [SerializeField] GameObject panelToShow;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            panelToShow.SetActive(true);
        }
    }
}
