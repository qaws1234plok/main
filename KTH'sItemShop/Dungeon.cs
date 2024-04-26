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

            int minDamage = (int)Math.Round(MinDam);
            int maxDamage = (int)Math.Round(MaxDam);

            int damage = random.Next(minDamage, maxDamage + 1);
            _player.ApplyDamage(damage);
        }

        private void ClearGold(int baseReward)
        {
            Random random = new Random();
            float attackPower = _player.AttackPower * 2;
            int minBonusAttackPorwer = (int)attackPower / 2;

            float bonusAttackPercent = random.Next(minBonusAttackPorwer, (int)attackPower);
            float BonusMoneyGold = baseReward * (1 + bonusAttackPercent / 100);
            _player.MoneyGold += (int)BonusMoneyGold;
            Console.WriteLine($"클리어 보상으로 {(int)BonusMoneyGold}만큼의 골드를 얻으셨습니다. 현재 보유 골드 {_player.MoneyGold}");
        }

    }
}
