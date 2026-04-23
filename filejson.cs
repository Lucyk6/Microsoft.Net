/*Запись в JSON
Person person = new Person { Name = "Иван", Age = 25 };
string json = JsonSerializer.Serialize(person);
File.WriteAllText("person.json", json);
Чтение из JSON
string json = File.ReadAllText("person.json");
Person person = JsonSerializer.Deserialize<Person>(json);
Console.WriteLine(person.Name);*/

/*Создание XML
XDocument doc = new XDocument(
    new XElement("Person",
        new XElement("Name", "Иван"),
        new XElement("Age", 25)
    )
);

doc.Save("person.xml");
Чтение XML
XDocument doc = XDocument.Load("person.xml");

var person = doc.Element("Person");
string name = person.Element("Name").Value;
int age = int.Parse(person.Element("Age").Value);*/


/*Файл
File.Create("newfile.txt").Close();
File.Delete("newfile.txt");
Папка
Directory.CreateDirectory("data");
Directory.Delete("data", true);
*/


using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

public enum LogLevel { INFO, WARNING, ERROR }

public class Logger
{
    private readonly string _logFile;
    private const long MAX_SIZE = 1024 * 1024; 

    public Logger(string logFile = "app.log") => _logFile = logFile;

    public void Log(LogLevel level, string msg)
    {
        if (File.Exists(_logFile) && new FileInfo(_logFile).Length >= MAX_SIZE)
            File.Move(_logFile, $"app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

        File.AppendAllText(_logFile,
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {msg}{Environment.NewLine}");
    }

    public void Info(string msg) => Log(LogLevel.INFO, msg);
    public void Warning(string msg) => Log(LogLevel.WARNING, msg);
    public void Error(string msg) => Log(LogLevel.ERROR, msg);
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class UserManager
{
    private List<User> _users = new List<User>();
    private readonly string _filePath;

    public UserManager(string filePath = "users.json")
    {
        _filePath = filePath;
        LoadUsers();
    }

    private void LoadUsers()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                _users = new List<User>();
                SaveUsers();
                return;
            }

            string json = File.ReadAllText(_filePath);
            _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка парсинга JSON: {ex.Message}");
            _users = new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки файла: {ex.Message}");
            _users = new List<User>();
        }
    }


    public bool AddUser(User user)
    {
        if (user == null) return false;


        if (_users.Any(u => u.Id == user.Id))
        {
            Console.WriteLine($"Пользователь с Id {user.Id} уже существует");
            return false;
        }

        _users.Add(user);
        SaveUsers();
        return true;
    }

    public bool RemoveUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            Console.WriteLine($"Пользователь с Id {id} не найден");
            return false;
        }

        _users.Remove(user);
        SaveUsers();
        return true;
    }

    public bool UpdateUser(User updatedUser)
    {
        if (updatedUser == null) return false;

        var existingUser = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (existingUser == null)
        {
            Console.WriteLine($"Пользователь с Id {updatedUser.Id} не найден");
            return false;
        }

        existingUser.Name = updatedUser.Name;
        existingUser.Age = updatedUser.Age;
        SaveUsers();
        return true;
    }


    private void SaveUsers()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true 
            };

            string json = JsonSerializer.Serialize(_users, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения файла: {ex.Message}");
        }
    }

    public List<User> GetAllUsers() => _users;
}
public class LogAnalyzer
{
    public static void AnalyzeLogs(string inputFilePath = "logs.txt", string outputFilePath = "report.txt")
    {
        var lines = File.ReadAllLines(inputFilePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        var logCounts = new Dictionary<string, int>
        {
            ["INFO"] = 0,
            ["WARNING"] = 0,
            ["ERROR"] = 0
        };

        foreach (var line in lines)
        {
            var level = line.Split(':')[0].Trim().ToUpper();
            if (logCounts.ContainsKey(level))
            {
                logCounts[level]++;
            }
        }

        var mostFrequent = logCounts
            .OrderByDescending(kv => kv.Value)
            .First()
            .Key;

        var reportLines = new List<string>();
        reportLines.Add($"INFO: {logCounts["INFO"]}");
        reportLines.Add($"WARNING: {logCounts["WARNING"]}");
        reportLines.Add($"ERROR: {logCounts["ERROR"]}");
        reportLines.Add($"Самый частый: {mostFrequent}");

        File.WriteAllLines(outputFilePath, reportLines);

        Console.WriteLine("Отчёт создан:");
        foreach (var line in reportLines)
        {
            Console.WriteLine(line);
        }
    }
}
public class BackupUtility
{
    private static readonly List<string> _log = new List<string>();

    public static void CreateBackup(string sourceDir = "data")
    {
        if (!Directory.Exists(sourceDir))
        {
            LogError($"Исходная директория '{sourceDir}' не существует");
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string backupDir = $"backup_{timestamp}";

        try
        {
            CopyDirectoryRecursive(sourceDir, backupDir);
            LogInfo($"Резервная копия создана: {backupDir}");
        }
        catch (Exception ex)
        {
            LogError($"Ошибка при создании резервной копии: {ex.Message}");
        }


        SaveLog("backup_log.txt");
    }

    private static void CopyDirectoryRecursive(string source, string destination)
    {
       
        Directory.CreateDirectory(destination);
        LogInfo($"Создана директория: {destination}");

        foreach (string file in Directory.GetFiles(source))
        {
            try
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destination, fileName);
                File.Copy(file, destFile, true);
                LogInfo($"Скопирован файл: {file} -> {destFile}");
            }
            catch (Exception ex)
            {
                LogError($"Ошибка копирования файла {file}: {ex.Message}");
            }
        }


