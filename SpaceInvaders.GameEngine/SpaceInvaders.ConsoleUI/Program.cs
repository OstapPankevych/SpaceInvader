using System;
using SpaceInvaders.GameEngine;
using SpaceInvaders.GameEngine.Logic;


namespace SpaceInvaders.ConsoleUI
{
    class Program
    {
        // Ostap: добавив клас ProcessingCommand для логічного групування команд з клавіатури
        static void Main(string[] args)
        {
            IDistanceStrategy d = new DistanceStrategy();
            Process game = new Process(d);
            ConsoleDraw draw = new ConsoleDraw();
            game.Draw += draw.Render;
            game.Show += draw.Show;
            game.Clear += Console.Clear;
                      
            draw.StartScreen();
            game.Init(60, 50, 7, 5);
            game.InputKey += ProcessingCommand.PressKey;

            while (true)
            {
                if(game.IsExit)
                {
                    game.Draw -= draw.Render;
                    game.Show -= draw.Show;
                    game.Clear -= Console.Clear;
                    game.InputKey -= ProcessingCommand.PressKey;

                    if (game.Win)
                    {                      
                        draw.GameOverScreen("Congratulation! You are the Winner!", game.Score);                      
                    }
                    else
                    {
                        draw.GameOverScreen("Thanks for playing.", game.Score);                      
                    }
                }
            }
        }
    }
}

