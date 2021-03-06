using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using m3md2;

namespace KeyboardMaster
{
    class RuDictonary : IDictonary
    {
        public string[] Words { get; set; }
        public int AverageLettersInWords => 5;
        public LanguageDictonary LanguageDictonary => LanguageDictonary.RU;

        public RuDictonary()
        {
            Words = new string[] { "слово", "школа", "программа", "класс", "объект", "пк", "дом", "музыка", "мозг", "работа", "женщина", "мужчина", "тарелка", "девушка", "штора", "лампа", "банка", "цветок", "время", "дело", "жизнь", "день", "ночь", "рука", "место", "лицо", "друг", "случай", "мир", "система", "вид", "деньги", "пианино", "гитара", "концерт", "лестница", "снег", "осень", "зима", "весна", "лето", "трава", "погода", "солнце", "звезда", "земля", "вопрос", "часть", "решение", "игра", "интеллект", "сборка", "фаза", "особенность", "стекло", "пакет", "магазин", "уборка", "товар", "скидка", "союз", "президент", "объединение", "партия", "политика", "телевизор", "техника", "письмо", "искусство", "клавиатура", "профессионал", "мак", "маляр", "кисть" };
        }

    }
}
