using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (collision.gameObject.CompareTag(Constant.TAG_MINIMAP_SQUARE))
            {
                MinimapSquare minimapSquare = collision.GetComponent<MinimapSquare>();
                minimapSquare.OnWave();
            }
        }
    }
}