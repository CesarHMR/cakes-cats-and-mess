using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public bool flag;
    [SerializeField] int _xSize;
    [SerializeField] int _ySize;
    [SerializeField] int _bombsAmount;
    [SerializeField] int _prizesAmount;
    [SerializeField] float _timeBetweenShow;
    int flagsOnTile;
    int discoveredPrizes;
    bool _canPlay = true;
    bool isPlaying = false;

    [SerializeField] TextMeshProUGUI _textBomb;
    [SerializeField] TextMeshProUGUI _textPrize;
    [SerializeField] TextMeshProUGUI _textPrizeCollected;
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _losePanel;

    [SerializeField] Timer _timer;

    Cell[] _gameCells;
    GridBuilder _gridBuilder;

    public static CellEvent OnOpenCell = new CellEvent();

    List<Cell> _cellsToOpen = new List<Cell>();

    private void Awake()
    {
        _gridBuilder = GetComponent<GridBuilder>();
        _gameCells = _gridBuilder.BuildGrid(_xSize, _ySize, _bombsAmount, _prizesAmount);
        OnOpenCell.AddListener(ClearArea);
        OnOpenCell.AddListener(CheckWin);

        _textBomb.SetText(_bombsAmount.ToString());
        _textPrize.SetText(_prizesAmount.ToString());
    }

    void ClearArea(int index)
    {
        if(isPlaying == false)
        {
            isPlaying = true;
            _timer.StartTimer();
        }


        if (_canPlay == false) return;

        if (flag)
        {
            _gameCells[index].isFlag = !_gameCells[index].isFlag;
            _gameCells[index].ShowFlag();

            flagsOnTile += _gameCells[index].isFlag ? 1 : -1;
            _textBomb.SetText((_bombsAmount - flagsOnTile).ToString());

            return;
        }

        if (_gameCells[index].isFlag) return;

        if (_gameCells[index].isPrize)
        {
            discoveredPrizes++;
            _textPrize.SetText((_prizesAmount - discoveredPrizes).ToString());
        }

        AddAndCheckArround(index);

        StartCoroutine(ShowCellsGradually());

    }

    IEnumerator ShowCellsGradually()
    {
        _canPlay = false;
        foreach (Cell cell in _cellsToOpen)
        {
            cell.ShowCell();
            yield return new WaitForSeconds(_timeBetweenShow);
        }
        _cellsToOpen.Clear();
        _canPlay = true;
    }

    void AddAndCheckArround(int index)
    {
        _cellsToOpen.Add(_gameCells[index]);

        if (_gameCells[index].ItemsArround != 0 || _gameCells[index].isBomb || _gameCells[index].isPrize) return;

        Cell[] cellsArround = new Cell[8];

        cellsArround[0] = GetLeft(index, _xSize) >= 0 && GetLeft(index, _xSize) < _gameCells.Length ? _gameCells[GetLeft(index, _xSize)] : null;
        cellsArround[1] = GetRight(index, _xSize, _gameCells) >= 0 && GetRight(index, _xSize, _gameCells) < _gameCells.Length ? _gameCells[GetRight(index, _xSize, _gameCells)] : null;
        cellsArround[2] = GetTop(index, _xSize) >= 0 && GetTop(index, _xSize) < _gameCells.Length ? _gameCells[GetTop(index, _xSize)] : null;
        cellsArround[3] = GetBot(index, _xSize) >= 0 && GetBot(index, _xSize) < _gameCells.Length ? _gameCells[GetBot(index, _xSize)] : null;
        cellsArround[4] = GetTopLeft(index, _xSize) >= 0 && GetTopLeft(index, _xSize) < _gameCells.Length ? _gameCells[GetTopLeft(index, _xSize)] : null;
        cellsArround[5] = GetTopRight(index, _xSize) >= 0 && GetTopRight(index, _xSize) < _gameCells.Length ? _gameCells[GetTopRight(index, _xSize)] : null;
        cellsArround[6] = GetBotLeft(index, _xSize) >= 0 && GetBotLeft(index, _xSize) < _gameCells.Length ? _gameCells[GetBotLeft(index, _xSize)] : null;
        cellsArround[7] = GetBotRight(index, _xSize) >= 0 && GetBotRight(index, _xSize) < _gameCells.Length ? _gameCells[GetBotRight(index, _xSize)] : null;

        for (int i = 0; i < cellsArround.Length; i++)
        {
            if (cellsArround[i] != null && !_cellsToOpen.Contains(cellsArround[i]) && !cellsArround[i].isFlag)
            {
                AddAndCheckArround(cellsArround[i].index);
            }
        }
    }

    public void CheckWin(int cellIndex)
    {
        bool won = true;

        foreach (var cell in _gameCells)
        {
            if (cell.isBomb && cell.isOpen)
            {
                Debug.Log("Lose");
                _losePanel.SetActive(true);
                _timer.StopTimer();
            }

            if (!cell.isBomb && !cell.isPrize && !cell.isOpen)
            {
                won = false;
            }
        }

        if (won)
        {
            Debug.Log("Won");

            int collected = 0;

            foreach (var cell in _gameCells)
            {
                if (cell.isPrize && cell.isOpen) collected++;
            }

            _textPrizeCollected.SetText(collected + " of " + _prizesAmount);
            _winPanel.SetActive(true);
            _timer.StopTimer();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public class CellEvent : UnityEvent<int> { }

    int GetTop(int index, int xSize)
    {
        return index - xSize;
    }

    int GetBot(int index, int xSize)
    {
        return index + xSize;
    }

    int GetLeft(int index, int xSize)
    {
        if (index == 0 || index % xSize == 0)
            return -1;

        return index - 1;
    }

    int GetRight(int index, int xSize, Cell[] cells)
    {
        if (index + 1 >= cells.Length) return -1;

        if ((index + 1) % xSize == 0)
            return -1;

        return index + 1;
    }

    int GetTopLeft(int index, int xSize)
    {
        if ((index - xSize) % xSize == 0) return -1;

        return index - xSize - 1;
    }
    int GetTopRight(int index, int xSize)
    {
        if ((index - xSize + 1) % xSize == 0) return -1;

        return index - xSize + 1;
    }
    int GetBotLeft(int index, int xSize)
    {
        if ((index + xSize) % xSize == 0) return -1;

        return index + xSize - 1;
    }
    int GetBotRight(int index, int xSize)
    {
        if ((index + 1) % xSize == 0) return -1;

        return index + xSize + 1;
    }
}
