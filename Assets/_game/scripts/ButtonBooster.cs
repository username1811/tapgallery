using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBooster : MonoBehaviour
{
    public BoosterType boosterType;
    public Image boosterImage;
    public RectTransform rectTransform;
    public Button button;


    [Header("con:")]
    public GameObject numberParent;
    public TextMeshProUGUI numberText;

    [Header("ko con:")]
    public GameObject addObj;

    [Header("coming soon:")]
    public GameObject lockObj;
    public TextMeshProUGUI lockText;
    public bool isLock => lockObj.gameObject.activeInHierarchy;


    public Booster booster => BoosterManager.Ins.boosters.FirstOrDefault(x=>x.boosterType==boosterType);


    public  void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            OnClick();
        });
    }

    public void OnInitt()
    {
        Refresh();
    }

    public void Refresh()
    {
        boosterImage.sprite = booster.icon;
        float posX = boosterType == BoosterType.Bomb ? 10f : -9f;
        boosterImage.rectTransform.anchoredPosition = new Vector2(posX, boosterImage.rectTransform.anchoredPosition.y);
        RefreshAmount();
        //ShowTutorial(isTutorial && !DataManager.Ins.playerData.tutorialedBoosterTypes.Contains(boosterType));
        ResizeImgKeepHeight();
        ShowLock(DataManager.Ins.playerData.currentLevelIndex < booster.levelIndexToIntroduce || !DataManager.Ins.playerData.unlockedBoosterTypes.Contains(boosterType));
    }

    public void ShowLock(bool isShow)
    {
        lockObj.SetActive(isShow);
        lockText.text = "Lv " + (booster.levelIndexToIntroduce + 1).ToString();
    }

    public void OnTut()
    {
        if (boosterType == BoosterType.Hint) return;
        UIManager.Ins.GetUI<GamePlay>().hand.gameObject.SetActive(true);
        UIManager.Ins.GetUI<GamePlay>().hand.Anim();
        UIManager.Ins.GetUI<GamePlay>().hand.rectTransform.position = this.rectTransform.position;
        UIManager.Ins.GetUI<GamePlay>().blackCoverObj.SetActive(true);
        UIManager.Ins.GetUI<GamePlay>().blackCoverObj.transform.SetParent(this.transform.parent);
        UIManager.Ins.GetUI<GamePlay>().blackCoverObj.transform.SetAsLastSibling();
        this.transform.SetAsLastSibling();


    }

    public void RefreshAmount()
    {
        if (booster.levelIndexToIntroduce > DataManager.Ins.playerData.currentLevelIndex)
        {
            numberParent.SetActive(false);
            addObj.SetActive(false);
            return;
        }
        int currentAmount = 0;
        switch (boosterType)
        {
            case BoosterType.Hint:
                currentAmount = DataManager.Ins.playerData.boosterHintAmount; break;
            case BoosterType.Bomb:
                currentAmount = DataManager.Ins.playerData.boosterBombAmount; break;
            case BoosterType.Magnet:
                currentAmount = DataManager.Ins.playerData.boosterMagnetAmount; break;
        }
        if (currentAmount > 0 && DataManager.Ins.playerData.unlockedBoosterTypes.Contains(boosterType))
        {
            numberParent.SetActive(true);
            addObj.SetActive(false);
            numberText.text = currentAmount.ToString();
        }
        else
        {
            numberParent.SetActive(false);
            addObj.SetActive(DataManager.Ins.playerData.unlockedBoosterTypes.Contains(boosterType));
        }
    }

    public void OnClick()
    {
        if (isLock) return;
        if (BoosterManager.Ins.isTutorialing) return;
        int currentAmount = 0;
        switch (boosterType)
        {
            case BoosterType.Hint:
                currentAmount = DataManager.Ins.playerData.boosterHintAmount; break;
            case BoosterType.Bomb:
                currentAmount = DataManager.Ins.playerData.boosterBombAmount; break;
            case BoosterType.Magnet:
                currentAmount = DataManager.Ins.playerData.boosterMagnetAmount; break;
        }
        if (currentAmount <= 0)
        {
            Debug.Log("not enough money");
        }
        else
        {
            BoosterManager.Ins.UseBooster(boosterType);
            if (BoosterManager.Ins.isTutorialing)
            {
                UIManager.Ins.GetUI<GamePlay>().hand.gameObject.SetActive(false);
                UIManager.Ins.GetUI<GamePlay>().blackCoverObj.SetActive(false);
            }
        }
    }

    IEnumerator IERestoreButton()
    {
        yield return new WaitForSeconds(0.2f);
        button.enabled = true;
    }

    public void AnimBounceAmount()
    {
        Vector3 oldScale = numberParent.transform.localScale;
        numberParent.transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            numberParent.transform.DOScale(oldScale, 0.1f).SetEase(Ease.InSine);
        });
    }

    public void ResizeImgKeepWidth()
    {
        float originalWidth = boosterImage.rectTransform.sizeDelta.x;
        boosterImage.SetNativeSize();
        float widthOnHeight = boosterImage.rectTransform.sizeDelta.x / boosterImage.rectTransform.sizeDelta.y;
        boosterImage.rectTransform.sizeDelta = new Vector2(originalWidth, originalWidth / widthOnHeight);
    }

    public void ResizeImgKeepHeight()
    {
        float originalHeight = boosterImage.rectTransform.sizeDelta.y;
        boosterImage.SetNativeSize();
        float heightOnWidth = boosterImage.rectTransform.sizeDelta.y / boosterImage.rectTransform.sizeDelta.x;
        boosterImage.rectTransform.sizeDelta = new Vector2(originalHeight / heightOnWidth, originalHeight);
    }

}
