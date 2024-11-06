using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridBuilder : MonoBehaviour
{
    // cell prize
    // cell 2x2
    [SerializeField] GameObject _cellPrefab;
    [SerializeField] GridLayoutGroup _layoutGroup;
    [SerializeField] RectTransform _rectTransform;

    public Cell[] BuildGrid(int xSize, int ySize, int bombsAmount, int prizesAmount)
    {
        Cell[] cells = new Cell[xSize * ySize];

        List<int> allCells = new List<int>();

        int[] bombsIndex = new int[bombsAmount];
        int[] prizesIndex = new int[prizesAmount];

        for (int i = 0; i < xSize * ySize; i++)
        {
            allCells.Add(i);
        }

        for (int i = 0; i < bombsAmount; i++)
        {
            int rand = Random.Range(0, allCells.Count);
            bombsIndex[i] = allCells[rand];
            allCells.RemoveAt(rand);
        }

        for (int i = 0; i < prizesAmount; i++)
        {
            int rand = Random.Range(0, allCells.Count);
            prizesIndex[i] = allCells[rand];
            allCells.RemoveAt(rand);
        }

        for (int i = 0; i < xSize * ySize; i++)
        {
            cells[i] = Instantiate(_cellPrefab, transform).GetComponent<Cell>();

            cells[i].isBomb = bombsIndex.Contains(i);
            cells[i].isPrize = prizesIndex.Contains(i);
        }

        for (int i = 0; i < xSize * ySize; i++)
        {
            int left = GetLeft(i, xSize);
            int right = GetRight(i, xSize, cells);
            int top = GetTop(i, xSize);
            int bot = GetBot(i, xSize);
            int topLeft = GetTopLeft(i, xSize);
            int topRight = GetTopRight(i, xSize);
            int botLeft = GetBotLeft(i, xSize);
            int botRight = GetBotRight(i, xSize);

            int bombsArround = 0;
            int prizesArround = 0;

            if (left >= 0 && cells[left].isBomb) bombsArround++;
            if (left >= 0 && cells[left].isPrize) prizesArround++;
            if (right >= 0 && right < cells.Length && cells[right].isBomb) bombsArround++;
            if (right >= 0 && right < cells.Length && cells[right].isPrize) prizesArround++;
            if (top >= 0 && cells[top].isBomb) bombsArround++;
            if (top >= 0 && cells[top].isPrize) prizesArround++;
            if (bot >= 0 && bot < cells.Length && cells[bot].isBomb) bombsArround++;
            if (bot >= 0 && bot < cells.Length && cells[bot].isPrize) prizesArround++;
            if (topLeft >= 0 && topLeft < cells.Length && cells[topLeft].isBomb) bombsArround++;
            if (topLeft >= 0 && topLeft < cells.Length && cells[topLeft].isPrize) prizesArround++;
            if (topRight >= 0 && topRight < cells.Length && cells[topRight].isBomb) bombsArround++;
            if (topRight >= 0 && topRight < cells.Length && cells[topRight].isPrize) prizesArround++;
            if (botLeft >= 0 && botLeft < cells.Length && cells[botLeft].isBomb) bombsArround++;
            if (botLeft >= 0 && botLeft < cells.Length && cells[botLeft].isPrize) prizesArround++;
            if (botRight >= 0 && botRight < cells.Length && cells[botRight].isBomb) bombsArround++;
            if (botRight >= 0 && botRight < cells.Length && cells[botRight].isPrize) prizesArround++;

            cells[i].bombsArround = bombsArround;
            cells[i].prizesArround = prizesArround;
            cells[i].index = i;
        }
        float xSpacing = (xSize - 1) * _layoutGroup.spacing.x;
        float ySpacing = (ySize - 1) * _layoutGroup.spacing.y;
        _layoutGroup.cellSize = new Vector2((_rectTransform.rect.width - xSpacing) / xSize, (_rectTransform.rect.height - ySpacing) / ySize);

        return cells;
    }

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