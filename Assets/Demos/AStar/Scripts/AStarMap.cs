using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class AStarMap : MonoBehaviour
{
    private bool canChange = true;
    private AStarMarkView star;
    private AStarMarkView target;
    AStarCore core;

    public Color BarrierColor = new Color(0, 0, 0);
    public Color PathColor = new Color(1, 1, 1);
    public float ChangeColorDur = 0.5f;
    public const int count = 10;

    public GameObject BlockPrefab;
    public Block[,] map = new Block[10, 10];
    
    public Block this[int x,int y]
    {
        get => map[x, y];
    }

    public bool CanChange
    {
        get => canChange;
        set => canChange = value;
    }

    void Awake()
    {
        CreateMap();
        CreateMoveable();
        core = new AStarCore(map);
    }

    public void FindPath()
    {
        var ans = new List<(int, int)>();
        if (core.FindPath(star.Moveable.Pos, target.Moveable.Pos, ans))
        {
            AStarMarkMove move = star.Moveable as AStarMarkMove;
            move.Move(ans);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FindPath();
        }
    }

    private void CreateMap()
    {
        for(int i = 0; i < count; i++)
        {
            for(int j = 0; j < count; j++)
            {
                GameObject go = GameObject.Instantiate(BlockPrefab);
                var image = go.GetComponent<Image>();
                image.raycastTarget = false;
                go.transform.SetParent(transform);
                go.transform.localPosition = GetPos(i, j);
                map[i, j] = new Block((isBarr) =>
                 {
                     image.DOColor(isBarr ? BarrierColor : PathColor,ChangeColorDur);
                 });
            }
        }
    }

    public void CreateMoveable()
    {
        var targetGO = Instantiate(Resources.Load<GameObject>("Target"));
        target = targetGO.AddComponent<AStarMarkView>();
        targetGO.transform.SetParent(transform);
        target.Init(this, new ValueTuple<int, int>(count - 1, count - 1), false);

        var starGO = Instantiate(Resources.Load<GameObject>("Star"));
        star = starGO.AddComponent<AStarMarkView>();
        starGO.transform.SetParent(transform);
        star.Init(this, new ValueTuple<int, int>(0, 0), true);
    }

    public static Vector2 GetPos(int x,int y)
    {
        return new Vector2(-450 + x * 100, -450 + y * 100);
    }

    public static ValueTuple<int,int> Pos2Index(Vector2 pos)
    {
        return new ValueTuple<int, int>((((int)pos.x + 450) / 100).Between(0,count-1), 
           ( ((int)pos.y + 450) / 100).Between(0,count-1));
    }
}
