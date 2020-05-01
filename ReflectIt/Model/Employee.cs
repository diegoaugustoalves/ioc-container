﻿using System;

namespace ReflectIt.Model
{
    public class Employee : Person
    {
        public string Name { get; set; }

        public void Speak<T>()
        {
            Console.WriteLine(typeof(T).Name);
        }
    }
}