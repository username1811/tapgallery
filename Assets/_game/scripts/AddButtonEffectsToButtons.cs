using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonEffectsToButtons : MonoBehaviour
{
    void Start()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Button[] buttons = GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                button.AddComponent<ButtonScaleEffect>();
            }
        });
    }
}
