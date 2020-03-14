using System;

public class Block
{
    Action<bool> onChangeColor;

    public Block(Action<bool> onChangeColor)
    {
        this.onChangeColor = onChangeColor;
        onChangeColor(isBarrier);
    }

    private bool isBarrier = false;
    public bool IsBarrier
    {
        get => isBarrier;
    }
    public bool HaveMark { get => haveMark; }

    private bool haveMark = false;


    public void Change()
    {
        if (!HaveMark)
        {
            isBarrier = !isBarrier;
            onChangeColor(isBarrier);
        }
    }

    public void SetMark(bool haveMark)
    {
        this.haveMark = haveMark;
    }
}
