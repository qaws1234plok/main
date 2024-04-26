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
    public class ManagementGameData
    {
        private Player _player;
        private Inventory _inventory;

        public ManagementGameData(Player player , Inventory inventory)
        {
            _player = player;
            _inventory = inventory;
        }

        public InventoryData ConvertToInventoryData()
        {
            InventoryData inventoryData = new InventoryData();
            foreach (var item in _inventory.Items)
            {
                inventoryData.Items.Add(new InventoryData.ItemData
                {
                    Name = item.Name,
                    IsEquipped = item.IsEquipped,
                    AttackBonus = item.AttackBonus,
                    DefenseBonus = item.DefenseBonus,
                    Description = item.Description,
                    Price = item.Price,
                    Type = item.Type
                });
            }
            return inventoryData;
        }

        public void ManageGameData()
        {
            Console.WriteLine("1. 데이터 저장하기");
            Console.WriteLine("2. 데이터 불러오기");

            Console.Write(">> ");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    CurrentPlayerData saveData = new CurrentPlayerData
                    {
                        Name = _player.Name,
                        Level = _player.Level,
                        Health = _player.Health,
                        BaseAttackPower = _player.baseAttackPower,
                        BaseDefencePower = _player.baseDefencePower,
                        attackPowerBonus = _player.AttackPowerBonus,
                        defencePowerBonus = _player.DefencePowerBonus,
                        
                        moneyGold = _player.MoneyGold

                    };
                    CurrentPlayerData.SaveCurrentPlayerData(saveData);

                    InventoryData currentInventoryData = ConvertToInventoryData();
                    InventoryData.SaveInventoryData(currentInventoryData);


                    Console.WriteLine("데이터가 저장되었습니다.");
                    break;

                case "2":
                    try
                    {
                        CurrentPlayerData loadData = CurrentPlayerData.LoadCurrentPlayerData();
                        _player.Name = loadData.Name;
                        _player.Level = loadData.Level;
                        _player.Health = loadData.Health;
                        _player.baseAttackPower = loadData.BaseAttackPower;
                        _player.baseDefencePower = loadData.BaseDefencePower;
                        _player.AttackPowerBonus = loadData.attackPowerBonus;
                        _player.DefencePowerBonus = loadData.defencePowerBonus;
                        _player.MoneyGold = loadData.moneyGold;
                        InventoryData loadedInventoryData = InventoryData.LoadInventoryData(); // 인벤토리 데이터 로드
                        _inventory.ClearItem(); // 기존 인벤토리를 클리어
                        foreach (var itemData in loadedInventoryData.Items)
                        {
                            Item newItem = new Item(
                            itemData.Name,
                            itemData.AttackBonus,
                            itemData.DefenseBonus,
                            itemData.Description,
                            itemData.Price,
                            itemData.Type,
                            itemData.IsEquipped,
                            itemData.IsPurchased
                            );
                            _inventory.AddItem(newItem); // 로드된 아이템을 인벤토리에 추가
                        }
                        Console.WriteLine("데이터가 로드되었습니다.");
                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
        }

 
    }
}
