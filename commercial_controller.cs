using System;
using System.Collections.Generic;

namespace C_
{
    // Battery class
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

    // Column class
    public class Column{

    }

    // Elevator class
    public class Elevator{

    }

    // Call Button class
    public class CallButton{

    }

    // Floor request button class
    public class FloorRequestButton{

    }


    // Door class
    public class Door{
        
    }


    // Main Program
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


