
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace наследование
{
    class Person
    {
        public string Name {  get; set; }
        public int Age { get; set; }

        public Person (string name, int age)
        {
            Name = name;
            Age = age;
        }

    } 
    class Student
    {
        public string Univercity  {  get; set; }


    }







    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
