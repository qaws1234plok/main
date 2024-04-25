using System;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Linq;
using System.Numerics;

namespace TextKTHitemShop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "르탄이가 찾은 상점";

            // 캐릭터 생성
            Player hero = new Player(10.0f, 5.0f);
            Console.Write("캐릭터의 이름을 입력하세요: "); // 이름 입력 부분
            hero.Name = Console.ReadLine(); // ReadLind을 통해 받은 값을 Name프로퍼티에 저장

            // 초기 스텟 설정
            hero.Level = 1;
            hero.Health = 100;
            hero.MoneyGold = 100000;

            ShopRtanGame game = new ShopRtanGame(hero);
            game.StartGame();
        }
    }

    public class ShopRtanGame
    {
        public Player Player { get; private set; }
        public Inventory Inventory { get; private set; }
        public TheItemShop TheItemShop { get; private set; }

        public Dungeon Dungeon { get; private set; }

        public RestPoint RestPoint { get; private set; }

        public ShopRtanGame(Player player)  // 생성자를 통해 Player 객체를 초기화
        {
            Player = player;

            // EquipmentManager를 먼저 생성합니다.
            EquipmentManager equipmentManager = new EquipmentManager(Player);

            // EquipmentManager를 사용하여 Inventory를 생성합니다.
            Inventory = new Inventory(equipmentManager);
            equipmentManager.Initialize(Inventory);  // EquipmentManager에 Inventory를 제공

            TheItemShop = new TheItemShop(this);
            Dungeon = new Dungeon(player);
            RestPoint = new RestPoint(player);
        }


        public void StartGame()
        {
            MainMenu menu = new MainMenu(this);
            Inventory.OnReturnToMainMenu += menu.MainShow; // 이벤트 구독
            menu.MainShow();
        }
    }

    public class MainMenu
    {
        private ShopRtanGame _game;

        public MainMenu(ShopRtanGame game)
        {
            _game = game;
        }

        public void MainShow()
        {
            Console.Clear();

            string choice;
            do
            {
                // 인사 문구
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine(); // 빈줄

                // 선택지
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전탐험");
                Console.WriteLine("5. 휴식");
                Console.WriteLine(); //빈줄

                // 입력창
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                choice = Console.ReadLine(); // 입력한 값을 받아서

                // 여기서 처리
                switch (choice)
                {
                    case "1":
                        _game.Player.ViewStatus();
                        break;

                    case "2":
                        _game.Inventory.ShowInventory();
                        break;

                    case "3":
                        _game.TheItemShop.ShowItemShop();
                        break;
                    
                    case "4":
                        _game.Dungeon.ShowMeDuengon();
                        break;

                    case "5":
                        _game.RestPoint.RestShowMenu();
                        break;

                    default:
                        Console.WriteLine("\n잘못된 선택입니다. 올바른 번호를 입력해주세요.");
                        Console.WriteLine();
                        MainShow();
                        break;
                }
            } while (choice != "0");
        }
    }


    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        private float baseAttackPower;
        private float baseDefencePower;
        public float AttackPowerBonus { get; private set; }
        public float DefencePowerBonus { get; private set; }
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



        public Item(string name, int? attackBonus, int? defenseBonus, string description, int price, ItemType type)
        {
            Name = name;
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
            Description = description;
            Price = price;
            Type = type;
            IsPurchased = false;
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
            { ItemSeries.ApprenticeArmor, new Item("수련자 갑옷", null, 5, "수련에 도움을 주는 갑옷입니다.", 1000, ItemType.Armor) },
            { ItemSeries.IronArmor, new Item("무쇠갑옷", null, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500, ItemType.Armor) },
            { ItemSeries.SpartanArmor, new Item("스파르타의 갑옷", null, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, ItemType.Armor) },
            { ItemSeries.BerserkArmor, new Item("광전사의 갑주", null, 50, "모든 것을 맡겨라", 15000, ItemType.Armor) },
            { ItemSeries.OldSword, new Item("낡은 검", 2, null, "쉽게 볼 수 있는 낡은 검 입니다.", 600, ItemType.Weapon) },
            { ItemSeries.BronzeAxe, new Item("청동 도끼", 5, null, "어디선가 사용됐던 거 같은 도끼입니다.", 1500, ItemType.Weapon) },
            { ItemSeries.SpartanSpear, new Item("스파르타의 창", 7, null, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000, ItemType.Weapon) },
            { ItemSeries.GutsSword, new Item("드래곤 슬레이어", 100, null, "그것은 검이라고 하기엔 너무나도 크고 두껍고 조잡했다", 10000, ItemType.Weapon) }
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
            while(true)
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

    public class Dungeon
    {
        private Player _player;

        private float a = 5.0f;
        private float b = 11.0f;
        private float c = 17.0f;


        public Dungeon(Player player)
        {
            _player = player;
        }

        public void ShowMeDuengon()
        {
            while (true)
            {
                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();

                Console.WriteLine("1. 쉬운 던전");
                Console.WriteLine("2. 일반 던전");
                Console.WriteLine("3. 어려운 던전");
                Console.WriteLine("0. 나가기");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        EasyDuengon(_player);
                        break;

                    case "2":
                        NomalDungeon(_player);
                        break;

                    case "3":
                        HardDungeon(_player);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }

        }

        public void EasyDuengon(Player player) //클리어시 체력 감소
        {
            Random random = new Random();
            if (_player.DefencePower >= a || random.Next(100) < 60)
            {
                Console.WriteLine("던전 클리어!");
                ClearRandomDamage();
                ClearGold(1000);
                _player.PlayerDungeonClerar();
                Thread.Sleep(1000);
                Console.Clear();
            }

            else
            {
                Console.WriteLine("클리어 실패...");
                Console.WriteLine("던전의 입구로 돌아갑니다.");

            }
        }

        public void NomalDungeon(Player player)
        {
            Random random = new Random();
            if (_player.DefencePower >= b || random.Next(100) < 60)
            {
                Console.WriteLine("던전 클리어!");
                ClearRandomDamage();
                ClearGold(1700);
                _player.PlayerDungeonClerar();
                Thread.Sleep(1000);
                Console.Clear();
            }

            else
            {
                Console.WriteLine("클리어 실패...");
                Console.WriteLine("던전의 입구로 돌아갑니다.");
            }
        }

        public void HardDungeon(Player player)
        {
            Random random = new Random();
            if (_player.DefencePower >= c || random.Next(100) < 60)
            {
                Console.WriteLine("던전 클리어!");
                ClearRandomDamage();
                ClearGold(2500);
                _player.PlayerDungeonClerar();
                Thread.Sleep(1000);
                Console.Clear();
            }

            else
            {
                Console.WriteLine("클리어 실패...");
                Console.WriteLine("던전의 입구로 돌아갑니다.");
            }
        }



        private void ClearRandomDamage() // 데미지
        {
            Random random = new Random();
            float DefencePlusMinus = _player.DefencePower - a;

            float MinDam = 20 + DefencePlusMinus;
            float MaxDam = 35 + DefencePlusMinus;

            int minDamage =(int)Math.Round(MinDam);
            int maxDamage = (int)Math.Round(MaxDam);

            int damage = random.Next(minDamage, maxDamage + 1);
            _player.ApplyDamage(damage);
        }

        private void ClearGold(int baseReward)
        {
            Random random = new Random();
            float attackPower = _player.AttackPower *2;
            int minBonusAttackPorwer = (int)attackPower / 2;

            float bonusAttackPercent = random.Next(minBonusAttackPorwer, (int)attackPower);
            float BonusMoneyGold = baseReward * (1 + bonusAttackPercent / 100);
            _player.MoneyGold += (int)BonusMoneyGold;
            Console.WriteLine($"클리어 보상으로 {(int)BonusMoneyGold}만큼의 골드를 얻으셨습니다. 현재 보유 골드 {_player.MoneyGold}");
        }

    }

    public class RestPoint
    {
        private Player _player;
        public RestPoint(Player player)
        {
            _player = player;
        }

        public void RestShowMenu()
        {
            while(true)
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine("500G를 내면 체력을 회복하실 수 있습니다.");
                Console.WriteLine();

                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SpendMoneyGold();
                        HealthCare();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        public void SpendMoneyGold ()
        {
            _player.MoneyGold -= 500;
            Console.WriteLine($"휴식을 위해 500G를 소모하셨습니다. 현재 보유 골드 {_player.MoneyGold}G");
        }

        public void HealthCare()
        {
            int carePoint = 100 - _player.Health;
            _player.ApplyRecovery(carePoint);
        }
    }

    public class PlayerLevelUp()
    {

    }

}
