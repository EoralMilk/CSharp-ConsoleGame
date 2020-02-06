using System;
using System.Threading;

namespace ConsoleGame
{
    class AeroplaneChess
    {
        #region 全局变量
        //地图信息数组
        public static int[] _map;
        
        //玩家名数组 暂时设定为双人游戏
        public static string[] _playerName = new string[2];

        //玩家位置数组 
        public static int[] _playerPosition = new int[2];

        /*玩家状态数组: 判定与执行顺序由低到高
            0暂停（跳过下一回合），
            1加速（下一回合掷骰子数值+1）,
            2双骰子（下一回合掷两个骰子）
        */
        public static int[,] _playerState = new int[2, 3] {{0,0,0},{0,0,0}};

        //通用随机实例
        public static Random rt = new Random();

        //记录胜利者
        public static int winner = 0;
        //记录回合数
        public static int turnCounter = 0;
        #endregion
        
        #region 显示标题与游戏规则方法
        /// <summary>
        /// 生成标题信息
        /// </summary>
        public static void ShowGameTitle()
        {
            Console.BackgroundColor = ConsoleColor.White ;
            Console.ForegroundColor = ConsoleColor.Blue ;
            Console.Clear();
            Console.WriteLine("==+==+==+==  简单又随机飞行棋  ==+==+==+==");
            Console.ForegroundColor = ConsoleColor.Green ;
            Console.WriteLine("===========  双人掷骰子跳格子  ===========");
            Console.ForegroundColor = ConsoleColor.DarkYellow ;
            Console.WriteLine("===========  五种不同效果格子  ===========");
            Console.ForegroundColor = ConsoleColor.Red ;
            Console.WriteLine("===========  地图完全随机生成  ===========");
            Console.ForegroundColor = ConsoleColor.DarkCyan ;
            Console.WriteLine("===========  先到达终点者获胜  ===========");
        }

        /// <summary>
        /// 显示游戏规则
        /// </summary>
        public static void ShowGameRules()
        {
            Console.ForegroundColor = ConsoleColor.Black ;
            Console.Write("□：平淡无奇\t");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("△：加速\t");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("●：地雷\t");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("×：路障\t");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("卍：随机传送\t");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("☆：来抽卡吧！");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("①：{0}\t②：{1}\t◆：两个玩家重合", _playerName[0], _playerName[1]);
            Console.WriteLine();
        }

        #endregion
        
        #region 初始化地图方法
        /// <summary>
        /// 生成地图信息数组
        /// </summary>
        public static void InitMap()
        {
            _map = new int[100];
            //SpeedUp
            int temp = 0;
            for (int j = 0; j < 20; j++)
            {
                temp = rt.Next(j*5+1,j*5+5);
                if(_map[temp] == 0)
                {
                    _map[temp] = 1;
                }
                else
                {
                    j--;
                }
            }
            //Bomb
            for (int j = 0; j < 5; j++)
            {
                temp = rt.Next(j*20+1,j*20+20);
                if(_map[temp] == 0)
                {
                    _map[temp] = 2;
                }
                else
                {
                    j--;
                }
            }
            //Pause
            for (int j = 0; j < 5; j++)
            {
                temp = rt.Next(j*20+1,j*20+20);
                if(_map[temp] == 0)
                {
                    _map[temp] = 3;
                }
                else
                {
                    j--;
                }
            }
            //Teleport
            for (int j = 0; j < 5; j++)
            {
                temp = rt.Next(j*20+1,j*20+20);
                if(_map[temp] == 0)
                {
                    _map[temp] = 4;
                }
                else
                {
                    j--;
                }
            }
            //Draw
            for (int j = 0; j < 20; j++)
            {
                temp = rt.Next(j*5+1,j*5+5);
                if(_map[temp] == 0)
                {
                    _map[temp] = 5;
                }
                else
                {
                    j--;
                }
            }
        }
        #endregion
        
        #region 绘制地图方法
        /// <summary>
        /// 绘制地块
        /// </summary>
        /// <param name="type">地块类型号</param>
        public static void DrawBlock(int type, int blockIndex)
        {
            //在地图上生成玩家位置信息
            if(_playerPosition[0] != _playerPosition[1])
            {
                if(_playerPosition[0] == blockIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("①");
                    Console.BackgroundColor = ConsoleColor.White;
                    return;
                }
                else if(_playerPosition[1] == blockIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("②");
                    Console.BackgroundColor = ConsoleColor.White;
                    return;
                }
            }
            else if(_playerPosition[0] == blockIndex)
            {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("◆");
                    Console.BackgroundColor = ConsoleColor.White;
                    return;
            }

            switch (type)
            {
                case 0: //Block
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("□");
                    break;
                case 1: //SpeedUp
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("△");
                    break;
                case 2: //Bomb
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("●");
                    break;
                case 3: //Pause
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("×");
                    break;
                case 4: //Teleport
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("卍");
                    break;
                case 5: //Draw
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("☆");
                    break;
                default:
                break;
            }
        }


