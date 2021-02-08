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
        public List<Column> columnsList;
        public List<CallButton> callButtonsList;
        
        public Battery(int id, string status,int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn)
        {
            this.ID = id;
            this.amountOfColumns = amountOfColumns;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfBasements = amountOfBasements;
            this.columnsList = new List<Column>();
            this.callButtonsList = new List<CallButton>();
            

            for (int i = 0; i < this.amountOfColumns; i++){
                Column column = new Column(i, "online", 2, 2, 2, false);
                columnsList.Add(column);
            }

            // for (int i = 0; i < this.amountOfFloors; i++){
            //     callButtonsList[i] = new CallButton();
            // }
            
        }
    }

    // Column class
    public class Column{
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfElevators;
        public List<int> servedFloors;
        public List<Elevator> elevatorsList;
        public List<FloorRequestButton> floorRequestButtonsList;

        public Column(int id, string status, int amountOfFloors, int amountOfElevators, int servedFloors, bool isBasement){
            
            this.ID = id;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfElevators = amountOfElevators;
            this.elevatorsList = new List<Elevator>(){};
            this.floorRequestButtonsList = new List<FloorRequestButton>(){};
            this.servedFloors = new List<int>(){};
        }
    }

    // Elevator class
    public class Elevator{
        public int ID;
        public string status;
        public int amountOfFloors;
        public string direction;
        public int currentfloor;
        public Door door;
        public List<int> floorRequestList; // will have to come back to this

        public Elevator(int id, string status, int amountOfFloors, int currentfloor){
            
            this.ID = id;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.direction = null;
            this.currentfloor = currentfloor;
            // this.door = new Door();
            this.floorRequestList = new List<int>(){}; // will have to come back to this
        }
    }

    // Call Button class
    public class CallButton{
        public int ID;
        public string status;
        public int floor;
        public string direction;

        public CallButton(int id, string status, int floor, string direction){
            this.ID = id;
            this.status = status;
            this.floor = floor;
            this.direction = direction;
        }
    }

    // Floor request button class
    public class FloorRequestButton{
        public int ID;
        public string status;
        public int floor;

        public FloorRequestButton(int id, string status, int floor){
            this.ID = id;
            this.status = status;
            this.floor = floor;
        }
    }


    // Door class
    public class Door{
        public int ID;
        public string status;

        public Door(int id, string status){
            this.ID = id;
            this.status = status;
        }
    }


    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------// TESTING //-------------------");

            Battery testBat = new Battery(1, "online", 4, 66, 6, 5);
            Console.WriteLine(testBat.status);
            Console.WriteLine("....");
            Console.WriteLine(testBat.columnsList[3].ID);
        }

    }
}


