using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame
{
    public class GameRecord
    {
        public string time { get; set; }
        public string typeOfGame { get; set; }
        public int score { get; set; }

        public GameRecord(string time, string typeOfGame, int score)
        {
            this.time = time;
            this.typeOfGame = typeOfGame;
            this.score = score;
        }
    }
}
