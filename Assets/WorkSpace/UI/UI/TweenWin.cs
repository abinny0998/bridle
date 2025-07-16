using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TweenWin : MonoBehaviour
{
    [Header("Tween Win Settings")]
    [SerializeField] private CanvasGroup winPanelCanvasGroup;
    [SerializeField] private GameObject Panel_Button;
    [SerializeField] private GameObject Title_Panel_Win;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private GameObject[] Star;

    [SerializeField] private Button BackButton;
    [SerializeField] private Button NextLevelButton;

    public event Action BackButtonClicked;
    public event Action NextLevelButtonClicked;

    private Vector3 originalButtonPosition;


    private void Start()
    {
        winPanelCanvasGroup.alpha = 0;
        winPanelCanvasGroup.interactable = false;
        winPanelCanvasGroup.blocksRaycasts = false;

        Title_Panel_Win.transform.localScale = new Vector3(0, 1, 1);

        originalButtonPosition = Panel_Button.transform.localPosition;
        Panel_Button.transform.localPosition = originalButtonPosition + Vector3.down * 2000f;

        foreach (var star in Star)
        {
            star.transform.localScale = Vector3.zero;
        }

    }

    void OnEnable()
    {
        BackButton?.onClick.AddListener(BackButtonClickedHandler);
        NextLevelButton?.onClick.AddListener(NextLevelButtonClickedHandler);
    }
    void OnDisable()
    {
        BackButton?.onClick.RemoveListener(BackButtonClickedHandler);
        NextLevelButton?.onClick.RemoveListener(NextLevelButtonClickedHandler);
    }

    public void ShowWinPanel(int levelIndex = 1)
    {
        TitleText.text = $"Stage {levelIndex}";
        winPanelCanvasGroup.DOFade(1, 0.5f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                Title_Panel_Win.transform.DOScaleX(1.2f, 0.8f).SetEase(Ease.OutBack).SetDelay(0.2f).
                    OnComplete(() =>
                    {
                        ShowStarsAnimated(Star.Length);
                    });
            });
        winPanelCanvasGroup.interactable = true;
        winPanelCanvasGroup.blocksRaycasts = true;
    }

    public void HideWinPanel()
    {
        winPanelCanvasGroup.DOFade(0, 0.5f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                winPanelCanvasGroup.interactable = false;
                winPanelCanvasGroup.blocksRaycasts = false;

                Title_Panel_Win.transform.localScale = new Vector3(0, 1, 1);

                Panel_Button.transform.localPosition = originalButtonPosition + Vector3.down * 2000f;

                foreach (var star in Star)
                {
                    star.transform.localScale = Vector3.zero;
                    star.SetActive(false);
                }
                NextLevelButton.gameObject.SetActive(true);
                NextLevelButton.interactable = true;
                BackButton.interactable = true;

                // foreach (var particle in Particle)
                // {
                //     particle.SetActive(false);
                // }
            });
    }

    public void ShowStarsAnimated(int starCount)
    {
        PlayStarSequence(starCount);
    }

    private void PlayStarSequence(int count)
    {
        Sequence starSequence = DOTween.Sequence();

        int[][] starGroups =
        {
        new[] { 2 },
        new[] { 1, 3 },
        new[] { 0, 4 }
    };
        Vector3[] scales =
        {
        new Vector3(1.5f, 1.5f, 1),
        new Vector3(1.0f, 1.0f, 1),
        new Vector3(0.7f, 0.7f, 1)
    };

        for (int groupIndex = 0; groupIndex < starGroups.Length; groupIndex++)
        {
            foreach (int i in starGroups[groupIndex])
            {
                if (i < count && i < Star.Length && Star[i] != null)
                {
                    Star[i].SetActive(true);
                    Star[i].transform.localScale = Vector3.zero;
                    starSequence.Join(
                        Star[i].transform.DOScale(scales[groupIndex], 0.4f).SetEase(Ease.OutBack)
                    );
                }
            }
            starSequence.AppendInterval(0.3f);
        }
        starSequence.Append(
            Panel_Button.transform.DOLocalMove(originalButtonPosition, 0.6f).SetEase(Ease.OutBack)
        );
    }


    public void HideButtonNextLevel()
    {
        if (NextLevelButton != null)
        {
            NextLevelButton.gameObject.SetActive(false);
        }
    }

    public void NextLevelButtonClickedHandler()
    {
        if (NextLevelButton != null)
            NextLevelButton.interactable = false; // Chặn nhấn thêm

        NextLevelButtonClicked?.Invoke();
    }

    public void BackButtonClickedHandler()
    {
        if (BackButton != null)
            BackButton.interactable = false;

        BackButtonClicked?.Invoke();
    }
}
