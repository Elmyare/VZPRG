using System;
using System.Collections.Generic;

public enum OrderStatus
{
    Pending,
    Paid,
    Shipped,
    Completed
}

public class Order
{
    public int OrderNumber { get; }
    public decimal TotalAmount { get; }
    public OrderStatus Status { get; private set;}
    private List<INotifier> _notifiers;

    public Order(int orderNumber, decimal totalAmount)
    {
        OrderNumber = orderNumber;
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        _notifiers = new List<INotifier>();
    }
    public void Pay(IPaymentMethod paymentMethod) 
    {

        if (Status == OrderStatus.Pending && paymentMethod.ProcessPayment(TotalAmount))
        {
            Status = OrderStatus.Paid;
            NotifyStatusChange("Order has been paid.");
        }
        else
        {
            Console.WriteLine("Payment failed. Order remains pending.");
        }
    }

    public void Ship()
    {
        if (Status == OrderStatus.Paid)
        {
            Status = OrderStatus.Shipped;
            NotifyStatusChange("Order has been shipped.");
        }
    }

    public void Complete()
    {
        if (Status == OrderStatus.Shipped)
        {
            Status = OrderStatus.Completed;
            NotifyStatusChange("Order has been completed.");
        }
    }

    public void NotifyStatusChange(string message)
    {
        foreach (var notifier in _notifiers)
        {
            notifier.Notify(message);
        }
    }

    public void AddNotifier(INotifier notifier)
    {
        _notifiers.Add(notifier);
    }
}

public interface IPaymentMethod
{
    public bool ProcessPayment(decimal amount);
}

public class CreditCardPayment: IPaymentMethod
{
    private string _cardNumber;

    public CreditCardPayment(string cardNumber)
    {
        _cardNumber = cardNumber;
    }

    public bool ProcessPayment(decimal amount)
    {
        return true;
    }
}

public class PayPalPayment: IPaymentMethod
{
    private string _email;

    public PayPalPayment(string email)
    {
        _email = email;
    }

    public bool ProcessPayment(decimal amount)
    {
        return amount<=500m;
    }
}

public interface INotifier
{
    public void Notify(string message);
}

public class EmailNotifier : INotifier
{
    private string _email;
    public EmailNotifier(string email)
    {
        _email = email;
    }
    public void Notify(string message)
    {
        Console.WriteLine($"Email to {_email}: {message}");
    }
}

public class SMSNotifier : INotifier
{
    private string _phone;
    public SMSNotifier(string phone)
    {
        _phone = phone;
    }
    public void Notify(string message)
    {
        Console.WriteLine($"SMS to {_phone}: {message}");
    }
}

public class Program
{
    public static void Main()
    {
        Order order = new Order(123, 600m);

        order.AddNotifier(new EmailNotifier("customer@example.com"));
        order.AddNotifier(new SMSNotifier("123-456-7890"));

        IPaymentMethod paymentMethod = new PayPalPayment("customer@example.com");
        order.Pay(paymentMethod);

        // Доставка заказа
        order.Ship();
        
        // Завершение заказа
        order.Complete();
    }
}