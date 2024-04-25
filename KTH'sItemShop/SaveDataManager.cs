using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace TextKTHitemShop
{
    public class CurrentPlayerData
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public int Health {  get; set; }

        public float BaseAttackPower { get; set; } 
        public float BaseDefencePower { get; set; } 

        public int moneyGold { get; set; }


        public static void SaveCurrentPlayerData(CurrentPlayerData data, string fileName = "SaveData.json")
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, jsonString);
        }

        public static CurrentPlayerData LoadCurrentPlayerData(string fileName = "SaveData.json")
        {
            if (!File.Exists(fileName)) 
            {
                throw new FileNotFoundException($"The file {fileName} was not found.");
            }

            string jsonString = File.ReadAllText(fileName); 
            return JsonSerializer.Deserialize<CurrentPlayerData>(jsonString); 
        }

    }
    public class InventoryData
    {
        public List<ItemData> Items { get; set; } = new List<ItemData>();

        public static void SaveInventoryData(InventoryData data, string fileName = "InventoryData.json")
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, jsonString);
        }

        public static InventoryData LoadInventoryData(string fileName = "InventoryData.json")
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"The file {fileName} was not found.");
            }

            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<InventoryData>(jsonString);
        }
    }

    public class ItemData
    {
        public ItemData() { }

        public string Name { get; set; }
        public bool IsEquipped { get; set; }
        public int? AttackBonus { get; set; } 
        public int? DefenseBonus { get; set; } 
        public string Description { get; set; }
        public int Price { get; set; } 
        public ItemType Type { get; set; } 
        public ItemData(string name, int? attackBonus, int? defenseBonus, string description, int price, ItemType type, bool isEquipped)
        {
            Name = name;
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
            Description = description;
            Price = price;
            Type = type;
            IsEquipped = isEquipped;
        }
    }
}

