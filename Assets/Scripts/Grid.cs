using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridType
{
    Normal = 0,
    Mine = 1,
}

public class Grid : MonoBehaviour
{
    public GridType type = GridType.Normal;
    public int mineCount = -1;  // 周围地雷数
    public bool hasFlag = false;
    public Sprite[] sprites = new Sprite[5];    // 0: normal, 1: mine, 2: flag, 3:hover, 4: down
    public Sprite[] numbers = new Sprite[10];
    public int gridsRow, gridsCol;
    private bool isHover = false;
    private bool isDown = false;
    private SpriteRenderer spriteRenderer;
    public bool vis;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private Sprite preSprite = null;
    private bool isVisible = false;
    private void Update()
    {
        if (!isVisible) return;
        var sprite = getCurrentSprite();
        if (preSprite != sprite)
        {
            spriteRenderer.sprite = sprite;
            preSprite = sprite;
        }

        if (gameManager.gameOver || gameManager.gameWin) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isHover = true;
            if (Input.GetMouseButtonDown(0))
            {
                gameManager.StartGame();
                isDown = true;
            }
            if (Input.GetMouseButtonUp(0) && isDown)
            {
                // 打开格子
                if (hasFlag) return;    // 插旗子时防止点击
                if (type == GridType.Mine)
                {
                    gameManager.GameOver();
                }
                else
                {
                    gameManager.BfsGrid(gridsRow, gridsCol);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                // 插旗
                hasFlag = !hasFlag;
                gameManager.ChangeFlag(type == GridType.Mine, hasFlag);
            }
        }
        else
        {
            isHover = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDown = false;
        }
    }

    private GameManager gameManager;

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private Sprite getCurrentSprite()
    {
        if (gameManager.gameOver && type == GridType.Mine)
        {
            return sprites[1];
        }
        if (mineCount >= 0)
        {
            return numbers[mineCount];
        }
        if (hasFlag)
        {
            return sprites[2];
        }
        if (isDown)
        {
            return sprites[4];
        }
        if (isHover)
        {
            return sprites[3];
        }
        return sprites[0];
    }
}
