using System;
using System.Drawing;
using System.Windows.Forms;
using Verification.help_form;

namespace Verification
{
    public partial class Helper : Form
    {
        private DataGridView dgvMistakes;
        private DataGridView dgvMetrics;

        private const int MISTAKE_COL_NUM = 5;
        private const int METRIC_COL_NUM = 5;
        private const int Panel1MaxWidth = 220;

        public Helper()
        {
            InitializeComponent();
            InitializeTreeView();
            splitContainer1.SplitterDistance = Panel1MaxWidth;

            createTables();
        }
        private void createTables()
        {
            dgvMistakes = new DataGridView();
            splitContainer1.Panel2.Controls.Add(dgvMistakes);
            dgvMistakes.Parent = splitContainer1.Panel2;
            dgvMistakes.Dock = DockStyle.Fill;
            dgvMistakes.ReadOnly = true;
            dgvMistakes.Visible = false;
            customizeTableMistaces();

            dgvMetrics = new DataGridView();
            splitContainer1.Panel2.Controls.Add(dgvMetrics);
            dgvMetrics.Parent = splitContainer1.Panel2;
            dgvMetrics.Dock = DockStyle.Fill;
            dgvMetrics.ReadOnly = true;
            dgvMetrics.Visible = false;
            customizeTableMetrics();
        }
        private void customizeTableMistaces()
        {
            dgvMistakes.ColumnHeadersDefaultCellStyle.Font =
                new Font(dgvMistakes.Font.FontFamily, 7f, FontStyle.Bold);


            dgvMistakes.ColumnCount = MISTAKE_COL_NUM;
            dgvMistakes.Columns[0].Name = "Номер";
            dgvMistakes.Columns[1].Name = "Серьезность";
            dgvMistakes.Columns[2].Name = "Ошибка";
            dgvMistakes.Columns[3].Name = "Описание";
            dgvMistakes.Columns[4].Name = "Этап";

            dgvMistakes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvMistakes.Font = new Font(dgvMistakes.Font.FontFamily, 7.5f);


            dgvMistakes.Columns[0].Width = 40;
            dgvMistakes.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMistakes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMistakes.Columns[MISTAKE_COL_NUM - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


        }
        private void customizeTableMetrics()
        {
            dgvMetrics.ColumnHeadersDefaultCellStyle.Font =
                new Font(dgvMetrics.Font.FontFamily, 7f, FontStyle.Bold);


            dgvMetrics.ColumnCount = METRIC_COL_NUM;
            dgvMetrics.Columns[0].Name = "Номер";
            dgvMetrics.Columns[1].Name = "Наименование";
            dgvMetrics.Columns[2].Name = "Расшифровка аббревиатуры";
            dgvMetrics.Columns[3].Name = "Обозначение";
            dgvMetrics.Columns[4].Name = "Единица измерения";

            dgvMetrics.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvMetrics.Font = new Font(dgvMetrics.Font.FontFamily, 7.5f);


            dgvMetrics.Columns[0].Width = 40;
            dgvMetrics.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMetrics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMetrics.Columns[METRIC_COL_NUM - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


        }
        private void InitializeTreeView()
        {
            treevHelp.BeginUpdate();
            treevHelp.Nodes.Add("Общие сведения");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.GENERAL;

            treevHelp.Nodes.Add("Работа с программой");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.PROGRAM;

            //treevHelp.Nodes[1].Nodes.Add("Верификация одной диаграммы");
            //treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[0].Tag = TreeTags.VER1;

            //treevHelp.Nodes[1].Nodes.Add("Верификация нескольких диаграммы");
            //treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[1].Tag = TreeTags.VER2;

            //treevHelp.Nodes[1].Nodes.Add("Верификация пакета");
            //treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[2].Tag = TreeTags.VER_PACK;

            treevHelp.Nodes.Add("Выводимые ошибки");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.MISTAKES;
            treevHelp.Nodes[2].Nodes.Add("Ошибки UCD");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[0].Tag = TreeTags.UCD_MISTAKES;

            treevHelp.Nodes[2].Nodes.Add("Ошибки AD");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[1].Tag = TreeTags.AD_MISTAKES;

            treevHelp.Nodes[2].Nodes.Add("Ошибки CD");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[2].Tag = TreeTags.CD_MISTAKES;
            treevHelp.EndUpdate();

            treevHelp.Nodes.Add("Выводимые метрики");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.METRICS;
            treevHelp.Nodes[3].Nodes.Add("Метрики UCD");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[0].Tag = TreeTags.UCD_METRICS;

            treevHelp.Nodes[3].Nodes.Add("Метрики AD");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[1].Tag = TreeTags.AD_METRICS;

            treevHelp.Nodes[3].Nodes.Add("Метрики CD");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[2].Tag = TreeTags.CD_METRICS;
        }

        private void changeVisible(int vis)
        {
            if (vis == 0)
            {
                rbHelp.Visible = true;
                dgvMistakes.Visible = false;
                dgvMetrics.Visible = false;
            } else if (vis == 1)
            {
                rbHelp.Visible = false;
                dgvMistakes.Visible = true;
                dgvMetrics.Visible = false;
            } else
            {
                rbHelp.Visible = false;
                dgvMistakes.Visible = false;
                dgvMetrics.Visible = true;
            }
        }
        private void fillTable(string resources, DataGridView dgv, int colNum)
        {
            dgv.Rows.Clear();
            var mistakesLst = resources.Split('|');
            // для всех строк
            for (int i = 0; i < mistakesLst.Length; i += colNum)
            {
                dgv.Rows.Add(new object[] { mistakesLst[i], mistakesLst[i+1],
                    mistakesLst[i+2], mistakesLst[i+3], mistakesLst[i+4] });
            }
        }

        private void showInfo()
        {
            //1 | СЕРЬЕЗНОСТЬ | ОШИБКА | ОПИСАНИЕ | ПРИЧИНЫ ОШИБКИ | ЭТАП ПРОВЕРКИ 
            switch ((TreeTags)treevHelp.SelectedNode.Tag)
            {
                case TreeTags.GENERAL:
                    changeVisible(0);
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                    break;
                case TreeTags.PROGRAM:
                    changeVisible(0);
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpProgram + "}";
                    break;
                //case TreeTags.VER1:
                //    changeVisible(true);
                //    rbHelp.Clear();
                //    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                //    break;
                //case TreeTags.VER2:
                //    changeVisible(true);
                //    rbHelp.Clear();
                //    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                //    break;
                //case TreeTags.VER_PACK:
                //    changeVisible(true);
                //    rbHelp.Clear();
                //    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                //    break;
                case TreeTags.AD_MISTAKES:
                    changeVisible(1);
                    fillTable(Properties.Resources.ADMistakes, dgvMistakes, MISTAKE_COL_NUM);
                    break;
                case TreeTags.CD_MISTAKES:
                    changeVisible(1);
                    fillTable(Properties.Resources.CDMistakes, dgvMistakes, MISTAKE_COL_NUM);
                    break;
                case TreeTags.UCD_MISTAKES:
                    changeVisible(1);
                    fillTable(Properties.Resources.UCDMistakes, dgvMistakes, MISTAKE_COL_NUM);
                    break;
                case TreeTags.AD_METRICS:
                    changeVisible(2);
                    fillTable(Properties.Resources.UCDMetrics, dgvMetrics, METRIC_COL_NUM);
                    break;
                case TreeTags.CD_METRICS:
                    changeVisible(2);
                    fillTable(Properties.Resources.UCDMetrics, dgvMetrics, METRIC_COL_NUM);
                    break;
                case TreeTags.UCD_METRICS:
                    changeVisible(2);
                    fillTable(Properties.Resources.UCDMetrics, dgvMetrics, METRIC_COL_NUM);
                    break;
                default:
                    break;
            }
        }

        private void treevHelp_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treevHelp.SelectedNode == null) return;
            if (treevHelp.SelectedNode.Tag == null) return;
            showInfo();
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1.Width > Panel1MaxWidth)
            {
                splitContainer1.SplitterDistance = Panel1MaxWidth;
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel1.Width > Panel1MaxWidth)
            {
                splitContainer1.SplitterDistance = Panel1MaxWidth;
            }
        }
    }
}
