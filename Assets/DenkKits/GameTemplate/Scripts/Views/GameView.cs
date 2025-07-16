using System.Collections;
using _GAME.Scripts.Popup;
using AssetKits.ParticleImage;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME.Scripts.Views
{
    public class GameView : UIView
    {
        [SerializeField] private TMP_Text _tNumberPallet;
        int userScore = 0;
        public override void Awake()
        {
            base.Awake();
        }
        public void ShowQuantityPallet(int currentPallet)
        {
            _tNumberPallet.text = $"{currentPallet}";
        }

        public void OpenSetting()
        {
            var param = new SettingPopupParam
            {
                showGroupBtn = true
            };
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.SettingPopup, param);
        }

        public void BTN_Home()
        {
            this.Hide(true);
            SceneManager.LoadScene("Main");
        }

        public void BTN_Replay()
        {
            this.Hide(true);
            UIManager.Instance.ViewManager.ShowView(UIViewName.GameView);
            SceneManager.LoadScene("Game");
        }
    }
}