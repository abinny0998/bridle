using Sirenix.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeItem : MonoBehaviour
{
    [SerializeField] private Image _iPreview;
    [SerializeField] private TMP_Text _tPreview;
    [SerializeField] private GameObject[] _btns = new GameObject[3];
    private Action<int> OnEquip;
    private ShopThemeModel _model;

    public void Set(ShopThemeModel model, Action<int> OnEquip)
    {
        this.OnEquip = OnEquip;
        RefreshButton(model);
        _iPreview.sprite = model.Preview;
        if (model.IsLock)
            _tPreview.text = $"You need to reach level {model.LevelReach} to unlock";
        else _tPreview.text = $"Already owned";
    }

    public void RefreshButton(ShopThemeModel model)
    {
        this._model = model;
        _btns.ForEach(x => x.SetActive(false));
        if (model.IsEquip)
            _btns[1].SetActive(true);
        else if (model.IsLock)
            _btns[^1].SetActive(true);
        else
            _btns[0].SetActive(true);
    }

    public void BTN_Equip()
    {
        _btns.ForEach(x => x.SetActive(false));
        _btns[1].SetActive(true);
        OnEquip(this._model.Id);
    }
}
