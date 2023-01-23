using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ekeisBattleship
{
    internal class Ship
    {
        private int size;
        private ShipType name;
        private char direction;
        private int[] start;
        private int health;

        public Ship(ShipType type)
        {
            this.size = (int)type;
            this.name = type;
            start = new int[2];
            health = size;
        }

        public int Size { get => size; }
        public char Direction { get => direction; set => direction = value; }
        public ShipType Name { get => name; }
        public int[] Start { get => start; set => start = value; }
        public int Health { get => health; set => health = value; }
    }
}
