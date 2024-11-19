using audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileAppearence : MonoBehaviour
{
    public bool isBomb;
    public bool isPrize;
    public bool isOpen;
    public bool isFlag;

    public int index;
    public int bombsArround;
    public int prizesArround;
    public int ItemsArround { get { return bombsArround + prizesArround; } }

    [SerializeField] Image _tileImage;
    public TextMeshProUGUI _tileText;

    [Header("Tile Colors")]
    [SerializeField] Color _defaultTileColor;
    [SerializeField] Color _revealedTileColor;
    [SerializeField] Color _mineTileColor;
    [SerializeField] Color _prizeTileColor;

    [Header("Icons")]
    [SerializeField] GameObject _flagIcon;
    [SerializeField] GameObject _mineIcon;
    [SerializeField] GameObject _prizeIcon;

    [Header("Text Colors")]
    [SerializeField] Color _defaultTextColor;
    [SerializeField] Color _prizeTextColor;

    private void Awake()
    {
        SetAsDefault();
    }

    public void ButtonClicked()
    {
        if (!isOpen)
            GridManager.OnOpenCell.Invoke(index);
    }
    public void ShowFlag()
    {
        _flagIcon.SetActive(isFlag);
        AudioManager.Instance.PlaySound("flag");
    }
    public void ShowCell()
    {
        isOpen = true;

        if (isBomb)
        {
            SetAsMine();

        }
        else if (isPrize)
        {
            SetAsPrize();
        }
        else
        {
            SetAsRevealed();
        }
    }

    void SetAsDefault()
    {
        _tileImage.color = _defaultTileColor;
        _tileText.color = Color.clear;
        _mineIcon.SetActive(false);
        _prizeIcon.SetActive(false);
        _flagIcon.SetActive(false);
    }

    void SetAsRevealed()
    {
        AudioManager.Instance.PlaySound("tile revealed");
        _tileImage.color = _revealedTileColor;

        if (prizesArround > 0)
        {
            _tileText.color = _prizeTextColor;
        }
        else if (bombsArround > 0)
        {
            _tileText.color = _defaultTextColor;
        }
        else
        {
            _tileText.color = Color.clear;
        }

        _tileText.SetText((bombsArround + prizesArround).ToString());
    }

    void SetAsPrize()
    {
        AudioManager.Instance.PlaySound("prize revealed");
        _tileImage.color = _prizeTileColor;
        _tileText.color = Color.clear;
        _prizeIcon.SetActive(true);
    }

    void SetAsMine()
    {
        AudioManager.Instance.PlaySound("bomb revealed");
        _tileImage.color = _mineTileColor;
        _tileText.color = Color.clear;
        _mineIcon.SetActive(true);
    }
}
