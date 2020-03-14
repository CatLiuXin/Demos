using System;
using System.Collections.Generic;
public class AStarMarkMove : IMoveable
{
    private ValueTuple<int, int> pos = new ValueTuple<int, int>(0,0);
    List<(int, int)> path;
    int nowPath;

    Action<ValueTuple<int, int>> onMoveTo;
    Func<ValueTuple<int, int>, bool> canMoveTo;
    Action onCantMove;

    public AStarMarkMove(Action<ValueTuple<int, int>> onMoveTo,
        Func<ValueTuple<int,int>,bool> canMoveTo,Action onCantMove)
    {
        this.onMoveTo = onMoveTo;
        this.canMoveTo = canMoveTo;
        this.onCantMove = onCantMove;
    }

    public ValueTuple<int, int> Pos { get => pos; set => pos = value; }

    public void MoveTo(ValueTuple<int, int> pos)
    {
        if (canMoveTo(pos))
        {
            onMoveTo(pos);
        }
        else
        {
            onCantMove();
        }
    }

    public void Move(List<(int,int)> path)
    {
        this.path = path;
        nowPath = 0;
        if (CanToNext())
        {
            MoveToNext();
        }
    }
    public bool CanToNext()
    {
        if (path == null || nowPath >= path.Count) return false;
        return true;
    }

    public void MoveToNext()
    {
        MoveTo(path[nowPath++]);
    }
}
