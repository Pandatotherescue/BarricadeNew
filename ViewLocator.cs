using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BarricadeNew.ViewModels;

namespace BarricadeNew;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;
        
        if (data.GetType() == typeof(ViewModelBase))
            return new TextBlock { Text = "Invalid ViewModelBase" };

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}