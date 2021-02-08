using System;
using System.Collections.Generic;

namespace C_
{
    public class Battery
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public string[] columnList;

        // public void createColumns()
        // {
        //     for (int i = 0; i < this.amountOfFloors; i++){
        //         columnList[i] = new Column();
        //     }
        // }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Battery test = new Battery();
            test.ID = 1;
            Console.WriteLine(test.ID);
        }

    }
}


