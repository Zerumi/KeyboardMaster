using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace KeyboardMaster
{
    /// <summary>
    /// Логика взаимодействия для PerfomanceRanking.xaml
    /// </summary>
    public partial class PerfomanceRanking : Window
    {
        readonly DispatcherTimer timer = new();

        public PerfomanceRanking()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMinutes(1);
            SetupScores();
            Network.OnScoresUpdate += SetupScores;
            Network.OnScoreAdd += Scores_OnAdd;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            List<Score> scores = Network.scores;

            trgNames.Rows.Clear();
            TableRow EmptyRow = new();
            TableCell EmptyCell = new();
            Paragraph EmptyPar = new();
            EmptyPar.Inlines.Add(string.Empty);
            EmptyCell.Blocks.Add(EmptyPar);
            EmptyRow.Cells.Add(EmptyCell);
            trgNames.Rows.Add(EmptyRow);
            foreach (Score item in scores)
            {
                TableRow NametableRow = new();
                TableCell NametableCell = new();
                Paragraph Nameparagraph = new();
                Nameparagraph.Inlines.Add($"{item.Name} ({m3md2.Parser.RelativeTime(item.Timestamp)})");
                NametableCell.Blocks.Add(Nameparagraph);
                NametableRow.Cells.Add(NametableCell);
                trgNames.Rows.Add(NametableRow);
            }
        }

        private void SetupScores()
        {
            List<Score> scores = Network.scores;

            #region SetupNames
            foreach (Score item in scores)
            {
                TableRow NametableRow = new();
                TableCell NametableCell = new();
                Paragraph Nameparagraph = new();
                Nameparagraph.Inlines.Add($"{item.Name} ({m3md2.Parser.RelativeTime(item.Timestamp)})");
                NametableCell.Blocks.Add(Nameparagraph);
                NametableRow.Cells.Add(NametableCell);
                trgNames.Rows.Add(NametableRow);
            }
            timer.Start();
            #endregion

            #region SetupTextPerfomance
            foreach (Score item in Network.scores)
            {
                TableRow TPtableRow = new();

                TableCell TPCCtableCell = new();
                Paragraph TPCCparagraph = new();
                TPCCparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.CorrectChars));
                TPCCtableCell.Blocks.Add(TPCCparagraph);

                TableCell TPInCtableCell = new();
                Paragraph TPInCparagraph = new();
                TPInCparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.IncorrectChars));
                TPInCtableCell.Blocks.Add(TPInCparagraph);

                TableCell TPAcctableCell = new();
                Paragraph TPAccparagraph = new();
                TPAccparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.Accuracy));
                TPAcctableCell.Blocks.Add(TPAccparagraph);

                TableCell TPIWtableCell = new();
                Paragraph TPIWparagraph = new();
                TPIWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.IdealWords));
                TPIWtableCell.Blocks.Add(TPIWparagraph);

                TableCell TPEWtableCell = new();
                Paragraph TPEWparagraph = new();
                TPEWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.ErrorWords));
                TPEWtableCell.Blocks.Add(TPEWparagraph);

                TableCell TPWWtableCell = new();
                Paragraph TPWWparagraph = new();
                TPWWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.WrongWords));
                TPWWtableCell.Blocks.Add(TPWWparagraph);

                TableCell TPWPMtableCell = new();
                Paragraph TPWPMparagraph = new();
                TPWPMparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.WordsPerMinute));
                TPWPMtableCell.Blocks.Add(TPWPMparagraph);

                TableCell TPAvMtableCell = new();
                Paragraph TPAvMparagraph = new();
                TPAvMparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.AverageWPM));
                TPAvMtableCell.Blocks.Add(TPAvMparagraph);

                TableCell TPSiWtableCell = new();
                Paragraph TPSiWparagraph = new();
                TPSiWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.StreakIdealWords));
                TPSiWtableCell.Blocks.Add(TPSiWparagraph);

                TableCell TPWActableCell = new();
                Paragraph TPWAcparagraph = new();
                TPWAcparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.WordAccuracy));
                TPWActableCell.Blocks.Add(TPWAcparagraph);

                TableCell TPTpptableCell = new();
                Paragraph TPTppparagraph = new();
                TPTppparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.TextPerfomancePoints));
                TPTpptableCell.Blocks.Add(TPTppparagraph);

                TPtableRow.Cells.Add(TPCCtableCell);
                TPtableRow.Cells.Add(TPInCtableCell);
                TPtableRow.Cells.Add(TPAcctableCell);
                TPtableRow.Cells.Add(TPIWtableCell);
                TPtableRow.Cells.Add(TPEWtableCell);
                TPtableRow.Cells.Add(TPWWtableCell);
                TPtableRow.Cells.Add(TPWPMtableCell);
                TPtableRow.Cells.Add(TPAvMtableCell);
                TPtableRow.Cells.Add(TPSiWtableCell);
                TPtableRow.Cells.Add(TPWActableCell);
                TPtableRow.Cells.Add(TPTpptableCell);

                trgTextPerfomance.Rows.Add(TPtableRow);
            }
            #endregion

            #region SetupCorePerfomance
            foreach (Score item in Network.scores)
            {
                TableRow CPtableRow = new();

                TableCell CPCPMtableCell = new();
                Paragraph CPCPMparagraph = new();
                CPCPMparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.CharsPerMinute));
                CPCPMtableCell.Blocks.Add(CPCPMparagraph);

                TableCell CPBcMtableCell = new();
                Paragraph CPBcMparagraph = new();
                CPBcMparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.BestCPM));
                CPBcMtableCell.Blocks.Add(CPBcMparagraph);

                TableCell CPAcMtableCell = new();
                Paragraph CPAcMparagraph = new();
                CPAcMparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.AverageCPM));
                CPAcMtableCell.Blocks.Add(CPAcMparagraph);

                TableCell CPBlttableCell = new();
                Paragraph CPBltparagraph = new();
                CPBltparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.BestLatency));
                CPBlttableCell.Blocks.Add(CPBltparagraph);

                TableCell CPLattableCell = new();
                Paragraph CPLatparagraph = new();
                CPLatparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.Latency));
                CPLattableCell.Blocks.Add(CPLatparagraph);

                TableCell CPAdltableCell = new();
                Paragraph CPAdlparagraph = new();
                CPAdlparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.AverageDelay));
                CPAdltableCell.Blocks.Add(CPAdlparagraph);

                TableCell CPPuFtableCell = new();
                Paragraph CPPuFparagraph = new();
                CPPuFparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.printingUniformity));
                CPPuFtableCell.Blocks.Add(CPPuFparagraph);

                TableCell CPCpptableCell = new();
                Paragraph CPCppparagraph = new();
                CPCppparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.CorePerfomancePoints));
                CPCpptableCell.Blocks.Add(CPCppparagraph);

                CPtableRow.Cells.Add(CPCPMtableCell);
                CPtableRow.Cells.Add(CPBcMtableCell);
                CPtableRow.Cells.Add(CPAcMtableCell);
                CPtableRow.Cells.Add(CPBlttableCell);
                CPtableRow.Cells.Add(CPLattableCell);
                CPtableRow.Cells.Add(CPAdltableCell);
                CPtableRow.Cells.Add(CPPuFtableCell);
                CPtableRow.Cells.Add(CPCpptableCell);

                trgCorePerfomance.Rows.Add(CPtableRow);
            }
