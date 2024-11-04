using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowTile arrowTile;
    public SpriteRenderer spriteRenderer;
    public DirectionType directionType;
    public Vector2 dir;
    public SpriteRenderer shadowSpriteRenderer;




    public void OnInit()
    {
        InitDir();
        RotateAlongDirection();
        InitShadowPos();
    }

    public void InitDir()
    {
        switch(directionType)
        {
            case DirectionType.Left:
                dir = Vector2.left;
                break;
            case DirectionType.Right:
                dir = Vector2.right;
                break;
            case DirectionType.Up:
                dir = Vector2.up;
                break;
            case DirectionType.Down:
                dir = Vector2.down;
                break;
        }
    }

    public void RotateAlongDirection()
    {
        switch (directionType)
        {
            case DirectionType.Up:
                this.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                break;
            case DirectionType.Right:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
            case DirectionType.Down:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
                break;
            case DirectionType.Left:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -270));
                break;
        }
    }

    public void InitShadowPos()
    {
        shadowSpriteRenderer.transform.localPosition = Vector3.zero;
        shadowSpriteRenderer.transform.position -= Vector3.up * 0.02f;
    }
}

public enum DirectionType
{
    Up, Right, Down, Left,   
}