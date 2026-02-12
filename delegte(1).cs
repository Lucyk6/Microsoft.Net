1)
using System;
public class Program
{
    public delegate int CalculteDeligate(int x, int y);
    public static int Add (int x, int y)
    {
        return x + y;
    }
    public static int Subtract (int x, int y)
    {
        return x - y;
    }
    public static void Main()
    {
        CalculteDeligate calculte = Add;
        Console.WriteLine(calculte(5,3));

        calculte = Subtract;
        Console.WriteLine (calculte(5,3));
    }

}
2) 
    using System;
public class Program
{
    public delegate int CalculteDeligate(int x, int y);
    public static int Add(int x, int y)
    {
        return x + y;
    }
    public static int Subtract(int x, int y)
    {
        return x - y;
    }
    //public static void Main()
    //{
    //    CalculteDeligate calculte = Add;
    //    Console.WriteLine(calculte(5, 3));

    //    calculte = Subtract;
    //    Console.WriteLine(calculte(5, 3));
    //}

    public delegate void PrintstringDelegte(string massage);
    public static void Main()
    {
        PrintstringDelegte printDelegte = delegate (string massage)
        {
            Console.WriteLine("trash");
           
            Console.WriteLine(Add(7, 6));
            Console.WriteLine(Subtract(7, 6));

        };
        printDelegte("hell nooo");
        printDelegte = delegate (string massage)
        {
            int a = 1;
            int b = 2;
            string che = "rubbish";
            Console.WriteLine($"{a + b} {che}");


        };
        printDelegte("hellllllnoooo");


    }


}
