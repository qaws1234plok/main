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
    public class RestPoint
    {
        private Player _player;
        public RestPoint(Player player)
        {
            _player = player;
        }

        public void RestShowMenu()
        {
            while (true)
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
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        public void SpendMoneyGold()
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

}
