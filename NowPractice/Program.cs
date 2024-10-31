using System;
using System.Collections.Generic;

public interface IButton
{
    public void Print();
}

public interface ICheckBox
{
    public void Render();
}

public class WindowsButton : IButton
{
    public void Print()
    {
        Console.WriteLine("Windows button");
    }
}

public class WindowsCheckBox : ICheckBox
{
    public void Render()
    {
        Console.WriteLine("Windows checkbox");
    }
}

public class MacOSButton : IButton
{
    public void Print()
    {
        Console.WriteLine("MacOS button");
    }
}

public class MacOSCheckBox : ICheckBox
{
    public void Render()
    {
        Console.WriteLine("MacOS checkbox");
    }
}

public interface IUIFactory
{
    public IButton CreateButton();
    public ICheckBox CreateCheckBox();
}


public class WindowsFactory: IUIFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ICheckBox CreateCheckBox() => new WindowsCheckBox();
}

public class MacOSFactory: IUIFactory
{
    public IButton CreateButton() => new MacOSButton();
    public ICheckBox CreateCheckBox() => new MacOSCheckBox();
}

public class Program
{
    public static void Main()
    {
        IUIFactory factory = new WindowsFactory();
        ICheckBox checkBox = factory.CreateCheckBox();
        checkBox.Render();
    }
}