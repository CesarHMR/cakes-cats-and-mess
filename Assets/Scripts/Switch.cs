using audio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] GridManager _gridManager;

    public void SwitchFlag()
    {
        _gridManager.flag = !_gridManager.flag;

        _textMeshPro.SetText(_gridManager.flag ? "flag" : "reveal");

        AudioManager.Instance.PlaySound("button");
    }
}
