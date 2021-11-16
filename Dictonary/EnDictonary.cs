using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    class EnDictonary : IDictonary
    {
        public string[] Words { get; set; }
        public int AverageLettersInWords => 5;

        public EnDictonary()
        {
            Words = new string[] { "word", "school", "program", "class", "object", "pc", "house", "music", "brain", "work", "woman", "man", "dish", "girl", "curtain", "lamp", "jar", "flower", "time", "case", "life", "day", "night", "hand", "place", "face", "friend", "event", "world", "system", "view", "money", "piano", "guitar", "concert", "ladder", "snow", "authumn", "winter", "spring", "summer", "grass", "weather", "sun", "star", "ground", "question", "part", "solution", "game", "intellegence", "assembly", "phase", "feature", "glass", "packet", "shop", "cleaning", "product", "sale", "union", "president", "association", "party", "politics", "tv", "technique", "letter", "art", "keyboard", "профессионал", "poppy", "painter", "brush" };
        }

    }
}
