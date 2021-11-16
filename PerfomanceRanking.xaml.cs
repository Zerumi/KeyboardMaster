using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyboardMaster
{
    /// <summary>
    /// Логика взаимодействия для PerfomanceRanking.xaml
    /// </summary>
    public partial class PerfomanceRanking : Window
    {
        public PerfomanceRanking()
        {
            InitializeComponent();
            SetupScores();
            Network.OnScoresUpdate += SetupScores;
            Network.OnScoreAdd += Scores_OnAdd;
        }

        private void SetupScores()
        {
            #region SetupNames
            List<Score> scores = Network.scores;

            FlowDocument usernames = new();
            Table tNames = new();
            TableRowGroup NametableRowGroup = new();

            TableRow EmptyRow = new();
            TableCell EmptyCell = new();
            Paragraph EmptyPar = new();
            EmptyPar.Inlines.Add(string.Empty);
            EmptyCell.Blocks.Add(EmptyPar);
            EmptyRow.Cells.Add(EmptyCell);
            NametableRowGroup.Rows.Add(EmptyRow);

            foreach (Score item in scores)
            {
                TableRow NametableRow = new();
                TableCell NametableCell = new();
                Paragraph Nameparagraph = new();
                Nameparagraph.Inlines.Add($"{item.Name} ({m3md2.Parser.RelativeTime(item.Timestamp)})");
                NametableCell.Blocks.Add(Nameparagraph);
                NametableRow.Cells.Add(NametableCell);
                NametableRowGroup.Rows.Add(NametableRow);
            }

            tNames.RowGroups.Add(NametableRowGroup);
            usernames.Blocks.Add(tNames);
            rtbUsernames.Document = usernames;
            #endregion

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
        }

        private void Scores_OnAdd(Score score)
        {
            throw new NotImplementedException();
        }
    }
}
