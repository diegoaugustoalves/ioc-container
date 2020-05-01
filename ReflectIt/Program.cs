using System;
using System.Collections.Generic;
using ReflectIt.Model;

namespace ReflectIt
{
    class Program
    {
        static void Main(string[] args)
        {
            var employeeList = CreateCollection(typeof(List<>),typeof(Employee));
            Console.Write(employeeList.GetType().Name);
            var genericArguments = employeeList.GetType().GenericTypeArguments;
            foreach (var argument in genericArguments)
            {
                Console.Write($"[{argument.Name}]");
            }
 
            Console.WriteLine();
            
            var employee = new Employee();
            var employeeType = typeof(Employee);
            var methodInfo = employeeType.GetMethod("Speak");
            methodInfo = methodInfo.MakeGenericMethod(typeof(DateTime));
            methodInfo.Invoke(employee, null);
        }

        private static object CreateCollection(Type collectionType, Type itemType)
        {
            var closedType = collectionType.MakeGenericType(itemType);
            return Activator.CreateInstance(closedType);
        }
    }
}