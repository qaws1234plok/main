using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextKTHitemShop;

namespace TextKTHitemShop
{
    public class TheItemShop
    {
        private Dictionary<ItemSeries, Item> items;
        private ShopRtanGame _game; // ShopRtanGame 객체를 참조하기 위한 변수

        public TheItemShop(ShopRtanGame game)
        {
            _game = game;
            InitializeItems();
        }

        private void InitializeItems()
        {
            items = new Dictionary<ItemSeries, Item>
            {
            { ItemSeries.ApprenticeArmor, new Item("수련자 갑옷", null, 5, "수련에 도움을 주는 갑옷입니다.", 1000, ItemType.Armor, false, false) },
            { ItemSeries.IronArmor, new Item("무쇠갑옷", null, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500, ItemType.Armor, false, false) },
            { ItemSeries.SpartanArmor, new Item("스파르타의 갑옷", null, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, ItemType.Armor, false, false) },
            { ItemSeries.BerserkArmor, new Item("광전사의 갑주", null, 50, "모든 것을 맡겨라", 15000, ItemType.Armor, false, false) },
            { ItemSeries.OldSword, new Item("낡은 검", 2, null, "쉽게 볼 수 있는 낡은 검 입니다.", 600, ItemType.Weapon, false, false) },
            { ItemSeries.BronzeAxe, new Item("청동 도끼", 5, null, "어디선가 사용됐던 거 같은 도끼입니다.", 1500, ItemType.Weapon, false, false) },
            { ItemSeries.SpartanSpear, new Item("스파르타의 창", 7, null, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000, ItemType.Weapon, false, false) },
            { ItemSeries.GutsSword, new Item("드래곤 슬레이어", 100, null, "그것은 검이라고 하기엔 너무나도 크고 두껍고 조잡했다", 10000, ItemType.Weapon, false, false) }
            };
        }


        public void ShowItemShop()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            // 보유 골드 표시할 곳
            Console.WriteLine($"{_game.Player.MoneyGold:N0} G");

            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            // 아이템 목록 표시할 곳
            foreach (var itemEntry in items) // Dictionary 'items'의 각 아이템에 대해서
            {
                Item item = itemEntry.Value;
                Console.WriteLine($" - {item}"); // 각 아이템 정보 출력
            }

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");

            string choice;
            do
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        BuyItem();
                        break;

                    case "2":
                        SellingItem();
                        break;

                    case "0":
                        _game.StartGame();
                        break;
                    default:
                        Console.WriteLine("\n잘못된 입력입니다.");
                        Console.WriteLine();
                        break;
                }
            } while (choice != "0");

        }

        private void BuyItem()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            // 보유 골드 칸
            Console.WriteLine($"{_game.Player.MoneyGold:N0} G");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");
            // 상점에 있는 아이템 칸
            int itemIndex = 1; // 아이템 목록의 인덱스를 위한 변수
            foreach (var itemEntry in items) // Dictionary 'items'의 각 아이템에 대해서
            {
                Item item = itemEntry.Value;
                Console.WriteLine($"{itemIndex}. {item}"); // 각 아이템 정보 출력
                itemIndex++;
            }
            Console.WriteLine();


            Console.WriteLine("0. 나가기");

            string itemChoice;
            do
            {
                Console.WriteLine("원하시는 행동을 입력하세요.");
                Console.Write(">> ");
                itemChoice = Console.ReadLine();

                if (int.TryParse(itemChoice, out int index) && index > 0 && index <= items.Count)
                {
                    Item itemToPurchase = items.Values.ElementAt(index - 1); // 딕셔너리의 값에서 엘리먼트 접근
                    if (itemToPurchase.IsPurchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                    }
                    else if (_game.Player.MoneyGold >= itemToPurchase.Price)
                    {
                        _game.Player.MoneyGold -= itemToPurchase.Price;
                        itemToPurchase.IsPurchased = true;
                        _game.Inventory.AddItem(itemToPurchase);
                        Console.WriteLine($"{itemToPurchase.Name}를 구매했습니다!");
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                    }
                }
                else if (itemChoice == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            } while (itemChoice != "0");

            Thread.Sleep(500); // 사용자에게 변화를 인지할 시간을 주기 위해 잠시 대기
            ShowItemShop();

        }

        public void SellingItem()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("불필요한 아이템을 팔 수 있는 상점입니다.");
                Console.WriteLine();

                Console.WriteLine("[보유 골드]");
                // 보유 골드 칸
                Console.WriteLine($"{_game.Player.MoneyGold:N0} G");
                Console.WriteLine();

                Console.WriteLine("[아이템 목록]");
                // 상점에 있는 아이템 칸
                int itemIndex = 1;
                foreach (var item in _game.Inventory.Items)  // 인벤토리 아이템만
                {
                    string status = item.IsEquipped ? "[E]" : " ";
                    Console.WriteLine($" - {itemIndex} {status}{item.Name} | {item.DefenseBonus} | {item.Description} | {item.Price * 0.85}G");
                    itemIndex++;
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                Console.WriteLine("판매를 원하시는 아이템을 선택하세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= _game.Inventory.Items.Count)
                {
                    Item selectedItem = _game.Inventory.Items[selectedIndex - 1];

                    if (selectedItem.IsEquipped)
                    {
                        _game.Player.UnequipedItem(selectedItem);
                        selectedItem.IsEquipped = false;
                    }

                    int salePrice = (int)(selectedItem.Price * 0.85);
                    _game.Player.MoneyGold += salePrice;
                    _game.Inventory.RemoveItem(selectedItem);
                    Console.WriteLine($"{selectedItem.Name}를 {salePrice}G에 판매했습니다. 남은 골드: {_game.Player.MoneyGold:N0} G");
                    selectedItem.IsPurchased = false;
                    Thread.Sleep(500);

                }
                else if (selectedIndex == 0)
                {
                    _game.TheItemShop.ShowItemShop();
                    break;  // 0을 입력하면 반복을 종료하고 함수를 빠져나옵니다.
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(250);
                }
            }
        }

    }
}
