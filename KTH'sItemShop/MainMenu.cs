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
                Console.WriteLine("9. 게임 데이터 관리");
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

                    case "9":
                        _game.ManagementGameData.ManageGameData();
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
}
