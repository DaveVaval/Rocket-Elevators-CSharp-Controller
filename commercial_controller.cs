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
        public int columnID;
        public List<Column> columnsList;
        public List<CallButton> callButtonsList;
        
        public Battery(int id, string status,int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn)
        {
            // Battery testBat = new Battery(1, "online", 4, 60, 6, 5);
            this.ID = id;
            this.amountOfColumns = amountOfColumns;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfBasements = amountOfBasements;
            this.columnsList = new List<Column>();
            this.callButtonsList = new List<CallButton>();
            this.columnID = 1;

            if (this.amountOfBasements > 0){
                createBasementColumn(this.amountOfBasements, amountOfElevatorPerColumn);
                amountOfColumns--;
            }

            createColumns(amountOfColumns, this.amountOfFloors, this.amountOfBasements, amountOfElevatorPerColumn);
            

            foreach(Column col in columnsList)
            {
                System.Console.WriteLine("column: " + col.ID);
                foreach(int floor in col.servedFloors)
                    System.Console.WriteLine("    floor: " + floor);
            }            
             
        }

        public void createBasementColumn(int amountOfBasements, int amountOfElevatorPerColumn){
            List<int> servedFloors = new List<int>();
            int floor = -1;
    
            for (int i = 0; i < amountOfBasements; i++){
                servedFloors.Add(floor);
                floor--;
            }
            columnsList.Add(new Column(columnID, "online", amountOfBasements, amountOfElevatorPerColumn, servedFloors, true));
            columnID++;
        }

        public void createColumns(int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn){
            int amountOfFloorsPerColumn = (int)Math.Ceiling((double)amountOfFloors / amountOfColumns);
            int floor = 1;

            for (int i = 1; i <= amountOfColumns; i++){ // i = i fixed the issue of the amount of columns
                List<int> servedFloors = new List<int>(); 
                for (int n = 0; n < amountOfFloorsPerColumn; n++){
                    if(floor <= amountOfFloors){
                        servedFloors.Add(floor);
                        floor++;
                    }
                }
                columnsList.Add(new Column(columnID, "online", amountOfFloors, amountOfElevatorPerColumn, servedFloors, false));
                columnID++;
            }
        }

        // public void createCallButtons
    }



    // Column class
    public class Column{
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfElevators;
        public bool isBasement;
        public List<int> servedFloors;
        public List<Elevator> elevatorsList;
        public List<FloorRequestButton> floorRequestButtonsList;

        public Column(int id, string status, int amountOfFloors, int amountOfElevators, List<int> servedFloors, bool isBasement){
            
            this.ID = id;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfElevators = amountOfElevators;
            this.isBasement = isBasement;
            this.elevatorsList = new List<Elevator>(){};
            this.floorRequestButtonsList = new List<FloorRequestButton>(){};
            this.servedFloors = servedFloors;
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

            Battery testBat = new Battery(1, "online", 4, 60, 6, 5);
            // Console.WriteLine("Battery: " + testBat.status);
            // Console.WriteLine("------");
            // Console.WriteLine(testBat.columnsList[1]);
            // Console.WriteLine("Column ID: " + testBat.columnsList[1].ID);
            // Console.WriteLine("status: " + testBat.columnsList[1].status);
            // Console.WriteLine("floors: " + testBat.columnsList[1].amountOfFloors);
            // Console.WriteLine("elevators: " + testBat.columnsList[1].amountOfElevators);
            // Console.WriteLine("basement: " + testBat.columnsList[1].isBasement);
            // Console.WriteLine("------");
            // int amountOfFloorsPerColumn = (int)Math.Ceiling((double)60 / 4);
            // Console.WriteLine(amountOfFloorsPerColumn);
            Console.WriteLine("Amount of columns: " + testBat.amountOfColumns);
        }

    }
}


