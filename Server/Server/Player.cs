using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Pos
    {
        public float X;
        public float Y;

        public Pos(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class Ids
    {
        // 아이템 id list 변환
        public List<int> List;
        public string String
        {
            get
            {
                return String.Join(',', this.List);
            }
        }

        public Ids(string ids)
        {
            this.List = new List<int>(Array.ConvertAll(ids.Split(','), int.Parse));
        }
    }

    class Player
    {
        public int Id;
        public string Name;

        public Pos Pos;
        public Pos DestinationPos;

        public Stat Stat;
        public Ids Inventory;
        public Ids Equipment;

        public Player(int id, string name, float posX, float posY, string equipment)
        {
            this.Id = id;
            this.Name = name;
            this.Pos = new Pos(posX, posY);
            this.DestinationPos = this.Pos;
            this.Equipment = new Ids(equipment);
        }

        public void LoadInventory(string inventory)
        {
            this.Equipment = new Ids(inventory);
        }
    }
}
