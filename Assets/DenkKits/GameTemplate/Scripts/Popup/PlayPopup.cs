using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME.Scripts.Popup
{
    public class PlayPopupParam
    {
        public int SelectedLevel;
    }
    public class PlayPopup : UIPopup
    {
        [SerializeField] private TMP_Text _tLevel;
        protected override void OnShowing()
        {
            base.OnShowing();
            var param = (PlayPopupParam)Parameter;
            if (param != null)
            {
                _tLevel.text = $"Level {param.SelectedLevel}";
                SaveDataHandler.Instance.CurrentLevel = param.SelectedLevel;
            }
        }

        public void BTN_OpenGame()
        {
            this.Hide();
            AudioManager.Instance.PlayMusic(AudioName.UI_Click);
            UIManager.Instance.ViewManager.HideView(DenkKits.UIManager.Scripts.UIView.UIViewName.MainView);
            UIManager.Instance.ViewManager.ShowView(DenkKits.UIManager.Scripts.UIView.UIViewName.GameView);
            SceneManager.LoadScene("Game");
        }    
    }
}