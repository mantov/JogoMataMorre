using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JogoMataMorre.Business;
using JogoMataMorre.Business.Entities;

namespace JogoMataMorre
{
    class Program
    {
        static void Main(string[] args)
        {
            var logBO = new LogBO();
            var listMatches = logBO.LoadGameLog();

            PrintScoreBoard(listMatches);
        }

        private static void PrintScoreBoard(List<Match> listMatches)
        {
            foreach (var match in listMatches)
            {
                Console.WriteLine(string.Format("Game number {0}", match.Number));
                Console.WriteLine("Ranking:");

                int position = 0;
                foreach (var player in match.Players)
                {
                    position++;
                    Console.Write(string.Format("{0} - {1} ({2}/{3})", position, player.Name, player.Kills.Count,
                        player.Deads.Count));
                    foreach (var award in player.Awards)
                    {
                        Console.Write(string.Format(" - {0}", award));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
