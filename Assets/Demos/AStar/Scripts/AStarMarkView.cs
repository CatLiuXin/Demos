using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class AStarMarkView : MonoBehaviour,IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    private Vector2 offsetPos;
    private bool isStar;
    [SerializeField]
    private AStarMarkMove moveable;
    [SerializeField]
    private static float moveTime = 1f;
    private AStarMap map;
    private static bool CanMove { get; set; } = true;

    public bool IsStar { get => isStar; }
    public IMoveable Moveable { get => moveable;  }

    public void Init(AStarMap map,ValueTuple<int,int> pos,bool isStar)
    {
        this.isStar = isStar;
        this.map = map;
        moveable.MoveTo(pos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CanMove)
        {
            transform.position = eventData.position - offsetPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanMove)
        {
            offsetPos = eventData.position - (Vector2)transform.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CanMove)
        {
            float X = Input.mousePosition.x - Screen.width / 2f + 40;
            float Y = Input.mousePosition.y - Screen.height / 2f + 40;
            moveable.MoveTo(AStarMap.Pos2Index(new Vector2(X, Y)));
        }
    }

    void Awake()
    {
        moveable = new AStarMarkMove((pos) =>
        {
            map.CanChange = false;
            CanMove = false;
            transform.DOLocalMove(AStarMap.GetPos(pos.Item1, pos.Item2), moveTime).onComplete = () =>
              {
                  CanMove = true;
                  map[moveable.Pos.Item1, moveable.Pos.Item2].SetMark(false);
                  var block = map[pos.Item1, pos.Item2];
                  moveable.Pos = pos;
                  block.SetMark(true);
                  map.CanChange = true;
                  if (moveable.CanToNext())
                  {
                      moveable.MoveToNext();
                  }
              };
        }, (pos) =>
        {
            var block = map[pos.Item1, pos.Item2];
            return !(block.IsBarrier);
        }, () =>
        {
            moveable.MoveTo(moveable.Pos);
        });
    }



}
