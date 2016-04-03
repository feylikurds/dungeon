﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Canvas
    {
        private readonly int worldHeight;
        private readonly int worldWidth;
        private readonly int menuWidth;
        private readonly int menuHeight;
        private Cell[,] screen;
        private readonly int capacity;

        public Canvas(int worldHeight, int worldWidth, int menuWidth, int menuHeight, Cell[,] screen, int capacity)
        {
            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;
            this.menuWidth = menuWidth;
            this.menuHeight = menuHeight;
            this.screen = screen;
            this.capacity = capacity;
        }

        public void Draw(Room[,] rooms, Player player, ConcurrentQueue<Pixel> cq)
        {
            for (var y = 0; y < worldHeight; y++)
                for (var x = 0; x < worldWidth; x++)
                {
                    screen[x, y] = new Cell(capacity);
                    screen[x, y].Pixels = rooms[x, y].Draw();
                }

            var col = worldWidth + menuWidth - 1;

            entry(screen, col, 0, "Health", player.Health, ConsoleColor.Yellow);
            entry(screen, col, 1, "Strength", player.Strength, ConsoleColor.Yellow);
            entry(screen, col, 2, "--------------", ConsoleColor.Yellow);

            var row = 3;

            foreach (var pixel in cq.Reverse())
                entry(screen, col, row++, pixel.Symbol, pixel.Color);

            entry(screen, col, worldHeight - 4, "Commands", ConsoleColor.Yellow);
            entry(screen, col, worldHeight - 3, "--------------", ConsoleColor.Yellow);
            entry(screen, col, worldHeight - 2, "q: Quit", ConsoleColor.Yellow);
            entry(screen, col, worldHeight - 1, "arrows: Move", ConsoleColor.Yellow);

            entry(screen, 0, worldHeight + 1, "P: Player", ConsoleColor.Yellow);
            entry(screen, 5, worldHeight + 1, "    ", ConsoleColor.Yellow);
            entry(screen, 20, worldHeight + 1, "H: Health", ConsoleColor.Green);
            entry(screen, 0, worldHeight + 2, "M: Monster", ConsoleColor.Red);
            entry(screen, 5, worldHeight + 2, "   ", ConsoleColor.Red);
            entry(screen, 20, worldHeight + 2, "S: Strength", ConsoleColor.Cyan);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Dungeons by Aryo Pehlewan aryopehlewan@hotmail.com Copyright 2016 Licensed GPLv3");
            Console.ResetColor();

            for (var y = 0; y < worldHeight + menuHeight; y++)
            {
                for (var x = 0; x < worldWidth + menuWidth; x++)
                {
                    var pixels = screen[x, y]?.Pixels;

                    if (pixels == null)
                        continue;

                    for (var i = 0; i < pixels.Length; i++)
                    {
                        Console.ForegroundColor = pixels[i].Color;
                        Console.Write(pixels[i].Symbol);
                        Console.ResetColor();
                    }
                }

                Console.Write(Environment.NewLine);
            }

        }

        private void entry(Cell[,] screen, int x, int y, string name, int value, ConsoleColor color)
        {
            screen[x, y] = new Cell(capacity);

            var text = String.Format("{0, 8}", name) + " = " + String.Format("{0, 3}", value);
            
            screen[x, y].Pixels = status(text, color);
        }

        private void entry(Cell[,] screen, int x, int y, string text, ConsoleColor color)
        {
            screen[x, y] = new Cell(capacity);
            screen[x, y].Pixels = status(text, color);
        }

        private Pixel[] status(string text, ConsoleColor color)
        {
            var cell = new Pixel[]
            {
                new Pixel { Symbol = " " },
                new Pixel { Symbol = text, Color = color }
            };

            return cell;
        }
    }
}
