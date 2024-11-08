using System;

// Абстрактный продукт
abstract class Product
{
    public abstract string GetInfo();
    public abstract void Transform();
}

// Конкретный продукт 1
class ConcreteProduct1 : Product
{
    private string info;

    public ConcreteProduct1(string info)
    {
        this.info = info.ToLower(); // Преобразуем к нижнему регистру
    }

    public override string GetInfo()
    {
        return info;
    }

    public override void Transform()
    {
        // Добавляем пробел после каждого символа, кроме последнего
        var transformed = "";
        for (int i = 0; i < info.Length; i++)
        {
            transformed += info[i];
            if (i < info.Length - 1 && info[i] != ' ')
            {
                transformed += " ";
            }
        }
        info = transformed;
    }
}

// Конкретный продукт 2
class ConcreteProduct2 : Product
{
    private string info;

    public ConcreteProduct2(string info)
    {
        this.info = info.ToUpper(); // Преобразуем к верхнему регистру
    }

    public override string GetInfo()
    {
        return info;
    }

    public override void Transform()
    {
        // Добавляем "**" после каждого символа, кроме последнего
        var transformed = "";
        for (int i = 0; i < info.Length; i++)
        {
            transformed += info[i];
            if (i < info.Length - 1 && info[i] != '*')
            {
                transformed += "**";
            }
        }
        info = transformed;
    }
}

// Абстрактный создатель
abstract class Creator
{
    public abstract Product FactoryMethod(string info);

    public string AnOperation(string info)
    {
        Product product = FactoryMethod(info);
        product.Transform();
        product.Transform();
        return product.GetInfo();
    }
}

// Конкретный создатель 1
class ConcreteCreator1 : Creator
{
    public override Product FactoryMethod(string info)
    {
        return new ConcreteProduct1(info);
    }
}

// Конкретный создатель 2
class ConcreteCreator2 : Creator
{
    public override Product FactoryMethod(string info)
    {
        return new ConcreteProduct2(info);
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        Creator creator1 = new ConcreteCreator1();
        Creator creator2 = new ConcreteCreator2();

        string result1 = creator1.AnOperation("Example Text");
        string result2 = creator2.AnOperation("Example Text");

        Console.WriteLine("Result from ConcreteCreator1: " + result1);
        Console.WriteLine("Result from ConcreteCreator2: " + result2);
    }
}
