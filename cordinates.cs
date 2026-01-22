using System;
internal class Program
{
//    static void Main()
//    {
//        var f1 = new Fraction(1, 2);
//        var f2 = new Fraction(3, 4);
//        Console.WriteLine($"f1+f2= {f1+ f2}");
//        Console.WriteLine($"f1-f2= {f1 - f2}");
//        Console.WriteLine($"f1*f2= {f1 * f2}");
//        Console.WriteLine($"f1/f2= {f1 / f2}");
//    }
//}
//public class Fraction
//{
//    public int Numerator { get; }

//    public int Denominator { get; }

//    public Fraction(int numerator, int denominator)
//    {
//        Numerator = numerator;
//        Denominator = denominator;

//    }
//    public static Fraction operator +(Fraction a, Fraction b)
//    {
//        int newNumerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
//        int newDenominator = a.Denominator * b.Denominator;
//        return new Fraction(newNumerator, newDenominator);

//    }
//    public static Fraction operator -(Fraction a, Fraction b)
//    {
//        int newNumerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
//        int newDenominator = a.Denominator * b.Denominator;
//        return new Fraction(newNumerator, newDenominator);

//    }
//    public static Fraction operator *(Fraction a, Fraction b)
//    {
//        int newNumerator = a.Numerator * b.Denominator * b.Numerator * a.Denominator;
//        int newDenominator = a.Denominator * b.Denominator;
//        return new Fraction(newNumerator, newDenominator);

//    }
//    public static Fraction operator /(Fraction a, Fraction b)
//    {
//        int newNumerator = a.Numerator * b.Denominator / b.Numerator * a.Denominator;
//        int newDenominator = a.Denominator * b.Denominator;
//        return new Fraction(newNumerator, newDenominator);

//    }
//    public override string ToString() => $"{Numerator}/{Denominator}";
2) static void Main(string[] args)    
   {
        var shop = new Shop();
        shop[0] = "apple";
        shop[1] = "banana";
        shop[2] = "orange";

        for (int i = 0; i < 3; i++) {

            Console.WriteLine($"Products {i}: {shop[i]}");
        }

    }
    public class Shop
    {
        private string[] _products = new string[100];
        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _products.Length)
                    throw new IndexOutOfRangeException();
                return _products[index];

                
            }
            set
            {
                if (index < 0 || index >= _products.Length)
                    throw new IndexOutOfRangeException();
                _products[index] = value;
                if (index % 2 == 0)
                {
                    Console.WriteLine("delitsa");
                }
                else
                {
                    Console.WriteLine("net nelzia");
                }
            }

        }

        public int ProductCount=> _products.Length;

     }


}
