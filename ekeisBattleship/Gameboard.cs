using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekeisBattleship
{
    internal class Gameboard
    {
        private char[,] board = new char[10, 10];
        private List<Ship> battleships = new List<Ship>();
        Boolean hacksEnabled = false;

        public char[,] Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
            }
        }

        internal List<Ship> Battleships { get => battleships; set => battleships = value; }
        public bool HacksEnabled { get => hacksEnabled; set => hacksEnabled = value; }

        public void SetupBoard()
        {
            FillGrid();

            GenerateShip(ShipType.CARRIER);
            GenerateShip(ShipType.BATTLESHIP);
            GenerateShip(ShipType.SUBMARINE);
            GenerateShip(ShipType.SUBMARINE);
            GenerateShip(ShipType.DESTROYER);
            GenerateShip(ShipType.DESTROYER);
        }

        private char GetRandomDirection()
        {
            Random random = new Random();
            int num = random.Next(0, 2);

            return num == 0 ? 'H' : 'V';
        }

        private int[] GetRandomLocation()
        {
            Random rand = new Random();
            int row = rand.Next(0, 10);
            int col = rand.Next(0, 10);

            return new int[] { row, col };
        }

        private void GenerateShip(ShipType shipType)
        {
            Boolean isValid = false;
            Ship ship = new Ship(shipType);
            char dir = ' ';
            int[] start = { 0, 0 };

            while (!isValid)
            {
                dir = GetRandomDirection();
                start = GetRandomLocation();

                isValid = IsValidPlacement(ship, dir, start);
            }

            ship.Direction = dir;
            ship.Start = start;
            battleships.Add(ship);
            DrawShip(ship);
        }

        public Boolean IsValidPlacement(Ship ship, char dir, int[] start)
        {
            Boolean isValid = true;

            if (dir == 'V' && start[0] + ship.Size - 1 < board.GetLength(0))
            {
                for (int i = 0; i < ship.Size && isValid; i++)
                {
                    if (board[start[0] + i, start[1]] == 'S')
                    {
                        isValid = false;
                    }
                }
            }
            else if (dir == 'H' && start[1] + ship.Size - 1 < board.GetLength(1))
            {
                for (int i = 0; i < ship.Size && isValid; i++)
                {
                    if (board[start[0], start[1] + i] == 'S')
                    {
                        isValid = false;
                    }
                }
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        public Ship locateShip(int row, int col)
        {
            foreach (Ship ship in battleships)
            {
                if (ship.Direction == 'V')
                {
                    if ((row >= ship.Start[0] && row <= ship.Start[0] + ship.Size - 1) && col == ship.Start[1])
                    {
                        return ship;
                    }
                }
                else
                {
                    if ((col >= ship.Start[1] && col <= ship.Start[1] + ship.Size - 1) && row == ship.Start[0])
                    {
                        return ship;
                    }
                }
            }
            return null;
        }

        public void CheckForSunkenShips()
        {
            for (int i = battleships.Count - 1; i >= 0; i--)
            {
                if (battleships[i].Health == 0)
                {
                    Console.WriteLine("You successfully sunk " + battleships[i].Name + "!\n");
                    battleships.RemoveAt(i);
                }
            }
        }

        public Boolean IsGameOver()
        {
            if (battleships.Count == 0)
            {
                return true;
            }
            return false;
        }

        public char GetChar(int row, int col)
        {
            return board[row, col];
        }

        public void SetChar(int row, int col, char aChar)
        {
            if (row < board.GetLength(0) && col < board.GetLength(1))
            {
                board[row, col] = aChar;
            }
            else
            {
                Console.WriteLine($"Invalid Location: {row}:{col}");
            }
        }

        public void FillGrid()
        {
            FillGrid(' ');
        }

        /// <summary>
        /// Fills a grid with a specified Character.
        /// </summary>
        /// <param name="aChar"></param>
        public void FillGrid(char aChar)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = aChar;
                }
            }
        }

        public void Display()
        {
            char rowChar = 'A';
            for (int row = 0; row < board.GetLength(0); row++)
            {
                DrawLine();
                Console.Write(" " + rowChar + " ");
                rowChar++;
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (hacksEnabled)
                    {
                        Console.Write($"| {board[row, col]} ");
                    }
                    else
                    {
                        if (board[row, col] == 'S')
                        {
                            Console.Write($"|   ");
                        }
                        else
                        {
                            Console.Write($"| {board[row, col]} ");
                        }
                    }
                }
                Console.WriteLine("|");
            }
            DrawLine();
            DrawColumnNumbers();
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// Draws a line for the game board
        /// 
        /// </summary>
        private void DrawLine()
        {
            Console.Write("   ");
            for (int col = 0; col < board.GetLength(1) * 4 + 1; col++)
            {
                Console.Write($"-");
            }
            Console.WriteLine();
        }

        private void DrawColumnNumbers()
        {
            Console.Write("    ");
            for (int col = 0; col < board.GetLength(1); col++)
            {
                Console.Write($" {col + 1}  ");
            }
            Console.WriteLine();
        }

        public void DrawShip(Ship ship)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                if (ship.Direction == 'H')
                {
                    board[ship.Start[0], ship.Start[1] + i] = 'S';
                }
                else
                {
                    board[ship.Start[0] + i, ship.Start[1]] = 'S';
                }
            }
        }
    }
}
