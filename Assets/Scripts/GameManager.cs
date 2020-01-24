using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gridPrefab;
    private Grid[,] grids;

    public float gameTimer { get; private set; } = 0f;
    public bool gameStart = false;
    public bool gameOver = false;
    public bool gameWin = false;
    private int mineCount = 0;
    private int flagCount = 0;
    private int openCount = 0;
    private UIManager uIManager;

    private void Start()
    {
        uIManager = GameObject.FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (gameStart && !gameWin)
        {
            gameTimer += Time.deltaTime;
            uIManager.setTime(gameTimer);
        }
    }

    private void spawnMines()
    {
        int totalGrids = grids.GetLength(0) * grids.GetLength(1);
        int[] seq = new int[totalGrids];
        for (int i = 0; i < seq.Length; ++i)
        {
            seq[i] = i;
        }
        for (int i = seq.Length - 1; i > 0; --i)
        {
            int random = Random.Range(0, i - 1);
            int t = seq[i];
            seq[i] = seq[random];
            seq[random] = t;
        }
        for (int i = 0; i < mineCount; ++i)
        {
            int index = seq[i];
            int r = index / grids.GetLength(1);
            int c = index % grids.GetLength(1);

            grids[r, c].type = GridType.Mine;
        }
    }
    public void NewGame(int row, int col, int mineCount)
    {
        if (grids != null)
        {
            for (int i = 0; i < grids.GetLength(0); ++i)
            {
                for (int j = 0; j < grids.GetLength(1); ++j)
                {
                    GameObject.Destroy(grids[i, j].gameObject);
                }
            }
        }
        grids = new Grid[row, col];
        float centerX = col / 2f;
        float centerY = row / 2f;
        for (int i = 0; i < row; ++i)
        {
            for (int j = 0; j < col; ++j)
            {
                GameObject grid = GameObject.Instantiate(gridPrefab, new Vector3(-centerX + j, -centerY + i, 0f), Quaternion.identity);
                grids[i, j] = grid.GetComponent<Grid>();
                grids[i, j].gridsRow = i;
                grids[i, j].gridsCol = j;
            }
        }
        this.mineCount = mineCount;
        spawnMines();
        gameTimer = 0f;
        uIManager.setTime(gameTimer);
        gameStart = false;
        gameWin = false;
        gameOver = false;
        flagCount = 0;
        openCount = 0;
    }

    public void StartGame()
    {
        if (!gameStart)
        {
            gameStart = true;
            gameTimer = 0f;
        }
    }

    private class Point
    {
        public int x, y;
        public Point(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
    }
    Point[] offset = new Point[] {
        new Point(-1, -1),
        new Point(-1, 0),
        new Point(-1, 1),
        new Point(0, -1),
        new Point(0, 1),
        new Point(1, -1),
        new Point(1, 0),
        new Point(1, 1),
    };

    private bool isInGrid(int r, int c)
    {
        return r >= 0 && r < grids.GetLength(0) && c >= 0 && c < grids.GetLength(1);
    }
    private int CalcGridMineCount(int r, int c) {
        if (grids[r, c].mineCount >= 0) return grids[r, c].mineCount;
        int count = 0;
        for (int i = 0; i < offset.Length; i++)
        {
            int or = r + offset[i].y;
            int oc = c + offset[i].x;
            if (isInGrid(or, oc) && grids[or, oc].type == GridType.Mine)
            {
                ++count;
            }
        }
        grids[r, c].mineCount = count;
        ++openCount;
        if (openCount + mineCount == grids.GetLength(0) * grids.GetLength(1))
        {
            WinGame();
        }
        return count;
    }

    public void BfsGrid(int r, int c) {
        int count = CalcGridMineCount(r, c);
        if (count != 0) return;
        Queue<Point> qs = new Queue<Point>();
        qs.Enqueue(new Point(r, c));
        while (qs.Count > 0)
        {
            Point p = qs.Dequeue();
            if (grids[p.x, p.y].mineCount > 0)
            {
                continue;
            }
            for (int i = 0; i < offset.Length; i++)
            {
                int or = p.x + offset[i].x;
                int oc = p.y + offset[i].y;
                if (isInGrid(or, oc) && grids[or, oc].type == GridType.Normal && grids[or, oc].mineCount < 0)
                {
                    CalcGridMineCount(or, oc);
                    
                    qs.Enqueue(new Point(or, oc));
                }
            }
        }
    }

    public void gameTest()
    {
        NewGame(10, 10, 10);
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void ChangeFlag(bool isMine, bool add)
    {
        if (isMine) {
            if (add)
            {
                ++flagCount;
                if (flagCount >= mineCount)
                {
                    WinGame();
                }
            }
            else --flagCount;
        }
    }

    private void WinGame()
    {
        gameWin = true;
        uIManager.ShowWin();
    }
}
