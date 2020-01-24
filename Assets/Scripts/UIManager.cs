using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject createGamePane;
    public void OnNewGame()
    {
        createGamePane.SetActive(true);
    }

    public InputField rowInput, colInput, mineCountInput;
    public Text tips;
    public Text time;

    public void setTime(float time)
    {
        this.time.text = (int)time + "";
    }
    public void CancelCreateGame()
    {
        createGamePane.SetActive(false);
    }

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void RowOnChange()
    {
        int row;
        if (!int.TryParse(rowInput.text, out row)) row = 1;
        if (row >= 24) rowInput.text = "24";
        if (row < 1) rowInput.text = "1";
    }
    public void ColOnChange()
    {
        int col;
        if (!int.TryParse(colInput.text, out col)) col = 1;
        if (col >= 30) colInput.text = "30";
        if (col < 1) colInput.text = "1";
    }
    public void MineCountOnChange()
    {
        int row;
        if (!int.TryParse(rowInput.text, out row)) row = 1;
        int col;
        if (!int.TryParse(colInput.text, out col)) col = 1;
        int mineCount;
        if (!int.TryParse(mineCountInput.text, out mineCount)) mineCount = 1;
        if (mineCount >= row * col) mineCountInput.text = row * col- 1 + "";
    }

    public void CreateGame()
    {
        int row;
        if (!int.TryParse(rowInput.text, out row)) row = 1;
        int col;
        if (!int.TryParse(colInput.text, out col)) col = 1;
        int mineCount;
        if (!int.TryParse(mineCountInput.text, out mineCount)) mineCount = 1;
        gameManager.NewGame(row, col, mineCount);
        createGamePane.SetActive(false);
    }

    public GameObject winPane;
    public void ShowWin()
    {
        StartCoroutine(ShowWinAndClose());
    }
    private IEnumerator ShowWinAndClose()
    {
        winPane.SetActive(true);
        yield return new WaitForSeconds(3);
        winPane.SetActive(false);
    }
}
