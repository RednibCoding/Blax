# Blax State Management Library for Blazor

![](icon.png)

## Introduction

Blax is a robust and flexible state management library designed specifically for Blazor applications.
It provides a simple yet powerful way to manage state across components, allowing you to build reactive and maintainable UIs.
With Blax, you can manage state without the boilerplate, making your Blazor apps even more awesome!

## Features

- Reactive Observables: Keep your UI in sync with the state automatically.
- Attributes-based State: Decorate your properties with [Observable] to make them easily observable.
- Type-safe API: Use a straightforward and type-safe API to manage state.
- Easy to Use: A simple API that feels native to C# and Blazor.
- Minimal Code: Minimal code to get started.

## Installation

Install the package via NuGet Package Manager:

```
Install-Package Blax
```

Or via the .NET CLI:

```
dotnet add package Blax
```

## Quick Start

### Create a State Class

Create a class that derives from **ObservableState** to represent your state, and decorate any properties you want to observe with the **[Observable]** attribute.

```cs
using Blax;

public class MyState : ObservableState
{
    [Observable]
    public virtual int Counter { get; set; }
}
```

> _Note:_ Any property decorated with the **[Observable]** attribute must be **virtual**

### Instantiate State in Program.cs/Startup.cs

Instantiate your state class in the Program.cs or Startup.cs and provide it to your components.

```cs
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        // Use the CreateInstance method provided by the ObservableState class as shown below
        builder.Services.AddScoped(sp => ObservableState.CreateInstance<MyState>());

        await builder.Build().RunAsync();
    }
}
```

### Use the State in a Razor Component

Inject the state into your component and use it directly in your methods and UI.

To ensure that components re-render when the state changes, wrap them with an Observer element.

The State attribute should be set to the state instance you wish to observe.

```html
@inject MyState myState

<Observer State="myState"
    <button @onclick="Increment">Increment</button>
    <p>@State.Counter</p>
</Observer>

@code {
    private void Increment()
    {
        State.Counter++;
    }
}
```

## API Reference

### Observable Attribute

Use the **[Observable]** attribute to make a property observable:

```cs
[Observable]
public virtual int MyProperty { get; set; }
```

> _Note:_ Any property decorated with the **[Observable]** attribute must be **virtual**

### Observable Lists and Dictionaries

Blax also includes ` ObservableList<T>` and `ObservableDictionary<TKey, TValue>` for your collections.

```cs
[Observable]
public virtual ObservableList<string> MyList { get; set; }

[Observable]
public virtual ObservableDictionary<int, string> MyDict { get; set; }

```

## Examples

### Counter Example

```cs
// AppState.cs
public class AppState : ObservableState
{
    [Observable]
    public virtual int Counter { get; set; }
}

// CounterComponent.razor
@inject AppState appState

<Observer State="appState">
    <button @onclick="Increment">Increment</button>
    <p>@appState.Counter</p>
</Observer>

@code {
    private void Increment()
    {
        appState.Counter++;
    }
}

```

### Todo List Example

**AppState.cs**

```cs
// AppState.cs
using Blax;

public class AppState : ObservableState
{
    [Observable]
    public virtual ObservableList<string> Todos { get; set; } = new();
}
```

**Program.cs or Startup.cs**

```cs
// Program.cs
// Add a scoped instance of our state
builder.Services.AddScoped(sp => ObservableState.CreateInstance<AppState>());

// or Startup.cs
services.AddScoped(sp => ObservableState.CreateInstance<AppState>());
```

**Index.razor**

```cs
// Index.razor
@page "/"
@using Blax
@using Example.Components
@using Example.States;

@inject AppState appState


<input @bind="newTodo" />
<button @onclick="AddTodo">Add</button>

<TodoDrawer />

@code {
    string newTodo = string.Empty;

    private void AddTodo()
    {
        if (!string.IsNullOrEmpty(newTodo))
        {
            appState.Todos.Add(newTodo);
            newTodo = string.Empty;
        }
    }
}

```

**TodoDrawer.razor**

```cs
// TodoDrawer.razor
@using Blax
@using Example.States;
@inject AppState appState

<h3>TodoDrawer</h3>

@* Note: Try removing the Observer and leave just the foreach loop. You will notice that the newly-created todos are not displayed, because the page no longer updates automatically. *@

<Observer State="appState">
    @foreach (var todo in appState.Todos)
    {
        <p>@todo</p>
    }
</Observer>
```

## Contributing

We welcome contributions! Please see CONTRIBUTING.md for details.

## License

Blax is licensed under the MIT license. See LICENSE for details.
