using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lky
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<book> books = new List<book>
            {
                new book {name="10 niggers", rate=7.3},
                new book {name="The Adventures of Sherlock Holmes ", rate=8.4},
                new book {name="The Adventures of Tom Sawyer", rate=5.3},
                new book {name="Pride and Prejudice", rate=9.3},
                new book {name="Femme de ménage»", rate=1.3}
            };
            var Bookse = books.Where(k => k.rate > 5);
            foreach (var bookl in Bookse)
            {

                Console.WriteLine($"{bookl.name}-rate is {bookl.rate}");


            }
        }
    }
    class book
    {
        public string name;
        public double rate;

    }
}
1)using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace lky
{
    class item
    {
        public string Name;
        public string Type;
        public string Rarity;
        public int Value;
      
    };
    class Program
    {
         static void Main()
        {
            List<item> list = new List<item>()
            {
                new item {Name="Denis", Type="оружие", Rarity="редкий", Value=80},
                new item {Name="Maksim",Type="зелье", Rarity="эпический", Value=80},
                new item {Name="Iliya", Type="броня", Rarity=" обычный", Value=52 },
                new item {Name="Sasha", Type="оружие", Rarity="редкий", Value=66},
                new item {Name="Vanya", Type="броня", Rarity="обычный", Value=44},
                new item {Name="Elena", Type="свиток", Rarity="редкий", Value=75},
                new item {Name="Oleg", Type="оружие", Rarity="обычный", Value=38},
                new item {Name="Maria", Type="зелье", Rarity="эпический", Value=95},
                new item {Name="Igor", Type="броня", Rarity="редкий", Value=70},
                new item {Name="Anna", Type="артефакт", Rarity="легендарный", Value=150},
                new item {Name="Dmitry", Type="оружие", Rarity="эпический", Value=110},
                new item {Name="Sophia", Type="зелье", Rarity="обычный", Value=25},
                new item {Name="Nikita", Type="броня", Rarity="редкий", Value=68},
                new item {Name="Victoria", Type="амулет", Rarity="эпический", Value=85},
            };
            Console.WriteLine("все редкие предметы");
            var rare = list.Where(i => i.Rarity == "редкий");
            foreach(var rares in rare)
            {
                Console.WriteLine($"{rares.Name}, {rares.Type}, {rares.Rarity}, {rares.Value} ");

            }
            Console.WriteLine();
            
            Console.WriteLine("самый дорогой предмет: ");
            var mostex = list.Where(i => i.Value > 50);
            foreach (var exp in mostex)
            {
                Console.WriteLine($"{exp.Name}, {exp.Type}, {exp.Rarity}, {exp.Value} ");

            }
            Console.WriteLine();

            Console.WriteLine("предметы по типу: ");
            var tip = list.GroupBy(i => i.Type);
           foreach (var t in tip)
           {
                Console.WriteLine($"{t.Key}");

           }
           Console.WriteLine() ;
           
            var averageByType = list.GroupBy(i => i.Type)
             .Select(g => new
             {
                Type = g.Key,
                AverageValue = g.Average(i => i.Value)
              }
             );
            foreach (var avg in averageByType)
            {
               Console.WriteLine($"{avg.Type}: {avg.AverageValue:F2}");
            }
         

            Console.WriteLine("5. Предметы с ценой выше средней:");
            double overallAverage = list.Average(i => i.Value);
            Console.WriteLine($"Средняя цена всех предметов: {overallAverage:F2}");
            var aboveAverage = list.Where(i => i.Value > overallAverage);
            foreach (var item in aboveAverage)
            {
                Console.WriteLine($"{item.Name} ({item.Type}, {item.Rarity}) - {item.Value}");
            }




          }




    }
   
}
