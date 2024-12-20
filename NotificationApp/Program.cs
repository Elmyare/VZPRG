﻿using System;
using System.Collections.Generic;

// Интерфейс INotifyer
public interface INotifyer
{
    void Notify(decimal balance);
}

// Класс SMSLowBalanceNotifyer
public class SMSLowBalanceNotifyer : INotifyer
{
    private string _phone;
    private decimal _lowBalanceValue;

    public SMSLowBalanceNotifyer(string phone, decimal lowBalanceValue)
    {
        _phone = phone;
        _lowBalanceValue = lowBalanceValue;
    }

    public void Notify(decimal balance)
    {
        if (balance < _lowBalanceValue)
        {
            Console.WriteLine($"SMSLowBalanceNotifyer: Balance {balance:C} is below the threshold for phone {_phone}");
        }
    }
}

// Класс EMailBalanceChangedNotifyer
public class EMailBalanceChangedNotifyer : INotifyer
{
    private string _email;

    public EMailBalanceChangedNotifyer(string email)
    {
        _email = email;
    }

    public void Notify(decimal balance)
    {
        Console.WriteLine($"EMailBalanceChangedNotifyer: Balance changed to {balance:C}. Email sent to {_email}");
    }
}

// Класс Account
public class Account
{
    private decimal _balance;
    private List<INotifyer> _notifyers;

    public Account()
    {
        _balance = 0;
        _notifyers = new List<INotifyer>();
    }

    public Account(decimal balance)
    {
        _balance = balance;
        _notifyers = new List<INotifyer>();
    }

    public decimal Balance
    {
        get { return _balance; }
    }

    public void AddNotifyer(INotifyer notifyer)
    {
        _notifyers.Add(notifyer);
    }

    public void ChangeBalance(decimal value)
    {
        _balance = value;
        Notification();
    }

    private void Notification()
    {
        foreach (var notifyer in _notifyers)
        {
            notifyer.Notify(_balance);
        }
    }
}

// Тестирование работы системы
public class Program
{
    public static void Main()
    {
        // Создание аккаунта с начальным балансом
        Account account = new Account(100);

        // Добавление уведомления по SMS с порогом низкого баланса
        SMSLowBalanceNotifyer smsNotifyer = new SMSLowBalanceNotifyer("123-456-7890", 50);
        account.AddNotifyer(smsNotifyer);

        // Добавление уведомления по Email при изменении баланса
        EMailBalanceChangedNotifyer emailNotifyer = new EMailBalanceChangedNotifyer("example@example.com");
        account.AddNotifyer(emailNotifyer);

        // Изменение баланса
        Console.WriteLine("Изменение баланса на 40:");
        account.ChangeBalance(40);

        Console.WriteLine("\nИзменение баланса на 80:");
        account.ChangeBalance(80);
    }
}
