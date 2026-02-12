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
