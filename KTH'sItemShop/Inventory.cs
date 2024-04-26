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
    public class Inventory
    {
        private List<Item> _items = new List<Item>(); // 아이템 리스트 추가
        public IReadOnlyList<Item> Items => _items.AsReadOnly(); // 외부 읽기 전용 접근

        private EquipmentManager _equipmentManager;

        public event Action OnReturnToMainMenu; // 이벤트를 추가합니다.



        Item Item { get; set; }
        public Inventory(EquipmentManager equipmentManager)
        {
            _equipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public void ClearItem()
        {
            _items.Clear();
        }

        public void ShowInventory()
        {
            Console.Clear();

            Console.WriteLine("인벤토리");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            // 아이템 명단이 들어 있을 곳
            foreach (var item in _items)
            {
                string status = item.IsEquipped ? "[E]" : " ";
                string itemInfo = $"{status}{item.Name} | ";
                if (item.AttackBonus.HasValue)
                    itemInfo += $"공격력 +{item.AttackBonus} | ";
                else if (item.DefenseBonus.HasValue)
                    itemInfo += $"방어력 +{item.DefenseBonus} | ";
                itemInfo += item.Description;

                Console.WriteLine(" - " + itemInfo);
            }

            Console.WriteLine();

            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            string choice;
            do
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _equipmentManager.EquipedItem();
                        break;

                    case "0":
                        OnReturnToMainMenu?.Invoke(); // 이벤트 호출
                        break;
                    default:
                        Console.WriteLine("\n잘못된 선택입니다. 올바른 번호를 입력해주세요.");
                        Console.WriteLine();
                        break;
                }
            } while (choice != "0");
        }
    }
}
