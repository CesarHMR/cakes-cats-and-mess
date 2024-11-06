using audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public bool isBomb;
    public bool isPrize;
    public bool isOpen;
    public bool isFlag;

    public int index;
    public int bombsArround;
    public int prizesArround;
    public int ItemsArround { get { return bombsArround + prizesArround; } }

    [SerializeField] Image image;
    public TextMeshProUGUI text;

    [SerializeField] Color _defaultColor;
    [SerializeField] Color _emptyColor;
    [SerializeField] Color _bombColor;
    [SerializeField] Color _prizeColor;

    [SerializeField] GameObject _flag;


    [SerializeField] Color _defaultTextColor;
    [SerializeField] Color _prizeTextColor;

    private void Awake()
    {
        image.color = _defaultColor;
    }

    public void ButtonClicked()
    {
        if (!isOpen)
            GridManager.OnOpenCell.Invoke(index);
    }
    public void ShowFlag()
    {
        _flag.SetActive(isFlag);
        AudioManager.Instance.PlaySound("flag");
    }
    public void ShowCell()
    {
        isOpen = true;

        if (isBomb)
        {
            AudioManager.Instance.PlaySound("bomb revealed");
            image.color = _bombColor;
        }
        else if (isPrize)
        {
            AudioManager.Instance.PlaySound("prize revealed");
            image.color = _prizeColor;
        }
        else
        {
            AudioManager.Instance.PlaySound("tile revealed");
            image.color = _emptyColor;
        }

        if (isPrize || isBomb)
        {
            text.color = Color.clear;
        }
        else if (prizesArround > 0)
        {
            text.color = _prizeTextColor;
        }
        else if (bombsArround > 0)
        {
            text.color = _defaultTextColor;
        }
        else
        {
            text.color = Color.clear;
        }

        text.SetText((bombsArround + prizesArround).ToString());
    }
}