        /// <summary>
        /// 绘制地图
        /// </summary>
        public static void DrawMap()
        {
            Console.Clear();
            ShowGameRules();

            //绘制地图
            for(int i=0 ; i < 30 ; i++)
            {
                DrawBlock(_map[i], i);
            }
            Console.WriteLine();
            for(int i=30 ; i < 35 ; i++)
            {
                Console.Write("                                                          ");
                DrawBlock(_map[i], i);
                Console.WriteLine();
            }
            for(int i=0 ; i < 30 ; i++)
            {
                DrawBlock(_map[64 - i], 64-i);
            }
            Console.WriteLine();
            for(int i=65 ; i < 70 ; i++)
            {
                DrawBlock(_map[i], i);
                Console.WriteLine();
            }
            for(int i=70 ; i < 100 ; i++)
            {
                DrawBlock(_map[i], i);
            }
            Console.WriteLine();
        } 
        #endregion
        
        #region 玩家行动方法
        /// <summary>
        /// 某一玩家摇骰子
        /// </summary>
        /// <param name="playerNum">玩家序号</param>
        /// <param name="diceVal">骰子值的总和</param>
        public static void RollDice(int playerNum, ref int diceVal)
        {
            int dice_1, dice_2 = 0;

            if(_playerState[playerNum, 0] == 1)
            {
                diceVal = 0;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("{0}晕倒了，{0}这回合不能行动！", _playerName[playerNum]);
                Console.ReadKey();
                return;
            }

            dice_1 = rt.Next(1,7);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("{0}丢出了一个骰子，数值为{1}。", _playerName[playerNum], dice_1);
            Console.ReadKey();

            if(_playerState[playerNum, 2] == 1)
            {
                dice_2 = rt.Next(1,7);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("受到双倍骰子效果影响，{0}丢出了第二个骰子！数值为{1}！", _playerName[playerNum], dice_2);
                Console.ReadKey();

            }
            if(_playerState[playerNum, 1] == 1)
            {
                dice_1 += 1;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("受到加速效果影响，{0}丢出的骰子数值+1，现为{1}" ,_playerName[playerNum], dice_1);
                Console.ReadKey();

                if(_playerState[playerNum, 2] == 1)
                {
                    dice_2 += 1;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("受到加速效果影响，{0}丢出的第二个骰子数值+1，现为{1}！" ,_playerName[playerNum], dice_2);
                    Console.ReadKey();

                }
            }
            diceVal += dice_1 + dice_2;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("{0}将向前跳{1}个格子！", _playerName[playerNum], diceVal);
            Console.ReadKey();

            //清空状态列表
            for(int i = 0; i<3; i++)
            {
                _playerState[playerNum, i] = 0;
            }
        }
        
