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
            hero.MoneyGold = 1500;

            ShopRtanGame game = new ShopRtanGame(hero);
            game.StartGame();
        }
    }

    public class ShopRtanGame
    {
        public Player Player { get; private set; }
        public CurrentPlayerData PlayerData { get; private set; }

        public Inventory Inventory { get; private set; }
        public TheItemShop TheItemShop { get; private set; }

        public Dungeon Dungeon { get; private set; }

        public RestPoint RestPoint { get; private set; }

        public ManagementGameData ManagementGameData { get; private set; }

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
            ManagementGameData = new ManagementGameData(player, Inventory);
        }

        public void StartGame()
        {
            MainMenu menu = new MainMenu(this);
            Inventory.OnReturnToMainMenu += menu.MainShow; // 이벤트 구독
            menu.MainShow();
        }
    }  
}
