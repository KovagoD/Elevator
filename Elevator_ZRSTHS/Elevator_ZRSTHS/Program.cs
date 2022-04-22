using System;
using System.IO;
using System.Linq;

namespace Elevator_ZRSTHS
{
    class Elevator
    {
        bool isFinished = false;
        bool isGoingUp = true;

        int floorCount;
        int currentFloor = 0;

        public int turnCount = 0;
        int[] passengersOnBoard;
        int[][] passengers;

        public Elevator(string[] input)
        {
            floorCount = int.Parse(input[0].Split(' ')[0]);
            passengersOnBoard = new int[int.Parse(input[0].Split(' ')[1])];
            passengers = new int[floorCount][];

            for (int i = 1; i < floorCount + 1; i++)
            {
                int[] tmp = Array.ConvertAll<string, int>(input[i].Split(' '), int.Parse);
                passengers[i - 1] = RemoveZeros(tmp);
            }

            Simulate();
        }

        void Simulate()
        {
            while (!isFinished)
            {
                if (isGoingUp) { GoUp(); }
                else { GoDown(); }
                isFinished = IsAllPassengersGone();
            }

            Console.WriteLine("\nSimulation finished: " + turnCount+ " turns to transport all passengers");
        }

        void GoUp()
        {
            DEBUGInfo();
            CheckPassengersOnBoard();
            currentFloor++;
            if (currentFloor == floorCount - 1) { isGoingUp = false; }
        }

        void GoDown()
        {
            DEBUGInfo();
            CheckPassengersOnBoard();
            currentFloor--;
            if (currentFloor == 0) { isGoingUp = true; turnCount++; }
        }

        public void CheckPassengersOnBoard()
        {
            for (int i = 0; i < passengersOnBoard.Length; i++)
            {
                if (passengersOnBoard[i] == (currentFloor + 1))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t- got out of the lift: --> " + passengersOnBoard[i]);
                    Console.ForegroundColor = ConsoleColor.White;

                    passengersOnBoard[i] = 0;
                }

                if (passengersOnBoard[i] == 0)
                {
                    int tmp = GetClosestWaitingPassenger();
                    passengersOnBoard[i] = tmp != -1 ? tmp : 0;
                }
            }
        }

        public int GetClosestWaitingPassenger()
        {
            int difference = int.MaxValue;
            int result = -1;
            int index = -1;

            for (int i = 0; i < passengers[currentFloor].Length; ++i)
            {
                if (passengers[currentFloor][i] != 0 && ((isGoingUp && passengers[currentFloor][i] > currentFloor + 1) || (!isGoingUp && passengers[currentFloor][i] < currentFloor + 1)))
                {
                    int absoluteValue = passengers[currentFloor][i] > (currentFloor + 1) ? passengers[currentFloor][i] - (currentFloor + 1) : (currentFloor + 1) - passengers[currentFloor][i];
                    if (difference > absoluteValue)
                    {
                        difference = absoluteValue;
                        result = passengers[currentFloor][i];
                        index = i;
                    }
                }
            }

            if (result != -1)
            {
                passengers[currentFloor][index] = 0;
                passengers[currentFloor] = RemoveZeros(passengers[currentFloor]);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t- got into the lift: <-- " + result);
                Console.ForegroundColor = ConsoleColor.White;
            }
            return result;
        }

        int[] RemoveZeros(int[] array)
        {
            int arrayLength = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0) { arrayLength++; }
            }

            int[] newArray = new int[arrayLength];
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                {
                    newArray[j] = array[i];
                    j++;
                }
            }

            return newArray;
        }

        public bool IsAllPassengersGone()
        {
            bool result = true;
            for (int i = 0; i < floorCount - 1; i++)
            {
                if (passengers[i].Length > 0)
                {
                    result = false;
                }
            }

            for (int i = 0; i < passengersOnBoard.Length; i++)
            {
                if (passengersOnBoard[i] > 0)
                {
                    result = false;
                }
            }
            return result;
        }

        void DEBUGInfo()
        {
            Console.ForegroundColor = isGoingUp ? ConsoleColor.Red : ConsoleColor.Blue;
            Console.WriteLine("\n " + (currentFloor + 1) + ". floor, isGoingUp: " + isGoingUp + ", [" + turnCount + ". turn]:");
            Console.ForegroundColor = ConsoleColor.White;

            string tmp_1 = "";
            foreach (var item in passengersOnBoard) { if (item != 0) { tmp_1 += item + ", "; } }

            string tmp_2 = "";
            for (int i = 0; i < passengers[currentFloor].Length; i++) { tmp_2 += passengers[currentFloor][i] + ", "; }

            Console.WriteLine("\t-passengers on board: " + tmp_1 + "\n\t-passengers on floor: " + tmp_2);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Elevator elevator = new Elevator(File.ReadAllLines(@"LIFT.BE"));
            File.WriteAllTextAsync(@"LIFT.KI", elevator.turnCount.ToString());
        }
    }
}
