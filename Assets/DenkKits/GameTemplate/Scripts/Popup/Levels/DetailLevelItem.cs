using DenkKits.GameServices.SaveData;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.Scripts.Popup
{
    public class DetailLevelItem : MonoBehaviour
    {
        [SerializeField] private Button _bLevel;
        [SerializeField] private TMP_Text _tLevel;
        [SerializeField] private GameObject _gLock;
        private int _level;
        private Action<int> OnClicked;

        public void Set(int level, Action<int> OnClicked)
        {
            this.OnClicked ??= OnClicked;
            _level = level;
            _bLevel.interactable = level <=SaveDataHandler.Instance.HighestLevel;
            _gLock.SetActive(level > SaveDataHandler.Instance.HighestLevel);
            _tLevel.SetActive(level <= SaveDataHandler.Instance.HighestLevel);
            _tLevel.text = _level.ToString();
        }

        public void BTN_OnClickedLevel()
        {
            Debug.Log("LEVEL: " + _level);
            OnClicked?.Invoke(_level);
        }
    }
}