using System;
using System.Collections.Generic;
using System.Reflection;
using Blax;
using Castle.DynamicProxy;


namespace Blax;
public class ObservableState
{
    private readonly List<Action> _subscribers = new();
    private static readonly ProxyGenerator _generator = new();
    private object? _state;

    public ObservableState()
    {
        ValidateObservableProperties(GetType());
    }

    public static T CreateInstance<T>() where T : ObservableState, new()
    {
        var instance = new T();
        instance.InitializeProxy();
        return (T)instance._state!;
    }

    private void InitializeProxy()
    {
        if (_state == null)
        {
            var interceptor = new StateInterceptor();
            _state = _generator.CreateClassProxy(GetType(), interceptor);
        }
    }

    public object? State => _state;

    internal void Subscribe(Action callback)
    {
        if (_subscribers.Contains(callback)) return;

        _subscribers.Add(callback);

        var properties = GetType().GetProperties();

        foreach (var property in properties)
        {
            // Auto-subscribe ObservableList properties
            if (Attribute.IsDefined(property, typeof(ObservableAttribute)) && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableList<>))
            {
                var list = property.GetValue(this) as IObservableList;
                if (list != null)
                {
                    SubscribeList(list);
                }
            }

            // Auto-subscribe ObservableDictionary properties
            else if (Attribute.IsDefined(property, typeof(ObservableAttribute)) && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableDictionary<,>))
            {
                var dict = property.GetValue(this) as IObservableDictionary;
                if (dict != null)
                {
                    SubscribeDict(dict);
                }
            }
        }
    }

    internal void Unsubscribe(Action callback)
    {
        while (_subscribers.Remove(callback));

        var properties = GetType().GetProperties();

        foreach (var property in properties)
        {
            // Auto-subscribe ObservableList properties
            if (Attribute.IsDefined(property, typeof(ObservableAttribute)) && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableList<>))
            {
                var list = property.GetValue(this) as IObservableList;
                if (list != null)
                {
                    UnsubscribeList(list);
                }
            }

            // Auto-subscribe ObservableDictionary properties
            else if (Attribute.IsDefined(property, typeof(ObservableAttribute)) && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableDictionary<,>))
            {
                var dict = property.GetValue(this) as IObservableDictionary;
                if (dict != null)
                {
                    UnsubscribeDict(dict);
                }
            }
        }
    }

    internal void NotifySubscribers()
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Invoke();
        }
    }

    private void SubscribeList(IObservableList list)
    {
        list.ListChanged += NotifySubscribers;
    }

    private void UnsubscribeList(IObservableList list)
    {
        list.ListChanged -= NotifySubscribers;
    }

    private void SubscribeDict(IObservableDictionary dict)
    {
        dict.DictionaryChanged += NotifySubscribers;
    }

    private void UnsubscribeDict(IObservableDictionary dict)
    {
        dict.DictionaryChanged -= NotifySubscribers;
    }

    private void ValidateObservableProperties(Type type)
    {
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            var isObservable = Attribute.IsDefined(property, typeof(ObservableAttribute));
            if (isObservable && !(property?.GetMethod?.IsVirtual ?? false))
            {
                var error = $"The property '{property?.Name}' in type '{type.FullName}' is marked as [Observable] but is not virtual. [Observable] properties must be virtual.";
                throw new InvalidOperationException(error);
            }
        }
    }
}