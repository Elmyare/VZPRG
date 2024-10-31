using System;
using System.Collections.Generic;

public abstract class Room
{
    public int Number { get; }
    public int Capacity { get; }
    public decimal BasePrice { get; }

    public Room(int number, int capacity, decimal basePrice)
    {
        Number = number;
        Capacity = capacity;
        BasePrice = basePrice;
    }

    public abstract decimal GetFullPrice(int nights);

    public void GetInfo()
    {
        Console.WriteLine($"Number: {Number}, Capacity: {Capacity}, BasePrice: {BasePrice}");
    }
}

public class SingleRoom: Room
{

    public SingleRoom(int number) : base(number, 1, 100) {}

    public override decimal GetFullPrice(int nights)
    {
        return nights*BasePrice;
    }
}

public class DoubleRoom: Room
{
    public DoubleRoom(int number) : base(number, 2, 150) {}

    public override decimal GetFullPrice(int nights)
    {
        return nights*BasePrice*1.5m;
    }
}

public class SuiteRoom: Room
{
    public SuiteRoom(int number) : base(number, 4, 300) {}

    public override decimal GetFullPrice(int nights)
    {
        return nights*BasePrice*2m;
    }
}

// public interface IBookable
// {
//     public bool IsAvaible(DateTime startDate, int nights)

//     public void Book(DateTime startDate, int nights)
// }

public class Reservation
{
    public Room Room { get; }
    public DateTime StartDate { get; }
    public int Nights { get; }

    public Reservation(Room room, DateTime startDate, int nights)
    {
        Room = room;
        StartDate = startDate;
        Nights = nights;
    }

    public void GetInfo()
    {
        Console.WriteLine($"Room number: {Room.Number}, StartDate: {StartDate.ToShortDateString()}, Nights: {Nights}");
    }
}

public static class RoomFactory
{
    public static Room CreateRoom(string type, int number)
    {
        return type.ToLower() switch
        {
            "single" => new SingleRoom(number),
            "double" => new DoubleRoom(number),
            "suite" => new SuiteRoom(number),
            _ => throw new ArgumentException("Unknown room type")
        };
    }
}

public class Hotel
{
    public List<Room> Rooms;
    public List<Reservation> Reservations;

    public Hotel()
    {
        Rooms = new List<Room>();
        Reservations = new List<Reservation>();
    }
    public void AddRoom(Room room)
    {
        Rooms.Add(room);
    }

    public void BookRoom(int roomNumber, DateTime startDate, int nights)
    {
        Room room = Rooms.Find(r => r.Number == roomNumber);

        if (room == null)
        {
            Console.WriteLine("Room not found.");
            return;
        }

        if (!IsRoomAvaible(room, startDate, nights))
        {
            Console.WriteLine($"Room {roomNumber} not Avaible");
        }
        else
        {
            Reservation reservation = new Reservation(room, startDate, nights);
            Reservations.Add(reservation);
            Console.WriteLine($"Room {roomNumber} successfully booked.");
        }
    }

    private bool IsRoomAvaible(Room room, DateTime startDate, int nights)
    {
        foreach (var reservation in Reservations)
        {
            if (reservation.Room == room && Overlaps(reservation, startDate, nights))
            {
                return false;
            }
        }
        return true;
    }

    private bool Overlaps(Reservation reservation, DateTime startDate, int nights)
    {
        DateTime reservationEndDate = reservation.StartDate.AddDays(reservation.Nights);
        DateTime endDate = startDate.AddDays(nights);
        
        return startDate < reservationEndDate && endDate > reservation.StartDate;
    }

    public void DisplayRooms()
    {
        Console.WriteLine("Rooms available:");
        foreach (var room in Rooms)
        {
            room.GetInfo();
        }
    }

    public void DisplayReservations()
    {
        Console.WriteLine("Current Reservations:");
        foreach (var reservation in Reservations)
        {
            reservation.GetInfo();
        }
    }
}

public class Program
{
    public static void Main()
    {
        Hotel hotel = new Hotel();

        // Добавление комнат с помощью фабрики
        hotel.AddRoom(RoomFactory.CreateRoom("single", 101));
        hotel.AddRoom(RoomFactory.CreateRoom("double", 102));
        hotel.AddRoom(RoomFactory.CreateRoom("suite", 103));

        // Отображение комнат
        hotel.DisplayRooms();

        // Бронирование комнаты
        hotel.BookRoom(101, DateTime.Now, 2);
        hotel.BookRoom(102, DateTime.Now.AddDays(1), 3);
        hotel.BookRoom(103, DateTime.Now.AddDays(5), 5);

        // Повторное бронирование той же комнаты для проверки доступности
        hotel.BookRoom(101, DateTime.Now, 1); // Ожидается отказ из-за пересечения по датам

        // Отображение бронирований
        hotel.DisplayReservations();
    }
}