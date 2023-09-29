using System;
using System.Collections.Generic;


namespace Blax;

public interface IObservableList
{
    event Action? ListChanged;
}

/// <summary>
/// Represents a list that raises an event when it changes.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
public class ObservableList<T> : List<T>, IObservableList
{
    /// <summary>
    /// Occurs when the list is changed.
    /// </summary>
    public event Action? ListChanged;

    /// <summary>
    /// Adds an object to the end of the List<T>.
    /// </summary>
    public new void Add(T item)
    {
        base.Add(item);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Adds the elements of the specified collection to the end of the List<T>.
    /// </summary>
    public new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the specified predicate.
    /// </summary>
    public new int RemoveAll(Predicate<T> match)
    {
        var result = base.RemoveAll(match);
        if (result > 0) ListChanged?.Invoke();
        return result;
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the List<T>.
    /// </summary>
    public new bool Remove(T item)
    {
        var result = base.Remove(item);
        if (result) ListChanged?.Invoke();
        return result;
    }

    /// <summary>
    /// Removes the all the elements that match the conditions defined by the specified predicate.
    /// </summary>
    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Removes a range of elements from the List<T>.
    /// </summary>
    public new void RemoveRange(int index, int count)
    {
        base.RemoveRange(index, count);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Removes all elements from the List<T>.
    /// </summary>
    public new void Clear()
    {
        base.Clear();
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Inserts an element into the List<T> at the specified index.
    /// </summary>
    public new void Insert(int index, T item)
    {
        base.Insert(index, item);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Inserts the elements of a collection into the List<T> at the specified index.
    /// </summary>
    public new void InsertRange(int index, IEnumerable<T> collection)
    {
        base.InsertRange(index, collection);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Sorts the elements in the entire List<T> using the default comparer.
    /// </summary>
    public new void Sort()
    {
        base.Sort();
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Sorts the elements in the entire List<T> using the specified comparer.
    /// </summary>
    public new void Sort(IComparer<T> comparer)
    {
        base.Sort(comparer);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Sorts the elements in the entire List<T> using the specified Comparison<T>.
    /// </summary>
    public new void Sort(Comparison<T> comparison)
    {
        base.Sort(comparison);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Sorts the elements in a range of elements in List<T> using the specified comparer.
    /// </summary>
    public new void Sort(int index, int count, IComparer<T> comparer)
    {
        base.Sort(index, count, comparer);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Reverses the elements in the entire List<T>.
    /// </summary>
    public new void Reverse()
    {
        base.Reverse();
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Reverses the elements in a range of elements in List<T>.
    /// </summary>
    public new void Reverse(int index, int count)
    {
        base.Reverse(index, count);
        ListChanged?.Invoke();
    }

    /// <summary>
    /// Sets or gets the element at the specified index.
    /// </summary>
    public new T this[int index]
    {
        get => base[index];
        set
        {
            base[index] = value;
            ListChanged?.Invoke();
        }
    }

    /// <summary>
    /// Sets the capacity to the actual number of elements in the List<T>, if that number is less than a threshold value.
    /// </summary>
    public new void TrimExcess()
    {
        base.TrimExcess();
        ListChanged?.Invoke();
    }
}

