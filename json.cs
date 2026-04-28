using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Order
{
    public int Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Quantity { get; set; }
    public double Total => Price * Quantity;
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
}

public class Program
{
    private static readonly string OrdersFile = "orders.json";
    private static readonly string BackupFile = "orders_backup.json";
    private static readonly string LogFile = "app.log";
    private static readonly string JsonErrorLog = "errors.json";
    private static List<Order> orders = new();

    public static void Main()
    {
       
        Log("INFO", "Запуск программы");

        LoadOrders();

        while (true)
        {
            ShowMenu();
            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ShowAllOrders();
                        break;
                    case "2":
                        AddOrder();
                        break;
                    case "3":
                        DeleteOrder();
                        break;
                    case "4":
                        FindOrder();
                        break;
                    case "5":
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Log("WARNING", $"Некорректный ввод в меню: {choice}");
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log("ERROR", $"Неожиданная ошибка в меню: {ex.Message}", ex);
            }
        }
    }


    private static void ShowMenu()
    {
        Console.WriteLine("\n--- Меню ---");
        Console.WriteLine("1. Показать все заказы");
        Console.WriteLine("2. Добавить заказ");
        Console.WriteLine("3. Удалить заказ");
        Console.WriteLine("4. Найти заказ по Id");
        Console.WriteLine("5. Выход");
        Console.Write("Выберите действие: ");
    }

    private static void LoadOrders()
    {
        try
        {
            if (File.Exists(OrdersFile))
            {
                string json = File.ReadAllText(OrdersFile);
                orders = JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
                Console.WriteLine($"Загружено {orders.Count} заказов.");
            }
            else
            {
                orders = new List<Order>();
                Console.WriteLine("Файл orders.json не найден. Создан пустой список заказов.");
            }
        }
        catch (JsonException ex)
        {
            Log("ERROR", $"Ошибка десериализации JSON: {ex.Message}", ex);
            orders = new List<Order>();
        }
        catch (IOException ex)
        {
            Log("ERROR", $"Ошибка чтения файла: {ex.Message}", ex);
            orders = new List<Order>();
        }
    }

    private static void SaveOrders()
    {
        try
        {
         
            if (File.Exists(OrdersFile))
            {
                File.Copy(OrdersFile, BackupFile, true);
            }

            string json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(OrdersFile, json);
        }
        catch (IOException ex)
        {
            Log("ERROR", $"Ошибка записи файла: {ex.Message}", ex);
        }
    }

  
    private static void ShowAllOrders()
    {
        if (!orders.Any())
        {
            Console.WriteLine("Список заказов пуст.");
            return;
        }

        var sortedOrders = orders.OrderByDescending(o => o.Price).ToList();
        Console.WriteLine("\n--- Список всех заказов (отсортировано по цене) ---");
        foreach (var order in sortedOrders)
        {
            Console.WriteLine($"Id: {order.Id}, Товар: {order.Product}, " +
                            $"Цена: {order.Price:F2}, Кол-во: {order.Quantity}, " +
                            $"Сумма: {order.Total:F2}");
        }
    }

    private static void AddOrder()
    {
        try
        {
            Console.Write("Введите название товара: ");
            string? product = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(product))
            {
                Log("WARNING", "Попытка добавить заказ с пустым названием товара");
                Console.WriteLine("Название товара не может быть пустым.");
                return;
            }

            Console.Write("Введите цену товара: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                Log("WARNING", "Некорректная цена при добавлении заказа");
                Console.WriteLine("Цена должна быть числом больше 0.");
                return;
            }

            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Log("WARNING", "Некорректное количество при добавлении заказа");
                Console.WriteLine("Количество должно быть целым числом больше 0.");
                return;
            }

            int newId = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
            var order = new Order
            {
                Id = newId,
                Product = product,
                Price = price,
                Quantity = quantity
            };

            orders.Add(order);
            SaveOrders();
            Log("INFO", $"Добавлен заказ Id={order.Id}, Товар: {order.Product}, Сумма: {order.Total:F2}");
            Console.WriteLine($"Заказ успешно добавлен с Id={order.Id}.");
        }
        catch (Exception ex)
        {
            Log("ERROR", $"Ошибка при добавлении заказа: {ex.Message}", ex);
        }
    }


    private static void DeleteOrder()
    {
        Console.Write("Введите Id заказа для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Log("WARNING", "Некорректный Id при попытке удаления заказа");
            Console.WriteLine("Id должен быть целым числом.");
            return;
        }

        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            Log("WARNING", $"Попытка удалить несуществующий заказ с Id={id}");
            Console.WriteLine($"Заказ с Id={id} не найден.");
            return;
        }

        orders.Remove(order);
        SaveOrders();
        Log("INFO", $"Удалён заказ Id={id}");
        Console.WriteLine($"Заказ с Id={id} успешно удалён.");
    }

    private static void FindOrder()
    {
        Console.Write("Введите Id заказа для поиска: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Log("WARNING", "Некорректный Id при поиске заказа");
            Console.WriteLine("Id должен быть целым числом.");
            return;
        }

        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            Console.WriteLine($"Заказ с Id={id} не найден.");
            return;
        }

        Console.WriteLine($"\n--- Найден заказ ---");
        Console.WriteLine($"Id: {order.Id}");
        Console.WriteLine($"Товар: {order.Product}");
        Console.WriteLine($"Цена: {order.Price:F2}");
        Console.WriteLine($"Количество: {order.Quantity}");
        Console.WriteLine($"Общая сумма: {order.Total:F2}");
    }
    private static void Log(string level, string message, Exception? ex = null)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"[{timestamp}] [{level}] {message}";

        try
        {
            File.AppendAllText(LogFile, logMessage + Environment.NewLine);

            if (level == "ERROR" && ex != null)
            {
                var entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Level = level,
                    Message = message,
                    Exception = ex.ToString()
                };
                string json = JsonSerializer.Serialize(entry, new JsonSerializerOptions { WriteIndented = true });
                File.AppendAllText(JsonErrorLog, json + "," + Environment.NewLine);
            }
        }
        catch (IOException)
        {
            Console.WriteLine($"[LOG ERROR] Не удалось записать в лог: {logMessage}");
        }
    }
}
