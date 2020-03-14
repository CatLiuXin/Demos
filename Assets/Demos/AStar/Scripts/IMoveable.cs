using System;

public interface IMoveable 
{
    ValueTuple<int,int> Pos { get; set; }
    void MoveTo(ValueTuple<int,int> pos);
}
