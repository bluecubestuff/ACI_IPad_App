using System.Collections.Generic;

[System.Serializable]
public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public Pair(Pair<T,U> _pair)
    {
        this.First = _pair.First;
        this.Second = _pair.Second;
    }

    public T First { get; set; }
    public U Second { get; set; }

    public bool isFirst = true;
};