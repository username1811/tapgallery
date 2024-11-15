using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheelUI : UICanvas
{
    [Header("References:")]
    public RectTransform wheel;
    public Button spinButton;
    public float spinDuration = 5f;
    private float targetAngle;
    private bool isSpinning = false;
    public PickerWheel picker;
    public Image RewardImage;

    public override void Open()
    {
        base.Open();
    
    }

    public void SpinWheel()
    {
        StartSpin();
    }

    public void StartSpin()
    {
        Clear();
        if (isSpinning) return;

        isSpinning = true;

        targetAngle = 360 * Random.Range(5, 8) + Random.Range(0, 360);

        wheel
            .DORotate(new Vector3(0, 0, -targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                isSpinning = false;
                OnSpinComplete();
            });
    }

    private void OnSpinComplete()
    {
        float finalAngle = wheel.eulerAngles.z; 
        if (finalAngle < 0) finalAngle += 360;

        finalAngle %= 360;

        finalAngle += 24.5f;
        if (finalAngle < 0) finalAngle += 360; 

        int index = Mathf.FloorToInt(finalAngle / picker.pieceAngle) % picker.dataSpin.dataList.Count;
        SpinItem selectedPiece = picker.dataSpin.dataList[index];

        Pieces pie = picker.piecesList[index];
        pie.PlayFx(selectedPiece);

        RewardImage.sprite = selectedPiece.icon;
        Debug.Log("Selected Piece Label: " + selectedPiece.Amount);

       

    }



    public void BackBtn()
    {
     
        UIManager.Ins.OpenUI<Home>();
      
        Close(0);
    }

    public void Clear()
    {
        wheel.localEulerAngles = new Vector3(0, 0, 0);
        RewardImage.sprite = null;
    }
}
