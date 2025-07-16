using _GAME.Scripts.Views;
using DenkKits.GameServices.Manager;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using Imba.Utils;
using UnityEngine;

namespace _GAME.Scripts.Controllers
{
    public class GameController : ManualSingletonMono<GameController>
    {
        private bool _isGamePaused = false;
        private int _userScore = 0;
        private GameView _gameView;

        public override void Awake()
        {
            base.Awake();
            SoArchitectureManager.Instance.PauseGame.AddListener(PauseGame);
            SoArchitectureManager.Instance.ResumeGame.AddListener(ResumeGame);
        }

        void Start()
        {
            _gameView = UIManager.Instance.ViewManager.GetViewByName<GameView>(UIViewName.GameView);
            UIManager.Instance.ViewManager.ShowView(UIViewName.GameView);
            UIManager.Instance.HideTransition(() => { });
        }

        void Update()
        {
            if (_isGamePaused)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                ShowEndGame();
            }
        }

        public void PauseGame()
        {
            _isGamePaused = true;
        }

        public void ResumeGame()
        {
            _isGamePaused = false;
        }

        /// <summary>
        /// GAME ENDED
        /// </summary>
        public void ShowEndGame()
        {
            PauseGame();
            
            EndGamePopupParam param = new EndGamePopupParam
            {
                score = _userScore,
                isNewHighScore = _userScore > SaveDataHandler.Instance.UserHighScore
            };

            if (_userScore > SaveDataHandler.Instance.UserHighScore)
            {
                SaveDataHandler.Instance.UserHighScore = _userScore;
                SaveDataHandler.Instance.RequestSave();
            }

            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.CompletedPopup, param);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SoArchitectureManager.Instance.PauseGame.RemoveListener(PauseGame);
            SoArchitectureManager.Instance.ResumeGame.RemoveListener(ResumeGame);
        }

        //-----------------------------


    }
}