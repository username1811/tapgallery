
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public GameObject heartRedObj;


    public void Show(bool isShow)
    {
        heartRedObj.SetActive(isShow);
    }
}
