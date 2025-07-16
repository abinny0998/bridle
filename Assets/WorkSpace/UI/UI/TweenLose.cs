using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TweenLose : MonoBehaviour
{
    [SerializeField] private CanvasGroup losePanelCanvasGroup;
    [SerializeField] private GameObject Panel_Button;
    [SerializeField] private GameObject Panel_Stars;
    [SerializeField] private GameObject Title_Panel_Lose;

    private Vector3 originalButtonPosition;
    private Vector3 originalStarsPosition;


    [Header("UI elements")]
    [SerializeField] private InGame inGame;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button TryAgainButton;


    private void Start()
    {
        losePanelCanvasGroup.alpha = 0;
        losePanelCanvasGroup.interactable = false;
        losePanelCanvasGroup.blocksRaycasts = false;

        Title_Panel_Lose.transform.localScale = new Vector3(0, 1, 1);

        // Store original button position and move it down
        originalButtonPosition = Panel_Button.transform.localPosition;
        Panel_Button.transform.localPosition = originalButtonPosition + Vector3.down * 3000f;
        originalStarsPosition = Panel_Stars.transform.localPosition;
        Panel_Stars.transform.localPosition = originalStarsPosition + Vector3.down * 3000f;
    }

    void OnEnable()
    {
        BackButton?.onClick.AddListener(OnBackClickedOnce);
        TryAgainButton?.onClick.AddListener(OnTryAgainClickedOnce);
    }

    private void OnBackClickedOnce()
    {
        BackButton.interactable = false;
        inGame.ReturnMenu();
    }

    private void OnTryAgainClickedOnce()
    {
        TryAgainButton.interactable = false;
        inGame.PlayAgainGame();
    }
    void OnDisable()
    {
        BackButton?.onClick.RemoveAllListeners();
        TryAgainButton?.onClick.RemoveAllListeners();
    }

    public void ShowLosePanel()
    {
        losePanelCanvasGroup.DOFade(1, 0.5f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                Title_Panel_Lose.transform.DOScaleX(1.2f, 0.8f).SetEase(Ease.OutBack).SetDelay(0.2f).
                    OnComplete(() =>
                    {
                        Panel_Stars.transform.DOLocalMove(originalStarsPosition, 0.6f).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            Panel_Button.transform.DOLocalMove(originalButtonPosition, 0.6f).SetEase(Ease.OutBack);
                        });
                    });
            });
        losePanelCanvasGroup.interactable = true;
        losePanelCanvasGroup.blocksRaycasts = true;
    }


    public void HideLosePanel()
    {
        losePanelCanvasGroup.DOFade(0, 0.5f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                losePanelCanvasGroup.interactable = false;
                losePanelCanvasGroup.blocksRaycasts = false;
                Title_Panel_Lose.transform.localScale = new Vector3(0, 1, 1);
                Panel_Button.transform.localPosition = originalButtonPosition + Vector3.down * 2000f;
                Panel_Stars.transform.localPosition = originalStarsPosition + Vector3.down * 2000f;
                BackButton.interactable = true;
                TryAgainButton.interactable = true;
            });
    }
}
