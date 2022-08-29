using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace Server
{
    class SQLiteManager
    {
        // !!! TEMP
        private static string _dbPath = @"d:\temp.db";
        private int _maxInventorySize = 30;
        private int _maxEquipmentSize = 5;

        public SQLiteManager()
        {
            if (!File.Exists(_dbPath))
            {
                CreateDB();
            }
        }

        // -------------------------------------------------------------------------
        // DB 초기화
        // -------------------------------------------------------------------------
        private void CreateDB()
        {
            SQLiteConnection.CreateFile(_dbPath);

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();

                // Players 테이블
                command.CommandText =
                @"
                    CREATE TABLE Players
                    (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        equipment TEXT,
                        posX REAL,
                        posY REAL
                    )
                ";
                command.ExecuteNonQuery();

                // Inventory 테이블
                command.CommandText =
                @"
                    CREATE TABLE Inventories
                    (
                        id INTEGER PRIMARY KEY,
                        items TEXT,
                        FOREIGN KEY(id) REFERENCES Players(id)
                    )
                ";
                command.ExecuteNonQuery();

                // Items 테이블
                command.CommandText =
                @"
                    CREATE TABLE Items
                    (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        type INTEGER NOT NULL,
                        rank INTEGER NOT NULL,
                        description TEXT,
                        damage INTEGER,
                        attackSpeed REAL,
                        criticalPercent REAL,
                        defense INTEGER,
                        maxHp INTEGER,
                        hp INTEGER,
                        recoverHp INTEGER,
                        speed REAL
                    )
                ";
                command.ExecuteNonQuery();
            }
        }

        private void InitItemDB()
        {
            // Item.csv to DB
        }

        // -------------------------------------------------------------------------
        // Players
        // -------------------------------------------------------------------------
        public Player TryGetPlayer(string name)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT *
                    FROM Players
                    WHERE name = $name
                ";
                command.Parameters.AddWithValue("$name", name);

                bool result = false;
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    result = reader.Read();

                    if (result)
                    {
                        return new Player
                        (
                            (int)(long)reader["id"],
                            (string)reader["name"],
                            (float)reader["posX"],
                            (float)reader["posY"],
                            (string)reader["equipment"]
                        );
                    }
                }

                //if (result == false)
                //{
                //    Console.WriteLine("false");
                command.CommandText =
                @"
                    INSERT INTO Players (name)
                    VALUES ($name)
                ";
                command.ExecuteNonQuery();
                //new String('d', )
                //    Console.WriteLine(temp);
                //}

                return null;
            }
        }

        public Player CreatePlayer(string name)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO Players (name)
                    VALUES ($name)
                ";
                command.Parameters.AddWithValue("$name", name);
                int id = command.ExecuteNonQuery();

                command.CommandText =
                @"
                    INSERT INTO Inventories (id)
                    VALUES ($id)
                ";
                command.ExecuteNonQuery();

                return null;
            }
        }
    }
}
