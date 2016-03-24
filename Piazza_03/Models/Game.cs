using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeW4G2.Models
{
    class Game
    {
        public enum Direction { up, down, left, right};
        public static Direction direction;
        public static int speed = 200;
        public static int ConsoleHeight = 22; // Высота задана по умлочанию, потребуется для того, чтобы выводить индикатор очков и текущего уровня
        public static int countOfEatenFood = 0, numberOfLvl = 1, points = 0; // Индикатор очков для прохождения следуещего уровня, общих очков и текущего уровня
        public static Wall wall = new Wall();
        public static Snake snake = new Snake();
        public static Food food = new Food();
        public static bool GameOver = false;
        public static Thread t1 = new Thread(moveSnake);

        public Game()
        {
            constantsSetup();
            /*t1.IsBackground = true;
            t1.Start();*/
            Timer timer = new Timer(moveSnake);
            timer.Change(0, 100);
            
            while (!GameOver)
            {
                //snake.Draw();
                ConsoleKeyInfo button = Console.ReadKey();
                switch(button.Key)
                {
                    case ConsoleKey.UpArrow:
                        direction = Direction.up;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.down;
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = Direction.left;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.right;
                        break;
                    case ConsoleKey.F1:
                        Save();
                        break;
                    case ConsoleKey.F2:
                        Resume();
                        break;
                    default:
                        break;                        
                }
            }
            //t1.Abort();
        }

        public static void moveSnake(object state)
        {
            /*while (!GameOver)
            { */  
                if (!GameOver)
                {
                    switch (direction)
                    {
                        case Direction.up:
                            snake.move(0, -1);
                            break;
                        case Direction.down:
                            snake.move(0, 1);
                            break;
                        case Direction.left:
                            snake.move(-1, 0);
                            break;
                        case Direction.right:
                            snake.move(1, 0);
                            break;
                    }
                    if (snake.CheckCollisionWithWalls())
                        GameOver = true;
                    if (Game.snake.body[0].x > Console.WindowWidth - 1) //Если змейка перешла размер ширины экрана, то продолжает свое движение с нулевой позиции по горизонтали
                        Game.snake.body[0].x = 0;
                    if (Game.snake.body[0].x < 0) //Если змейка вышла за пределы нулевой позиций по горизонтали, то продолжает свое движение с другого конца экрана по горизонтали
                        Game.snake.body[0].x = Console.WindowWidth - 1;
                    if (Game.snake.body[0].y > ConsoleHeight)  //Если змейка вышла за пределы высоты экрана, то продолжает свое движение с верхней начальной позиции
                        Game.snake.body[0].y = 0;
                    if (Game.snake.body[0].y < 0)  //Если змейка вышла за пределы экрана по вертикали, то продолжает свое движение с нижней доступной точки экрана
                        Game.snake.body[0].y = ConsoleHeight;
                    object lockThis = new object();
                    lock(lockThis)
                    {
                        if (snake.body.Count % 3 == 0)
                        {
                            Console.Clear();
                            wall.Draw();
                            showStats();
                            speed -= 50;
                            Game.numberOfLvl++;
                            Game.countOfEatenFood = 0;
                            snake.respawnSnake();
                            Game.food.setNewPosition();
                            Game.food.Draw();
                        }
                    }
                    
                    snake.Draw();
                    //Thread.Sleep(speed);
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(10, 10);
                    Console.WriteLine("Game Over");
                    Console.ReadKey();
                }
          //  }
            

        }

        public void constantsSetup()
        {
            wall.setLevel(1);
            wall.Draw();
            food.Draw();
            showStats();
        }

        public static void showStats()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(2, 23);
            Console.WriteLine("Level {0}", numberOfLvl);
            Console.SetCursorPosition(2, 24);
            Console.WriteLine("{0} points", points);
        }

        public void Save()
        {
            snake.Save();
            food.Save();
            wall.Save();
        }

        public void Resume()
        {
            snake.Resume();
            food.Resume();
            wall.Resume();
        }

    }
}
