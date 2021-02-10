using System;
using System.Collections.Generic;
using System.Collections;



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
        public int floorRequestButtonID;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorRequestButtonsList;
        
        public Battery(int id, string status,int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn)
        {
            // Battery testBat = new Battery(1, "online", 4, 60, 6, 5);
            this.ID = id;
            this.amountOfColumns = amountOfColumns;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfBasements = amountOfBasements;
            this.columnsList = new List<Column>();
            this.floorRequestButtonsList = new List<FloorRequestButton>();
            this.columnID = 1;
            this.floorRequestButtonID = 1;

            if (this.amountOfBasements > 0){
                createBasementFloorRequestButtons(amountOfBasements);
                createBasementColumn(this.amountOfBasements, amountOfElevatorPerColumn);
                amountOfColumns--;
            }
            createFloorRequestButtons(amountOfFloors);
            createColumns(amountOfColumns, this.amountOfFloors, this.amountOfBasements, amountOfElevatorPerColumn);
            
            // for debug only
            // foreach(Column column in columnsList)
            // {
            //     System.Console.WriteLine("column: " + column.ID);
            //     foreach(Elevator elevator in column.elevatorsList)
            //         System.Console.WriteLine("    elevator: " + elevator.ID);
            // }            
             
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

            for (int i = 1; i <= amountOfColumns; i++){ // i = 1 fixed the issue of the amount of columns
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

        public void createFloorRequestButtons(int amountOfFloors){
            int buttonFloor = 1;
            for (int i = 1; i <= amountOfFloors; i++){
                floorRequestButtonsList.Add(new FloorRequestButton(floorRequestButtonID, "off", buttonFloor));
                floorRequestButtonID++;
                buttonFloor++;
            }
        }

        public void createBasementFloorRequestButtons(int amountOfBasements){
            int buttonFloor = -1;
            for (int i = 1; i <= amountOfBasements; i++){
                floorRequestButtonsList.Add(new FloorRequestButton(floorRequestButtonID, "off", buttonFloor));
                buttonFloor--;
                floorRequestButtonID++;
            }
        }

        public Column findBestColumn(int requestedFloor){ // what?
            foreach (Column column in columnsList){
                if(column.servedFloors.Contains(requestedFloor)){
                    return column;
                }
            }
        }

        public void assignElevator(int requestedFloor, string direction){
            Column column = findBestColumn(requestedFloor); // return?
            Elevator elevator = column.findElevator(1, direction); // return?
            elevator.floorRequestList.Add(requestedFloor);
            elevator.sortFloorList();
            elevator.move();
            elevator.openDoors();
        }
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
        public List<CallButton> callButtonsList;

        public Column(int id, string status, int amountOfFloors, int amountOfElevators, List<int> servedFloors, bool isBasement){
            
            this.ID = id;
            this.status = status;
            this.amountOfFloors = amountOfFloors;
            this.amountOfElevators = amountOfElevators;
            this.isBasement = isBasement;
            this.elevatorsList = new List<Elevator>(){};
            this.callButtonsList = new List<CallButton>(){};
            this.servedFloors = servedFloors;
            createElevators(amountOfFloors, amountOfElevators);
            createCallButtons(amountOfFloors, isBasement);

            // debug
            // foreach(Elevator elevator in elevatorsList)
            // {
            //     System.Console.WriteLine("elevator: " + elevator.ID);
            //     // foreach(int floor in column.servedFloors)
            //     //     System.Console.WriteLine("    floor: " + floor);
            // } 
        }


        public void createElevators(int amountOfFloors, int amountOfElevators){
            int elevatorID = 1;
            for(int i = 0; i < amountOfElevators; i++){
                elevatorsList.Add(new Elevator(elevatorID, "idle", amountOfFloors, 1));
                elevatorID++;
            }
        }

        public void createCallButtons(int amountOfFloors, bool isBasement){
            int callButtonID = 1;
            if (isBasement == true){
                int buttonFloor = -1;
                for(int i = 0; i < amountOfFloors; i++){
                    callButtonsList.Add(new CallButton(callButtonID, "off", buttonFloor, "up"));
                    buttonFloor--;
                    callButtonID++;
                }
            } 
            else{
                int buttonFloor = 1;
                for(int i = 0; i < amountOfFloors; i++){
                    callButtonsList.Add(new CallButton(callButtonID, "off", buttonFloor, "down"));
                    buttonFloor++;
                    callButtonID++;
                }
            }
        }

        public Elevator requestElevator(int userPosition, string direction){
            Elevator elevator = findElevator(userPosition, direction); // return?
            elevator.floorRequestList.Add(1);
            elevator.sortFloorList();
            elevator.move();
            elevator.openDoors();
            return elevator;
        }

        public Elevator findElevator(int requestedFloor, string requestedDirection){ // return?
            Hashtable bestElevatorInfo = new Hashtable(){
                {"bestElevator", null},
                {"bestScore", 6},
                {"referenceGap", int.MaxValue}
            };
            if(requestedFloor == 1){
                foreach(Elevator elevator in elevatorsList){
                    if(1 == elevator.currentfloor && elevator.status == "stopped"){
                        bestElevatorInfo = checkElevator(1, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(1 == elevator.currentfloor && elevator.status == "idle"){
                        bestElevatorInfo = checkElevator(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(1 > elevator.currentfloor && elevator.direction == "up"){
                        bestElevatorInfo = checkElevator(3, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(1 < elevator.currentfloor && elevator.direction == "down"){
                        bestElevatorInfo = checkElevator(3, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInfo = checkElevator(4, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else{
                        bestElevatorInfo = checkElevator(5, elevator, requestedFloor, bestElevatorInfo);
                    }
                    // hmmmm?
                }
            }
            else{
                foreach(Elevator elevator in elevatorsList){
                    if(requestedFloor == elevator.currentfloor && elevator.status == "stopped" && requestedDirection == elevator.direction){
                        bestElevatorInfo = checkElevator(1, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(requestedFloor > elevator.currentfloor  && elevator.direction == "up" && requestedDirection == "up"){
                        bestElevatorInfo = checkElevator(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(requestedFloor < elevator.currentfloor  && elevator.direction == "down" && requestedDirection == "down"){
                        bestElevatorInfo = checkElevator(2, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInfo = checkElevator(4, elevator, requestedFloor, bestElevatorInfo);
                    }
                    else{
                        bestElevatorInfo = checkElevator(5, elevator, requestedFloor, bestElevatorInfo);
                    }
                }
            }
            return (Elevator)bestElevatorInfo["bestElevator"]; // need to make sure this works
        }

        public Hashtable checkElevator(int baseScore, Elevator elevator, int floor, Hashtable bestElevatorInfo){
            if(baseScore < (int)bestElevatorInfo["bestScore"]){
                bestElevatorInfo["bestScore"] = baseScore;
                bestElevatorInfo["bestElevator"] = elevator;
                bestElevatorInfo["referenceGap"] = (int)Math.Abs((double)elevator.currentfloor - floor);
            }
            else if((int)bestElevatorInfo["bestScore"] == baseScore){
                int gap = (int)Math.Abs((double)elevator.currentfloor - floor);
                if((int)bestElevatorInfo["referenceGap"] > gap){
                    bestElevatorInfo["bestElevator"] = elevator;
                    bestElevatorInfo["referenceGap"] = gap;
                }
            }
            return bestElevatorInfo;
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
            this.door = new Door(id, "closed");
            this.floorRequestList = new List<int>(){}; // will have to come back to this
        }

        public void move(){
            while(floorRequestList.Count != 0){
                int destination = floorRequestList[0];
                status = "moving";
                if(currentfloor < destination){
                    direction = "up";
                    while(currentfloor < destination){
                        currentfloor++;
                    }
                }
                else if(currentfloor > destination){
                    direction = "down";
                    while(currentfloor > destination){
                        currentfloor--;
                    }
                }
                status = "idle";
                floorRequestList.RemoveAt(0);
            }
        }

        public void sortFloorList(){
           if(direction == "up"){
               floorRequestList.Sort();
           }
           else{
               floorRequestList.Reverse();
           }
        }

        public void openDoors(){
            door.status = "open";
            door.status = "closed";
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

            var testHash = new Hashtable(){
                {"number", 6}
            };

            testHash["number"] = (int)Math.Abs((double)15 - 2.5);

            Console.WriteLine(testHash["number"]);

            
        }

    }
}


