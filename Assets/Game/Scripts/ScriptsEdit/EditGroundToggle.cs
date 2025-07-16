using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditGroundToggle : MonoBehaviour
{
    public Image backgroundTarget;
    public Button button;
    public Vector2 positionGround = Vector2.zero;
    public TMP_Text textPos;
    public MapType MapType = MapType.None;
    Action<EditGroundToggle> OnCallback;

    public void OnClicked()
    {
        OnCallback?.Invoke(this);
    }    

    public void SetPositionInArray(Vector2 position, Action<EditGroundToggle> OnCallback)
    {
        this.OnCallback = OnCallback;
        positionGround = position;
        textPos.text = $"({position.x},{position.y})";
    }

    public void ChangeMapType(MapType mapType, Color color)
    {
        if(MapType == mapType) return;
        MapType = mapType;
        backgroundTarget.color = color;
    }

    public void Refresh()
    {
        MapType = MapType.None;
        backgroundTarget.color = Color.white;
    }    
}
