using System.Collections.Generic;
using UnityEngine;

public class GroundsParent : MonoBehaviour
{
    [SerializeField] private List<Ground> _lstChildGround = new();

    public List<Ground> LstChildGround
    {
        get
        {
            if (_lstChildGround.Count > 0) return _lstChildGround;
            foreach (Transform child in this.transform)
            {
                _lstChildGround.Add(child.GetComponent<Ground>());
            }
            return _lstChildGround;
        }
        set => _lstChildGround = value;
    }
}
