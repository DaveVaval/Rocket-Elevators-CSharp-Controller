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
        public int amountOfColumns;
        public int amountOfBasements;
        public Column[] columnList;
        public CallButton[] callButtonsList;

        public Battery(int id, string status,int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn)
        {
            this.ID = id;
            this.amountOfColumns = amountOfColumns;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfBasements = amountOfBasements;
            this.columnList = new Column[]{};
            this.callButtonsList = new CallButton[]{};

            for (int i = 0; i < this.amountOfColumns; i++){
                columnList[i] = new Column();
            }

            for (int i = 0; i < this.amountOfFloors; i++){
                callButtonsList[i] = new CallButton();
            }
            
        }
    }

    // Column class
    public class Column{
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfElevators;
        public Elevator[] elevatorsList;
        public FloorRequestButton[] floorRequestButtonsList;

        public Column(int id, string status, int amountOfFloors, int amountOfElevators, int servedFloors, bool isBasement){
            
            this.ID = id;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfElevators = amountOfElevators;
            this.elevatorsList = new Elevator[]{};
            this.floorRequestButtonsList = new FloorRequestButton[]{};
        }
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
            Console.WriteLine("----------------// TESTING //-------------------");

            Battery testBat = new Battery(1, "online", 4, 66, 6, 5);
            Console.WriteLine(testBat.status);
        
        }

    }
}


