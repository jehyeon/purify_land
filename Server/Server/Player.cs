using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public int Hp { get; set; }         // !!! 임시 스탯
        public int MaxHp { get; set; }
    }
}