        foreach (string subDir in Directory.GetDirectories(source))
        {
            string dirName = Path.GetFileName(subDir);
            string destSubDir = Path.Combine(destination, dirName);
            CopyDirectoryRecursive(subDir, destSubDir);
        }
    }

    private static void LogInfo(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}";
        _log.Add(logEntry);
        Console.WriteLine(logEntry);
    }

    private static void LogError(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}";
        _log.Add(logEntry);
        Console.WriteLine(logEntry);
    }

    private static void SaveLog(string logFilePath)
    {
        try
        {
            File.WriteAllLines(logFilePath, _log);
            Console.WriteLine($"Лог сохранён в: {logFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения лога: {ex.Message}");
        }
    }
}
public class JsonToXmlConverter
{
    public static void Convert(string inputJsonPath, string outputXmlPath, string rootElementName = "Product")
    {
        try
        {
            string jsonContent = File.ReadAllText(inputJsonPath);
            JsonDocument jsonDoc = JsonDocument.Parse(jsonContent);

            XElement xmlRoot = ConvertJsonToXmlElement(jsonDoc.RootElement, rootElementName);

            XDocument xmlDoc = new XDocument(xmlRoot);
            xmlDoc.Save(outputXmlPath);

            Console.WriteLine($"Конвертация успешна: {inputJsonPath} → {outputXmlPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка конвертации: {ex.Message}");
        }
    }

    private static XElement ConvertJsonToXmlElement(JsonElement jsonElement, string elementName)
    {
        XElement xmlElement = new XElement(elementName);

        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in jsonElement.EnumerateObject())
                {
                    xmlElement.Add(ConvertJsonToXmlElement(property.Value, property.Name));
                }
                break;

            case JsonValueKind.Array:
                int index = 0;
                foreach (var item in jsonElement.EnumerateArray())
                {
                    string itemName = $"{elementName}Item{index++}";
                    xmlElement.Add(ConvertJsonToXmlElement(item, itemName));
                }
                break;

            default:
                xmlElement.Value = jsonElement.ToString();
                break;
        }

        return xmlElement;
    }
}


class Program
{
    static void Main()
    {
        var logger = new Logger();
        logger.Info("Запуск приложения");
        logger.Warning("Нехватка памяти");
        logger.Error("Ошибка подключения к БД");
        var userManager = new UserManager();
        userManager.AddUser(new User { Id = 1, Name = "Иван", Age = 25 });
        userManager.AddUser(new User { Id = 2, Name = "Анна", Age = 30 });
        userManager.UpdateUser(new User { Id = 1, Name = "Иван Петров", Age = 26 });

        userManager.RemoveUser(2);

        foreach (var user in userManager.GetAllUsers())
        {
            Console.WriteLine($"{user.Id}: {user.Name}, {user.Age} лет");
        }
        try
        {
            LogAnalyzer.AnalyzeLogs();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл логов не найден!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        BackupUtility.CreateBackup();

        string jsonContent = @"{
            ""Name"": ""Laptop"",
            ""Price"": 1000
        }";
        File.WriteAllText("input.json", jsonContent);
        JsonToXmlConverter.Convert("input.json", "output.xml");

        string complexJson = @"{
            ""Product"": {
                ""Name"": ""Laptop"",
                ""Price"": 1000,
                ""Specs"": {
                    ""CPU"": ""Intel i7"",
                    ""RAM"": ""16GB""
                },
                ""Tags"": [""electronics"", ""computer""]
            }
        }";
        File.WriteAllText("complex_input.json", complexJson);
        JsonToXmlConverter.Convert("complex_input.json", "complex_output.xml", "Product");

        
            using var watcher = new FileSystemWatcher("target_folder")
            {
                Filter = "*.txt",
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            watcher.Created += (s, e) => LogEvent($"Создан файл: {e.Name}");
            watcher.Deleted += (s, e) => LogEvent($"Удалён файл: {e.Name}");
            watcher.Changed += (s, e) => LogEvent($"Изменён файл: {e.Name}");

            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Мониторинг запущен. Нажмите Enter для выхода...");
            Console.ReadLine();
        static void LogEvent(string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Console.WriteLine(message);
            File.AppendAllText("watcher_log.txt", logEntry + Environment.NewLine);
        }


        string inputFile = "large_log.txt"; 
        string outputFile = "error_lines.txt"; 

        try
        {
            using (var reader = new StreamReader(inputFile))
            using (var writer = new StreamWriter(outputFile))
            {
                string line;
                int lineCount = 0;
                int errorCount = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineCount++;

                    if (line.Contains("ERROR"))
                    {
                        writer.WriteLine(line);
                        errorCount++;
                    }

            
                    if (lineCount % 10000 == 0)
                    {
                        Console.Write($"\rОбработано строк: {lineCount:N0}");
                    }
                }

                Console.WriteLine($"\nГотово! Обработано строк: {lineCount:N0}, найдено ERROR: {errorCount:N0}");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Файл {inputFile} не найден!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }


    }
}    







