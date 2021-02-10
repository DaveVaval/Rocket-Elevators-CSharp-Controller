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
            // foreach(Column column in columnsList){
            //     System.Console.WriteLine("column: " + column.ID);

            //     foreach(int floor in column.servedFloors)
            //         System.Console.WriteLine(" serve floor: " + floor);

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
            Column col = null;
            foreach (Column column in columnsList){
                if(column.servedFloors.Contains(requestedFloor)){
                    col = column;
                }
            }
            return col;
        }

        public Elevator assignElevator(int requestedFloor, string direction){
            // foreach(Column col in columnsList)
            //     foreach(Elevator elev in col.elevatorsList)
            //         elev.move();

            Column column = findBestColumn(requestedFloor); // return?
            Elevator elevator = column.findElevator(1, direction); // return?
            elevator.floorRequestList.Add(requestedFloor);
            elevator.sortFloorList();
            elevator.move();
            elevator.openDoors();
            // Console.WriteLine("Elevator " + column.ID  + elevator.ID + " is sent to floor: " + elevator.currentfloor);
            Console.WriteLine("Elevator {0} from column {1} is sent to floor: {2}", elevator.ID, column.ID, elevator.currentfloor);
            return elevator;
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
            this.floorRequestList = new List<int>(); // will have to come back to this
        }

        public void requestFloor(int requestedFloor){
            floorRequestList.Add(requestedFloor);
            sortFloorList();
            move();
            Console.WriteLine("Elevator reached floor: " + this.currentfloor);
            openDoors();
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
            Console.WriteLine("-----------------------------// TESTING //--------------------------------");
            Battery battery = new Battery(1, "online", 4, 60, 6, 5);
            // Console.WriteLine("Amount of columns: " + battery.amountOfColumns);
            void Scenario1(){
                //---------------------------------------------------------// Scenario 1 //------------------------------------------------------------
                // B1
                battery.columnsList[1].elevatorsList[0].currentfloor = 20;
                battery.columnsList[1].elevatorsList[0].direction = "down";
                battery.columnsList[1].elevatorsList[0].floorRequestList.Add(5);

                // B2
                battery.columnsList[1].elevatorsList[1].currentfloor = 3;
                battery.columnsList[1].elevatorsList[1].direction = "up";
                battery.columnsList[1].elevatorsList[1].floorRequestList.Add(15);

                // B3
                battery.columnsList[1].elevatorsList[2].currentfloor = 13;
                battery.columnsList[1].elevatorsList[2].direction = "down";
                battery.columnsList[1].elevatorsList[2].floorRequestList.Add(1);

                // B4
                battery.columnsList[1].elevatorsList[3].currentfloor = 15;
                battery.columnsList[1].elevatorsList[3].direction = "down";
                battery.columnsList[1].elevatorsList[3].floorRequestList.Add(2);

                // B5
                battery.columnsList[1].elevatorsList[4].currentfloor = 6;
                battery.columnsList[1].elevatorsList[4].direction = "down";
                battery.columnsList[1].elevatorsList[4].floorRequestList.Add(1);
                
                // User at lobby want's to go to floor 20, Elevator 5 should be sent
                Console.WriteLine("User is at the lobby and wants to go to floor 20");
                Console.WriteLine("He enters 20 on the pannel");
                Console.WriteLine(".........");
                Elevator elevator = battery.assignElevator(20, "up");
                Console.WriteLine("He enters the elevator");
                elevator.requestFloor(20);
                Console.WriteLine("He gets out...");
            }


            void Scenario2(){
            //--------------------------------------------// Scenario 2 //-----------------------------------------------------
            // C1
                battery.columnsList[2].elevatorsList[0].currentfloor = 1;
                battery.columnsList[2].elevatorsList[0].direction = "up";
                battery.columnsList[2].elevatorsList[0].floorRequestList.Add(21);

                // C2
                battery.columnsList[2].elevatorsList[1].currentfloor = 23;
                battery.columnsList[2].elevatorsList[1].direction = "up";
                battery.columnsList[2].elevatorsList[1].floorRequestList.Add(28);

                // C3
                battery.columnsList[2].elevatorsList[2].currentfloor = 33;
                battery.columnsList[2].elevatorsList[2].direction = "down";
                battery.columnsList[2].elevatorsList[2].floorRequestList.Add(1);

                // C4
                battery.columnsList[2].elevatorsList[3].currentfloor = 40;
                battery.columnsList[2].elevatorsList[3].direction = "down";
                battery.columnsList[2].elevatorsList[3].floorRequestList.Add(24);

                // C5
                battery.columnsList[2].elevatorsList[4].currentfloor = 39;
                battery.columnsList[2].elevatorsList[4].direction = "down";
                battery.columnsList[2].elevatorsList[4].floorRequestList.Add(1);
                
                // User at lobby want's to go to floor 36, Elevator 1 should be sent
                Console.WriteLine("User is at the lobby and wants to go to floor 36");
                Console.WriteLine("He enters 36 on the pannel");
                Console.WriteLine(".........");
                Elevator elevator = battery.assignElevator(36, "up");
                Console.WriteLine("He enters the elevator");
                elevator.requestFloor(36);
                Console.WriteLine("He gets out...");
            }

            //--------------------------------------------// Scenario 3 //-----------------------------------------------------
            // D1
                battery.columnsList[3].elevatorsList[0].currentfloor = 58;
                battery.columnsList[3].elevatorsList[0].direction = "down";
                battery.columnsList[3].elevatorsList[0].floorRequestList.Add(1);

                // D2
                battery.columnsList[3].elevatorsList[1].currentfloor = 50;
                battery.columnsList[3].elevatorsList[1].direction = "up";
                battery.columnsList[3].elevatorsList[1].floorRequestList.Add(60);

                // D3
                battery.columnsList[3].elevatorsList[2].currentfloor = 46;
                battery.columnsList[3].elevatorsList[2].direction = "up";
                battery.columnsList[3].elevatorsList[2].floorRequestList.Add(58);

                // D4
                battery.columnsList[3].elevatorsList[3].currentfloor = 1;
                battery.columnsList[3].elevatorsList[3].direction = "up";
                battery.columnsList[3].elevatorsList[3].floorRequestList.Add(54);

                // D5
                battery.columnsList[3].elevatorsList[4].currentfloor = 60;
                battery.columnsList[3].elevatorsList[4].direction = "down";
                battery.columnsList[3].elevatorsList[4].floorRequestList.Add(1);
                
                // User at floor 54 want's to go to floor 1, Elevator 1 should be sent
                Console.WriteLine("User is at floor 54 and wants to go to the lobby");
                Console.WriteLine("He presses on the pannel");
                Console.WriteLine(".........");
                Elevator elevator = battery.assignElevator(54, "down");
                Console.WriteLine("He enters the elevator");
                elevator.requestFloor(1);
                Console.WriteLine("He gets out...");
        }
    }
}