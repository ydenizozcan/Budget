using System;

namespace KazikazanButce
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lütfen bir hedef kitle sayısı giriniz:");
            var _target = Convert.ToInt64(Console.ReadLine());

            Console.WriteLine("Lütfen bir bütçe giriniz:");
            var _budget = Convert.ToInt64(Console.ReadLine());

            new BudgetDistribution(_target,_budget).Start();
           
            
        }
    }
}
