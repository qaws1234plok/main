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
    public class EquipmentManager
    {
        private Inventory _inventory;
        private Player _player;  // Player 객체 참조

        public EquipmentManager(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public void Initialize(Inventory inventory)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }
        public void ToggleItemEquipped(int itemIndex)
        {
            if (itemIndex < 1 || itemIndex > _inventory.Items.Count)
                throw new ArgumentException("Invalid item index.");

            Item selectedItem = _inventory.Items[itemIndex - 1];
            if (selectedItem.IsEquipped)
            {
                _player.UnequipedItem(selectedItem);
                selectedItem.IsEquipped = false;
            }
            else
            {
                _player.EquipedItem(selectedItem);
                selectedItem.IsEquipped = true;
            }
        }
        public void EquipedItem()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");
            // 아이템 목록 표시할 곳
            int itemIndex = 1; // 아이템 목록의 인덱스를 위한 변수
            foreach (var item in _inventory.Items)  // Inventory의 아이템 목록 접근
            {
                string status = item.IsEquipped ? "[E]" : " ";
                string itemInfo = $"{status}{item.Name} | ";
                if (item.AttackBonus.HasValue)
                    itemInfo += $"공격력 +{item.AttackBonus} | ";
                else if (item.DefenseBonus.HasValue)
                    itemInfo += $"방어력 +{item.DefenseBonus} | ";
                itemInfo += item.Description;

                Console.WriteLine($" - {itemIndex} {itemInfo}");
                itemIndex++;
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            Console.WriteLine("장착을 원하시는 아이템을 입력해주세요. 이미 같은 유형의 장비가 장착된 경우에는 탈착과 장착이 동시에 이루어 집니다.");
            Console.WriteLine("");
            Console.Write(">> ");

            string choice = Console.ReadLine();
            if (int.TryParse(choice, out int chosenIndex))
            {
                if (chosenIndex == 0)
                {
                    Console.WriteLine("인벤토리 화면으로 돌아갑니다.");
                    _inventory.ShowInventory();
                }
                else if (chosenIndex > 0 && chosenIndex <= _inventory.Items.Count)
                {
                    Item selectedItem = _inventory.Items[chosenIndex - 1];
                    if (selectedItem.IsEquipped)
                    {
                        _player.UnequipedItem(selectedItem);
                        selectedItem.IsEquipped = false;
                        Console.WriteLine($"{selectedItem.Name}가 탈착되었습니다.");
                    }
                    else
                    {
                        Item currentlyEquippedItem = _inventory.Items.FirstOrDefault(item => item.Type == selectedItem.Type && item.IsEquipped);
                        if (currentlyEquippedItem != null)
                        {
                            _player.UnequipedItem(currentlyEquippedItem);
                            currentlyEquippedItem.IsEquipped = false;
                            Console.WriteLine($"{currentlyEquippedItem.Name}가 탈착되었습니다.");
                        }

                        _player.EquipedItem(selectedItem);
                        selectedItem.IsEquipped = true;
                        Console.WriteLine($"{selectedItem.Name}가 장착되었습니다.");
                    }
                    EquipedItem();
                }
                else
                {
                    Console.WriteLine("잘못된 선택입니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

        }
    }
}
