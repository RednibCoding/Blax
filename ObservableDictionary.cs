using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blax;

public interface IObservableDictionary
{
    /// <summary>
    /// Occurs when the dictionary is changed.
    /// </summary>
    event Action? DictionaryChanged;
}

/// <summary>
/// Represents a dictionary that raises an event when it changes.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IObservableDictionary where TKey : notnull
{
    /// <summary>
    /// Occurs when the dictionary is changed.
    /// </summary>
    public event Action? DictionaryChanged;

    /// <summary>
    /// Adds the specified key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        DictionaryChanged?.Invoke();
    }

    /// <summary>
    /// Removes the value with the specified key from the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>true if the element is successfully found and removed; otherwise, false.</returns>
    public new bool Remove(TKey key)
    {
        var result = base.Remove(key);
        if (result)
        {
            DictionaryChanged?.Invoke();
        }
        return result;
    }

    /// <summary>
    /// Removes all keys and values from the dictionary.
    /// </summary>
    public new void Clear()
    {
        base.Clear();
        DictionaryChanged?.Invoke();
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>The value associated with the specified key.</returns>
    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            base[key] = value;
            DictionaryChanged?.Invoke();
        }
    }

    /// <summary>
    /// Adds multiple key-value pairs to the dictionary from another dictionary.
    /// </summary>
    /// <param name="items">The items to add to the dictionary.</param>
    public void AddRange(IDictionary<TKey, TValue> items)
    {
        foreach (var item in items)
        {
            base.Add(item.Key, item.Value);
        }
        DictionaryChanged?.Invoke();
    }

    /// <summary>
    /// Updates the value for a given key.
    /// </summary>
    /// <param name="key">The key to update.</param>
    /// <param name="value">The new value.</param>
    /// <returns>true if the key exists and the value was updated; otherwise, false.</returns>
    public bool Update(TKey key, TValue value)
    {
        if (ContainsKey(key))
        {
            base[key] = value;
            DictionaryChanged?.Invoke();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks whether the dictionary contains a key-value pair.
    /// </summary>
    /// <param name="key">The key to locate in the dictionary.</param>
    /// <param name="value">The value to locate in the dictionary.</param>
    /// <returns>true if the dictionary contains an element with the specified key and value; otherwise, false.</returns>
    public bool ContainsEntry(TKey key, TValue value)
    {
        return TryGetValue(key, out var existingValue) && EqualityComparer<TValue>.Default.Equals(existingValue, value);
    }

    /// <summary>
    /// Removes multiple keys from the dictionary.
    /// </summary>
    /// <param name="keys">The keys to remove.</param>
    public void RemoveRange(IEnumerable<TKey> keys)
    {
        foreach (var key in keys)
        {
            base.Remove(key);
        }
        DictionaryChanged?.Invoke();
    }
}
