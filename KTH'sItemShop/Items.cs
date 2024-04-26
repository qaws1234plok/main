using System;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TextKTHitemShop
{
    public enum ItemSeries
    {
        ApprenticeArmor,
        IronArmor,
        SpartanArmor,
        BerserkArmor,
        OldSword,
        BronzeAxe,
        SpartanSpear,
        GutsSword
    }

    public enum ItemType
    {
        Weapon,
        Armor
    }
    public class Item
    {
        public string Name { get; set; }

        public int? AttackBonus { get; set; } = null;
        public int? DefenseBonus { get; set; } = null;
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsEquipped { get; set; }
        public ItemType Type { get; set; }



        public Item(string name, int? attackBonus, int? defenseBonus, string description, int price, ItemType type, bool isEquipped, bool isPurchased)
        {
            Name = name;
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
            Description = description;
            Price = price;
            Type = type;
            IsEquipped = isEquipped;
            IsPurchased =isPurchased;
        }

        public string PriceFormatted => $"{Price:N0} G";

        public override string ToString()
        {
            string result = $"{Name} | ";

            // 공격력 또는 방어력 보너스를 표시
            if (AttackBonus.HasValue)
                result += $"공격력 +{AttackBonus} | ";
            else if (DefenseBonus.HasValue)
                result += $"방어력 +{DefenseBonus} | ";

            result += $"{Description} | ";

            // 구매 여부에 따라 가격을 표시하거나 '구매완료'를 표시
            if (IsPurchased)
                result += "구매완료";
            else
                result += $"{Price}";

            return result;
        }
    }

}
