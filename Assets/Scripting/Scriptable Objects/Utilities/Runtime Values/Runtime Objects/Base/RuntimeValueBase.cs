using UnityEngine;

public abstract class RuntimeValueBase<T> : ScriptableObject where T : class
{
    public T Item;

    public void SetItem(T item)
    {
        Item = item;
    }

    public void RemoveItem(T item)
    {
        if (item == Item)
            Item = null;
    }

    public void NullItem()
    {
        Item = null;
    }
}