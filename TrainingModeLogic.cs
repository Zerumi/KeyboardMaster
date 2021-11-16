using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace KeyboardMaster
{
    class TrainingModeLogic:ITrainingModeLogic
    {
        static TrainingMode trainingMode;
        ConfigurationRequest ConfigurationRequest = new ConfigurationRequest();
        static float speed = 4;
        public static int counter = 0;
        static int lvlspeedGL;
        public static bool verification = true;
        public static char currentChar;
        public static DispatcherTimer timer = new DispatcherTimer();
        static char[] en_Alpha = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public TrainingModeLogic(TrainingMode tr)
        {
            trainingMode = tr;
        }

        public void Start(int levelspeed)
        {
            timer = new DispatcherTimer();
            var lang = ConfigurationRequest.GetDictonary().LanguageDictonary;
            lvlspeedGL = levelspeed;
            speed /= levelspeed;
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (int)speed*1000);
            App.isTraining = true;
            StartGenerating();
            
        }
        public void StartGenerating()
        {
            trainingMode.movesCount.Content = $"Пройдено шагов: {counter}";
            timer.Stop();
            Random rnd = new Random();
            if (lvlspeedGL != 4)
            {
                switch (counter)
                {
                }
                if (counter < 15)
                {
                    trainingMode.speedUp.Content = $"Повышение скорости через: {15 - counter}";
                }
                else if (counter == 15)
                {
                    trainingMode.speedUp.Content = $"Повышение скорости через: {30 - counter}";
                    lvlspeedGL++;
                    speed = 4 / lvlspeedGL;
                    timer.Tick += timer_tick;
                    timer.Interval = new TimeSpan(0, 0, 0, (int)speed * 1000);
                }
                else if (counter<30)
                {
                    trainingMode.speedUp.Content = $"Повышение скорости через: {30 - counter}";
                }
                else if (counter == 30)
                {
                    trainingMode.speedUp.Content = $"Повышение скорости через: {60 - counter}";
                    lvlspeedGL++;
                    speed = 4 / lvlspeedGL;
                    timer.Tick += timer_tick;
                    timer.Interval = new TimeSpan(0, 0, 0, (int)speed * 1000);
                }
                else if (counter <60)
                {
                    trainingMode.speedUp.Content = $"Повышение скорости через: {60 - counter}";
                }
                else if (counter==60)
                {
                    trainingMode.speedUp.Content = $"Повышение скорости через: Макс";
                    lvlspeedGL++;
                    speed = 4 / lvlspeedGL;
                    timer.Tick += timer_tick;
                    timer.Interval = new TimeSpan(0, 0, 0, (int)speed * 1000);
                }
            }
            else
            {
                trainingMode.speedUp.Content = $"Повышение скорости через: Макс";
            }
            trainingMode.speed.Content = $"Текущая скорость: {lvlspeedGL}";

            currentChar = en_Alpha[rnd.Next(0, 24)];
            trainingMode.charLb.Content = currentChar.ToString();
            timer.Start();
        }
        public void timer_tick(object sender, EventArgs e)
        {
            App.isTraining = false;
            trainingMode.rect.Fill = Brushes.Red;
            timer.Stop();
            MessageBox.Show($"Тренировка окончена. Ваш результат: {counter}");
            counter = 0;
            trainingMode.rect.Fill = null;
            trainingMode.startBut.IsEnabled = true;
            speed = 4;
        }
        
    }
}
