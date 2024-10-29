using DG.Tweening.Core.Easing;
using UnityEngine;

public class ClickManager : Singleton<ClickManager> 
{
    /*void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, GameManager.Ins.arrowMask);
            if (hit.collider != null)
                Debug.Log(hit.collider.gameObject.name);
        }
    }*/
}

