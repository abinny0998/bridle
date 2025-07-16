using _GAME.Scripts.Popup;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.Views
{
    public class MainView : UIView
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text _tJewel;
        [SerializeField] private TMP_Text _tHighestLevel;
        protected override void OnShowing()
        {
            base.OnShowing();
            canvasGroup.alpha = 0;
            canvasGroup.SetActive(false);
            _tJewel.text = SaveDataHandler.Instance.CurrentJewel.ToString();
            _tHighestLevel.text = "Level " + SaveDataHandler.Instance.HighestLevel.ToString();
        }


        #region MAIN UI BUTTON CALLBACK

        public void OnClickPlayGame()
        {
            // Hide();
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.LevelsPopup);
        }

        public void OnClickOpenSetting()
        {
            var param = new SettingPopupParam
            {
                showGroupBtn = false
            };
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.SettingPopup, param);
        }

        public void BTN_ThemeShop()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.ThemeShopPopup);
        }

        public void BTN_SkinShop()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.ViewManager.ShowView(UIViewName.ShopCharacterView);
        }

        public void BTN_TerrainShop()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            ComingSoon();
        }

        private void ComingSoon()
        {
            canvasGroup.alpha = 1;
            canvasGroup.SetActive(true);
            canvasGroup.DOFade(0f, 1f)
                    .SetDelay(0.5f)
                    .OnComplete(() =>
                    {
                        canvasGroup.gameObject.SetActive(false);
                    });
        }    
        #endregion
    }
}