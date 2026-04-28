using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

public class Order
{
    public int Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public double Price { get; set; }
}


public interface IOrderRepository
{
    void Save(Order order);
    List<Order> GetAll();
}

public interface ILogger
{
    void Log(string message);
}

public interface IOrderService
{
    void CreateOrder(Order order);
    List<Order> GetOrders();
    bool DeleteOrder(int id);
}

public class FileOrderRepository : IOrderRepository
{
    private readonly string _filePath = "orders.json";
    private List<Order> _orders;

    public FileOrderRepository()
    {
        _orders = LoadOrders();
    }

    private List<Order> LoadOrders()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
            }
            return new List<Order>();
        }
        catch (Exception ex)
        {
            throw new IOException($"Ошибка при загрузке данных из {_filePath}: {ex.Message}", ex);
        }
    }

    private void SaveOrders()
    {
        try
        {
            var json = JsonSerializer.Serialize(_orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            throw new IOException($"Ошибка при сохранении данных в {_filePath}: {ex.Message}", ex);
        }
    }

    public void Save(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
        if (existing != null)
            _orders.Remove(existing);

        _orders.Add(order);
        SaveOrders();
    }

    public List<Order> GetAll() => new(_orders);
}

public class MemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    private int _nextId = 1;

    public void Save(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        if (order.Id == 0)
            order.Id = _nextId++;

        var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
        if (existing != null)
            _orders.Remove(existing);

        _orders.Add(order);
    }

    public List<Order> GetAll() => new(_orders);
}
public class FileLogger : ILogger
{
    private readonly string _logPath = "app.log";

    public void Log(string message)
    {
        try
        {
            var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(_logPath, entry + Environment.NewLine);
        }
        catch
        {
          
        }
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;

    public OrderService(IOrderRepository repository, ILogger logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void CreateOrder(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (string.IsNullOrWhiteSpace(order.Product))
        {
            var ex = new ArgumentException("Название товара не может быть пустым.");
            _logger.Log($"ОШИБКА: {ex.Message}");
            throw ex;
        }

        if (order.Price <= 0)
        {
            var ex = new ArgumentException("Цена должна быть больше 0.");
            _logger.Log($"ОШИБКА: {ex.Message}");
            throw ex;
        }

        if (order.Id == 0)
        {
            var allOrders = _repository.GetAll();
            order.Id = allOrders.Any() ? allOrders.Max(o => o.Id) + 1 : 1;
        }

        _repository.Save(order);
        _logger.Log($"Создан заказ Id={order.Id}, Товар: '{order.Product}', Цена: {order.Price:F2}");
    }

    public List<Order> GetOrders() => _repository.GetAll();

    public bool DeleteOrder(int id)
    {
        var allOrders = _repository.GetAll();
        var order = allOrders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            _logger.Log($"ПРЕДУПРЕЖДЕНИЕ: Попытка удалить несуществующий заказ с Id={id}");
            return false;
        }

        _orders = allOrders.Where(o => o.Id != id).ToList();
        SaveOrders();
        _logger.Log($"Удалён заказ Id={id}");
        return true;
    }
}

public class Program
{
    public static void Main()
    {

        var services = new ServiceCollection();
        services.AddSingleton<ILogger, FileLogger>();
        services.AddTransient<IOrderRepository, FileOrderRepository>();

        services.AddTransient<IOrderService, OrderService>();

        var provider = services.BuildServiceProvider();

        var orderService = provider.GetService<IOrderService>()
            ?? throw new InvalidOperationException("Не удалось получить IOrderService.");

        while (true)
        {
            Console.WriteLine("\n--- Система обработки заказов ---");
            Console.WriteLine("1. Добавить заказ");
            Console.WriteLine("2. Показать все заказы");
            Console.WriteLine("3. Удалить заказ по Id");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите действие: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddOrder(orderService);
                        break;
                    case "2":
                        ShowOrders(orderService);
                        break;
                    case "3":
                        DeleteOrder(orderService);
                        break;
                    case "4":
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }
    }

    private static void AddOrder(IOrderService service)
    {
        try
        {
            Console.Write("Название товара: ");
            string? product = Console.ReadLine();

            Console.Write("Цена: ");
            if (!double.TryParse(Console.ReadLine(), out double price))
            {
                Console.WriteLine("Ошибка: введите корректное число для цены.");
                return;
            }

            var order = new Order
            {
                Product = product ?? string.Empty,
                Price = price
            };

            service.CreateOrder(order);
            Console.WriteLine($"Заказ успешно создан с Id={order.Id}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании заказа: {ex.Message}");
        }
    }

    private static void ShowOrders(IOrderService service)
    {
        try
        {
            var orders = service.GetOrders();
            if (!orders.Any())
            {
                Console.WriteLine("Список заказов пуст.");
                return;
            }

            Console.WriteLine("\n--- Список заказов ---");
            foreach (var order in orders)
            {
                Console.WriteLine($"Id: {order.Id}, Товар: {order.Product}, Цена: {order.Price:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении заказов: {ex.Message}");
        }
    }

    private static void DeleteOrder(IOrderService service)
    {
        try
        {
            Console.Write("Введите Id заказа для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка: Id должен быть целым числом.");
                return;
            }

            bool success = service.DeleteOrder(id);
            if (success)
                Console.WriteLine($"Заказ с Id={id} успешно удалён.");
            else
                Console.WriteLine($"Заказ с Id={id} не найден.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении заказа: {ex.Message}");
        }
    }
}
