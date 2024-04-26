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
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public float baseAttackPower;
        public float baseDefencePower;
        public float AttackPowerBonus { get; set; }
        public float DefencePowerBonus { get; set; }
        public int MoneyGold { get; set; }
        public string JobClass { get; set; } = "전사";

        public int DungeonClearCount = 0;


        // 공격력과 방어력에 대한 프로퍼티
        public float AttackPower => baseAttackPower + AttackPowerBonus;
        public float DefencePower => baseDefencePower + DefencePowerBonus;

        public Player(float initialAttackPower, float initialDefencePower)
        {
            baseAttackPower = initialAttackPower;
            baseDefencePower = initialDefencePower;
        }

        public void PlayerDungeonClerar()
        {
            DungeonClearCount++;
            PlayerLevelUpCheck();
        }

        public void PlayerLevelUpCheck()
        {
            if (DungeonClearCount == Level)
            {
                PlayerLevelUp();
            }
        }

        public void PlayerLevelUp()
        {
            Level += 1;
            DungeonClearCount = 0;
            baseAttackPower += 1.0f;
            baseDefencePower += 0.5f;

            Console.WriteLine($"{Name}이(가) {Level}만큼 강해졌다! 공격력이 1 방어력이 0.5 상승했다!");
        }

        // 아이템 장착
        public void EquipedItem(Item item)
        {
            if (item.AttackBonus.HasValue)
                AttackPowerBonus += item.AttackBonus.Value;
            if (item.DefenseBonus.HasValue)
                DefencePowerBonus += item.DefenseBonus.Value;
        }

        // 아이템 탈착
        public void UnequipedItem(Item item)
        {
            if (item.AttackBonus.HasValue)
                AttackPowerBonus -= item.AttackBonus.Value;
            if (item.DefenseBonus.HasValue)
                DefencePowerBonus -= item.DefenseBonus.Value;
        }
        public void ViewStatus()
        {
            Console.Clear();

            string choice;
            do
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();

                Console.WriteLine($"Lv. {Level}");

                Console.WriteLine($"{Name} ( {JobClass} )");
                Console.WriteLine($"공격력 : {AttackPower} {(AttackPowerBonus != 0 ? $"(+{AttackPowerBonus})" : "")}");
                Console.WriteLine($"방어력 : {DefencePower} {(DefencePowerBonus != 0 ? $"(+{DefencePowerBonus})" : "")}");
                Console.WriteLine($"체 력 : {Health}");
                Console.WriteLine($"Gold : {MoneyGold:N0} G");

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                choice = Console.ReadLine();

            } while (choice != "0");

            Console.Clear();

        }

        public void ApplyDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"{Name}가(이) {damage}만큼의 피해를 받았다! 현재 체력 {Health}");

            if (Health <= 0)
            {
                Console.WriteLine($"무리한 탐험을 하다 {Name}이(가) 사망하였습니다. 게임을 종료합니다.");
                Console.WriteLine("게임을 종료하려면 아무키나 눌러주세요");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public void ApplyRecovery(int carePoint)
        {
            Health += carePoint;
        }
    }
}
