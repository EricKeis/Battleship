using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekeisBattleship
{
    internal class Game
    {
        private Gameboard board = new Gameboard();

        public void Run()
        {
            Boolean isGameOver = false;
            Boolean isPlaying = true;
            while (isPlaying)
            {
                board.SetupBoard();
                board.Display();
                while (!isGameOver)
                {
                    PlayerTurn();
                    board.Display();
                    board.CheckForSunkenShips();
                    isGameOver = board.IsGameOver();
                }
                Console.WriteLine("Congrats, You successfully sunk all the ships!");
                Console.WriteLine("\n\n");

                Console.WriteLine("Do you want to play again (Y/N)?");

                char answer = Char.ToLower(Console.ReadLine()[0]);
                isPlaying = answer == 'y' ? true : false;
                isGameOver = false;
            }
        }

        private void PlayerTurn()
        {
            int row = 0, col = 0;
            Boolean isValid = true;
            string target = "";

            do
            {
                isValid = true;

                Console.WriteLine("What's our target? (Enter \"-1\" to enable hacks)");
                target = Console.ReadLine().ToUpper();

                if (target.Equals("-1"))
                {
                    board.HacksEnabled = !board.HacksEnabled;
                }
                else
                {
                    row = target[0] - 65;
                    col = int.Parse(target.Substring(1)) - 1;

                    if (row >= 0 && row < board.Board.GetLength(0) && 
                        col >= 0 && col < board.Board.GetLength(1))
                    {
                        isValid = board.GetChar(row, col) != 'X' && board.GetChar(row, col) != 'O';

                        if (!isValid)
                        {
                            Console.WriteLine("Target already hit!");
                        }
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            } while (!isValid);
            
            if (!target.Equals("-1"))
            {
                Ship ship = board.locateShip(row, col);
                if (ship != null)
                {
                    board.SetChar(row, col, 'X');
                    ship.Health -= 1;
                    Console.WriteLine("\nYou got a HIT!");
                }
                else
                {
                    board.SetChar(row, col, 'O');
                }
            }
        }
    }
}