#endregion
        }

        private void Scores_OnAdd(Score item)
        {
            TableRow NametableRow = new();
            TableCell NametableCell = new();
            Paragraph Nameparagraph = new();
            Nameparagraph.Inlines.Add($"{item.Name} ({m3md2.Parser.RelativeTime(item.Timestamp)})");
            NametableCell.Blocks.Add(Nameparagraph);
            NametableRow.Cells.Add(NametableCell);
            trgNames.Rows.Add(NametableRow);

            TableRow TPtableRow = new();

            TableCell TPCCtableCell = new();
            Paragraph TPCCparagraph = new();
            TPCCparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.CorrectChars));
            TPCCtableCell.Blocks.Add(TPCCparagraph);

            TableCell TPInCtableCell = new();
            Paragraph TPInCparagraph = new();
            TPInCparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.IncorrectChars));
            TPInCtableCell.Blocks.Add(TPInCparagraph);

            TableCell TPAcctableCell = new();
            Paragraph TPAccparagraph = new();
            TPAccparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.Accuracy));
            TPAcctableCell.Blocks.Add(TPAccparagraph);

            TableCell TPIWtableCell = new();
            Paragraph TPIWparagraph = new();
            TPIWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.IdealWords));
            TPIWtableCell.Blocks.Add(TPIWparagraph);

            TableCell TPEWtableCell = new();
            Paragraph TPEWparagraph = new();
            TPEWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.ErrorWords));
            TPEWtableCell.Blocks.Add(TPEWparagraph);

            TableCell TPWWtableCell = new();
            Paragraph TPWWparagraph = new();
            TPWWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.WrongWords));
            TPWWtableCell.Blocks.Add(TPWWparagraph);

            TableCell TPWPMtableCell = new();
            Paragraph TPWPMparagraph = new();
            TPWPMparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.WordsPerMinute));
            TPWPMtableCell.Blocks.Add(TPWPMparagraph);

            TableCell TPAvMtableCell = new();
            Paragraph TPAvMparagraph = new();
            TPAvMparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.AverageWPM));
            TPAvMtableCell.Blocks.Add(TPAvMparagraph);

            TableCell TPSiWtableCell = new();
            Paragraph TPSiWparagraph = new();
            TPSiWparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.StreakIdealWords));
            TPSiWtableCell.Blocks.Add(TPSiWparagraph);

            TableCell TPWActableCell = new();
            Paragraph TPWAcparagraph = new();
            TPWAcparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.WordAccuracy));
            TPWActableCell.Blocks.Add(TPWAcparagraph);

            TableCell TPTpptableCell = new();
            Paragraph TPTppparagraph = new();
            TPTppparagraph.Inlines.Add(Convert.ToString(item.textPerfomance.TextPerfomancePoints));
            TPTpptableCell.Blocks.Add(TPTppparagraph);

            TPtableRow.Cells.Add(TPCCtableCell);
            TPtableRow.Cells.Add(TPInCtableCell);
            TPtableRow.Cells.Add(TPAcctableCell);
            TPtableRow.Cells.Add(TPIWtableCell);
            TPtableRow.Cells.Add(TPEWtableCell);
            TPtableRow.Cells.Add(TPWWtableCell);
            TPtableRow.Cells.Add(TPWPMtableCell);
            TPtableRow.Cells.Add(TPAvMtableCell);
            TPtableRow.Cells.Add(TPSiWtableCell);
            TPtableRow.Cells.Add(TPWActableCell);
            TPtableRow.Cells.Add(TPTpptableCell);

            trgTextPerfomance.Rows.Add(TPtableRow);

            TableRow CPtableRow = new();

            TableCell CPCPMtableCell = new();
            Paragraph CPCPMparagraph = new();
            CPCPMparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.CharsPerMinute));
            CPCPMtableCell.Blocks.Add(CPCPMparagraph);

            TableCell CPBcMtableCell = new();
            Paragraph CPBcMparagraph = new();
            CPBcMparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.BestCPM));
            CPBcMtableCell.Blocks.Add(CPBcMparagraph);

            TableCell CPAcMtableCell = new();
            Paragraph CPAcMparagraph = new();
            CPAcMparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.AverageCPM));
            CPAcMtableCell.Blocks.Add(CPAcMparagraph);

            TableCell CPBlttableCell = new();
            Paragraph CPBltparagraph = new();
            CPBltparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.BestLatency));
            CPBlttableCell.Blocks.Add(CPBltparagraph);

            TableCell CPLattableCell = new();
            Paragraph CPLatparagraph = new();
            CPLatparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.Latency));
            CPLattableCell.Blocks.Add(CPLatparagraph);

            TableCell CPAdltableCell = new();
            Paragraph CPAdlparagraph = new();
            CPAdlparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.AverageDelay));
            CPAdltableCell.Blocks.Add(CPAdlparagraph);

            TableCell CPPuFtableCell = new();
            Paragraph CPPuFparagraph = new();
            CPPuFparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.printingUniformity));
            CPPuFtableCell.Blocks.Add(CPPuFparagraph);

            TableCell CPCpptableCell = new();
            Paragraph CPCppparagraph = new();
            CPCppparagraph.Inlines.Add(Convert.ToString(item.corePerfomance.CorePerfomancePoints));
            CPCpptableCell.Blocks.Add(CPCppparagraph);

            CPtableRow.Cells.Add(CPCPMtableCell);
            CPtableRow.Cells.Add(CPBcMtableCell);
            CPtableRow.Cells.Add(CPAcMtableCell);
            CPtableRow.Cells.Add(CPBlttableCell);
            CPtableRow.Cells.Add(CPLattableCell);
            CPtableRow.Cells.Add(CPAdltableCell);
            CPtableRow.Cells.Add(CPPuFtableCell);
            CPtableRow.Cells.Add(CPCpptableCell);

            trgCorePerfomance.Rows.Add(CPtableRow);
        }

        private void TpInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В таблице TextPerfomance 11 значений, вот их список:\nCC (CorrectChars) - Правильно напечатанные символы\nInC (IncorrectChars) - Неправильно напечатанные символы\nAcc (Accuracy) - Аккуратность\nIW (IdealWords) - идеальные слова\nEW (ErrorWords) - слова, во время написания которых была допущена опечатка (которая была исправлена позднее)\nWW (WrongWords) - завершенные слова с опечатками\nWPM (Words Per Minute) - количество слов в минуту\nAvM (Average WPM) - Среднее кол-во слов в минуту\nSiW (Streak of Ideal Words) - Максимальная серия идеально написанных слов\nWAc (Word Accuracy) - Аккуратность написания слов\nTpp (TextPerfomancePoints) - Показатель производительности TextPerfomance");
        }

        private void CpInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В таблице CorePerfomance 8 значений, вот их список:\nCPM (Chars Per Minute) - кол-во знаков в минуту\nBcM (Best CPM) - лучший показатель символов в минуту\nAcM (Average CPM) - средний показатель символов в минуту\nBLt (Best Latency) - наименьшая задержка печати\nLat (Latency) - задержка печати\nAdL (Average Delay) - средняя задержка печати\nPuF (Print Uniformity) - Равномерность печати\nCpp (CorePerfomancePoints) - Показатель производительности CorePerfomance");
        }
    }
}
