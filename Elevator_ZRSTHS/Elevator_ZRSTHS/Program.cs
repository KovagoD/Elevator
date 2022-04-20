﻿using System;
using System.IO;
using System.Linq;

namespace Elevator_ZRSTHS
{
    class Elevator
    {
        bool isFinished = false;

        public int floorCount;
        public int currentFloor = 0;

        public int maxCapacity;
        public int currentCapacity = 0;

        public bool   = true;

        int turnCount = 0;
        int[] passengersOnBoard;
        int[][] passengers;

        public Elevator(string[] input)
        {
            floorCount = int.Parse(input[0].Split(' ')[0]);
            maxCapacity = int.Parse(input[0].Split(' ')[1]);
            passengersOnBoard = new int[maxCapacity];
            //new int[maxCapacity];
            passengers = new int[floorCount][];

            for (int i = 1; i < floorCount + 1; i++)
            {
                int[] tmp = Array.ConvertAll<string, int>(input[i].Split(' '), int.Parse);
                passengers[i - 1] = tmp;
            }

            Simulate();
        }

        public void Simulate()
        {
            while (!isFinished)
            {

                if (isGoingUp)
                {
                    GoUp();
                }
                else
                {
                    GoDown();
                }
                isFinished = isAllPassengersGone();

            }

            Console.WriteLine("finsihed: "+isFinished +" " + turnCount);
        }

        public void GoUp()
        {
            DEBUGInfo();

            CheckPassengersOnBoard();
            string tmp_1 = "";
            foreach (var item in passengersOnBoard) { tmp_1 += item + ", "; }
            Console.WriteLine("\t-------passengers on board: " + tmp_1);

            currentFloor++;
            if (currentFloor == floorCount - 1) { isGoingUp = false; }
        }

        public void GoDown()
        {
            DEBUGInfo();

            CheckPassengersOnBoard();
            string tmp_1 = "";
            foreach (var item in passengersOnBoard) { tmp_1 += item + ", "; }
            Console.WriteLine("\t-------passengers on board: " + tmp_1);

            currentFloor--;
            if (currentFloor == 0) { isGoingUp = true; turnCount++; }
        }

        public void CheckPassengersOnBoard()
        {
            for (int i = 0; i < passengersOnBoard.Length; i++)
            {
                if (passengersOnBoard[i] == (currentFloor + 1))
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tleszállt: --> " + passengersOnBoard[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                    passengersOnBoard[i] = 0;
                }

                if (passengersOnBoard[i] == 0)
                {
                    int tmp = GetClosestWaitingPassenger();
                    passengersOnBoard[i] = tmp != -1 ? tmp : 0;
                }
            }
                string tmp_2 = "";
                for (int j = 0; j < passengers[currentFloor].Length; j++) { tmp_2 += passengers[currentFloor][j] + ", "; }
                Console.WriteLine("\tpassengers[currentFloor]: " + tmp_2);
        }

        public int GetClosestWaitingPassenger()
        {
            int difference = int.MaxValue;
            int result = -1;
            int index = -1;


            int len = 0;
            for (int i = 0; i < passengers[currentFloor].Length; i++)
            {
                if (passengers[currentFloor][i] != 0)
                    len++;
            }

            int[] newArray = new int[len];
            for (int i = 0, j = 0; i < passengers[currentFloor].Length; i++)
            {
                if (passengers[currentFloor][i] != 0)
                {
                    newArray[j] = passengers[currentFloor][i];
                    j++;
                }
            }
            passengers[currentFloor] = newArray;
            //Console.WriteLine(passengers[currentFloor].Length+" length");

            for (int i = 0; i < passengers[currentFloor].Length; ++i)
            {
                if (passengers[currentFloor][i] != 0 && ((isGoingUp && passengers[currentFloor][i] > currentFloor + 1) || (!isGoingUp && passengers[currentFloor][i] < currentFloor + 1)))
                {
                    
                    if (!isGoingUp)
                    {
                        Array.Sort(passengers[currentFloor]);
                    }
                    else
                    {
                        Array.Reverse(passengers[currentFloor]);
                    }
                   

                    if (difference > AbsoluteValue(passengers[currentFloor][i], currentFloor + 1))
                    {
                        difference = AbsoluteValue(passengers[currentFloor][i], currentFloor + 1);
                        result = passengers[currentFloor][i];
                        index = i;
                    }
                }
            }

            if (result != -1)
            {
                passengers[currentFloor][index] = 0;
                Console.Write("\t-Closest of element " + (currentFloor + 1) + " is : " + result + "\n");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\tfelszállt: <-- " + result);
                Console.ForegroundColor = ConsoleColor.White;

            

            }

            return result;
        }

        public int AbsoluteValue(int a, int b)
        {
            return a > b ? a - b : b - a;
        }
       
        public bool isAllPassengersGone()
        { 
            bool result = true;
            for (int i = 0; i < floorCount-1; i++)
            {
                if (passengers[i].Length>0)
                {
                    result = false;
                }
            }

            for (int i = 0; i < passengersOnBoard.Length; i++)
            {
                if (passengersOnBoard[i]>0)
                {
                    result = false;
                }
            }
            return result;
        }
        
        public void DEBUGInfo()
        {
            Console.ForegroundColor = isGoingUp ? ConsoleColor.Red : ConsoleColor.Blue;
            Console.WriteLine(currentFloor + 1 + ". floor: ----------------------------------");
            Console.ForegroundColor = ConsoleColor.White;

            string tmp_1 = "";
            foreach (var item in passengersOnBoard) { tmp_1 += item + ", "; }

            string tmp_2 = "";
            for (int j = 0; j < passengers[currentFloor].Length; j++) { tmp_2 += passengers[currentFloor][j] + ", "; }

            Console.WriteLine("\t-isGoingUp: " + isGoingUp + "\n\t-turn count: " + turnCount + " \n\t-passengers on board: " + tmp_1 + "\n\t-passengers on floor: " + tmp_2);
            Console.WriteLine("\n");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Elevator elevator = new Elevator(File.ReadAllLines(@"LIFT.BE"));
            Console.ReadKey();
        }
    }
}