        /// <summary>
        /// 按照跳跃距离跳跃到某个格子上触发各种效果
        /// </summary>
        /// <param name="playerNum">玩家序号</param>
        /// <param name="diceVal">跳跃距离</param>
        public static void JumpOn(int playerNum, int diceVal)
        {
            _playerPosition[playerNum] += diceVal;
            if(_playerPosition[playerNum] >= 99)
            {
                winner = playerNum +1;
                return;
            }
            else if(_playerPosition[playerNum] <= 0)
            {
                _playerPosition[playerNum] = 0;
            }

            if(_playerState[playerNum, 0] == 1)
            {
                _playerState[playerNum, 0] = 0;
                return;
            }

            switch (_map[_playerPosition[playerNum]])
            {
                case 0:
                    DrawMap();
                    Console.ForegroundColor = ConsoleColor.Black;
                    System.Console.WriteLine("{0}踩到的是平淡无奇的普通格子，什么都没有发生。", _playerName[playerNum]);
                    Console.ReadKey();

                    break;
                case 1:
                    DrawMap();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    System.Console.WriteLine("{0}踩到了加速格子，下回合投掷的骰子数值将加1！", _playerName[playerNum]);
                    Console.ReadKey();
                    //下回合加速
                    _playerState[playerNum , 1] = 1;

                    break;
                case 2:
                    DrawMap();
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("哎呀，{0}踩到了地雷，被炸飞到了6个格以前！", _playerName[playerNum]);
                    Console.ReadKey();
                    JumpOn(playerNum, -6);
                    break;
                case 3:
                    DrawMap();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    System.Console.WriteLine("哎呀，{0}撞在了路障上，撞晕了！", _playerName[playerNum]);
                    Console.ReadKey();
                    //下回合晕倒
                    _playerState[playerNum , 0] = 1;

                    break;
                case 4:
                    DrawMap();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    System.Console.WriteLine("{0}踩到了传送门，一阵天旋地转！", _playerName[playerNum]);
                    Console.ReadKey();
                    _playerPosition[playerNum] = rt.Next(0,101);
                    JumpOn(playerNum, 0);
                    break;
                case 5:
                    if(_playerState[playerNum, 0] == 1)
                    {
                        DrawMap();
                        Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine("{0}处于眩晕中，不能抽卡！", _playerName[playerNum]);
                        Console.ReadKey();
                        break;
                    }
                    DrawMap();
                    Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("{0}踩到了抽卡区，来抽卡吧！", _playerName[playerNum]);
                    Console.ReadKey();

                    //开始抽奖
                    switch (rt.Next(1,6))
                    {
                        case 1: //轰炸对方
                            System.Console.WriteLine("{0}抽到了RPG！{0}用RPG将对方炸飞了！", _playerName[playerNum]);
                            Console.ReadKey();

                            if(playerNum == 0)
                            {
                                JumpOn(1, -6);
                            }
                            else
                            {
                                JumpOn(0, -6);
                            }
                            break;
                        case 2: //砸晕对方
                            System.Console.WriteLine("{0}抽到了板砖！{0}用板砖将对方砸晕了！", _playerName[playerNum]);
                            Console.ReadKey();
                            
                            if(playerNum == 0)
                            {
                                _playerState[1,0] = 1;
                            }
                            else
                            {
                                _playerState[0,0] = 1;
                            }
                            break;
                        case 3: //Double Dice！
                            System.Console.WriteLine("{0}抽到了双骰子体验卷！下回合将多投掷一个骰子！", _playerName[playerNum]);
                            Console.ReadKey();

                            if(playerNum == 0)
                            {
                                _playerState[0,2] = 1;
                            }
                            else
                            {
                                _playerState[1,2] = 1;
                            }
                            break;
                        case 4: //调换位置！
                            System.Console.WriteLine("{0}抽到了？？？，两人的位置对换了！", _playerName[playerNum]);
                            Console.ReadKey();
                            int temp = _playerPosition[0];
                            _playerPosition[0] = _playerPosition[1];
                            _playerPosition[1] = temp;
                            JumpOn(0,0);
                            JumpOn(1,0);
                            break;
                        default:
                            System.Console.WriteLine("{0}什么都没抽到。", _playerName[playerNum]);
                            Console.ReadKey();

                        break;
                    }
                    break;
                default:
                break;
            }
        }


        /// <summary>
        /// 玩家行动
        /// </summary>
        public static void PlayerRoll()
        {
            int diceVal_p1 = 0;
            int diceVal_p2 = 0;
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("第{0}回合开始！随机决定谁先开始行动！", ++turnCounter);
            Console.ReadKey();

            if(winner != 0) return;

            if (rt.Next(1,3) == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("让我们看看{0}的表现！", _playerName[0]);
                Console.ReadKey();
                RollDice(0,ref diceVal_p1);
                JumpOn(0,diceVal_p1);
                if(winner != 0) return;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("让我们看看{0}的表现！", _playerName[1]);
                Console.ReadKey();
                RollDice(1,ref diceVal_p2);
                JumpOn(1,diceVal_p2);
                if(winner != 0) return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("让我们看看{0}的表现！", _playerName[1]);
                Console.ReadKey();
                RollDice(1,ref diceVal_p2);
                JumpOn(1,diceVal_p2);
                if(winner != 0) return;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("让我们看看{0}的表现！", _playerName[0]);
                Console.ReadKey();
                RollDice(0,ref diceVal_p1);
                JumpOn(0,diceVal_p1);
                if(winner != 0) return;
            }

        }
        #endregion


        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ShowGameTitle();
            InitMap();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine("请输入玩家1的名字");
            _playerName[0] =Console.ReadLine();
            System.Console.WriteLine("请输入玩家2的名字");
            _playerName[1] =Console.ReadLine();
            System.Console.WriteLine("现在{0}和{1}将开始游戏！按任意键继续！",_playerName[0], _playerName[1]);
            DrawMap();
            while(true)
            { 
                PlayerRoll();
                if(winner != 0) break;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.WriteLine();
            System.Console.WriteLine("{0}胜利啦！", _playerName[winner-1]);
            System.Console.WriteLine("====游戏结束====");
            Console.ReadKey();
        }
    }
}
