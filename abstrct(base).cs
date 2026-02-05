using System;
using System.Drawing;
using System.Xml.Linq;
class Student
{
    public string Name { get; set; }
    public int Age { get; set; }

    public int Clas { get; set; } 

    public double Attendance { get; set; }
    public Student (string name, int age, int clas, double attendance)
    {
        Name = name;
        Age = age;
        Clas = clas;
        Attendance = attendance;

    }

}
class Excellent : Student
{
    public int Grades {  get; set; }
    public string Diploma { get; set; }

    public Excellent (string name, int age, int clas, double attendance, int grades, string diploma)
        : base( name,  age, clas, attendance)
    { 
      Grades = grades;

      Diploma = diploma;
    
    }


}

class Sportsmen : Student
{
    public string Act { get; set; }
    public string Medals { get; set; }

    public Sportsmen (string name, int age, int clas, double attendance, string act, string medals ) 
        : base ( name,  age, clas, attendance)
    {
       Act = act;

       Medals = medals;

    }

}

class Program
{
    static void Main()
    {

        Student[] students = new Student[4];

        students[0] = new Excellent("Анна", 17, 11, 98.5, 5, "Золотая медаль");
        students[1] = new Sportsmen("Иван", 16, 10, 95.0, "Футбол", "Серебро на региональном чемпионате");
        students[2] = new Excellent("Мария", 17, 11, 99.0, 5, "Похвальный лист");
        students[3] = new Sportsmen("Алексей", 16, 10, 92.0, "Плавание", "Бронза на городских соревнованиях");

        foreach (Student student in students)
        {
            Console.WriteLine($"Имя: {student.Name}");
            Console.WriteLine($"Возраст: {student.Age}");
            Console.WriteLine($"Класс: {student.Clas}");
            Console.WriteLine($"Посещаемость: {student.Attendance}%");

            if (student is Excellent excellentStudent)
            {
                Console.WriteLine($"Оценки: {excellentStudent.Grades}");
                Console.WriteLine($"Диплом: {excellentStudent.Diploma}");
            }
            else if (student is Sportsmen sportsmanStudent)
            {
                Console.WriteLine($"Вид спорта: {sportsmanStudent.Act}");
                Console.WriteLine($"Медали: {sportsmanStudent.Medals}");
            }

  
        }
    }
}




