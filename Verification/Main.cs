using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using Verification.ad_ver;
using Verification.cd_ver;
using Verification.package_ver;
using Verification.type_definer;
using Verification.uc_ver;
using Verification.settings;
using Verification.rating_system;
using Verification.metrics_calc;
using System.Linq;

namespace Verification
{
    public partial class Main : Form
    {
        public static Main MainFormInstance;
        public Distribution Distribution;
        private Helper helperForm;
        private RateSettingsController rateSettings;
        private bool isClearingRows;
        private bool firstSessionCheck = true;

        public Main()
        {
            InitializeComponent();
            MainFormInstance = this;
            Distribution = new Distribution();
            Distribution.NewDiagramAdded += AddDiagram;
            Distribution.SomethingChanged += UpdateDiagramOnGUI;

            helperForm = null;
            rateSettings = new RateSettingsController();
            isClearingRows = false;

            errorsGV.Font = new Font("Microsoft Sans Serif", 10);
            diagramsGV.Font = new Font("Microsoft Sans Serif", 10);
            diagramPicture.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // Выбор файлов
        private void выбратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseFiles();
        }

        // Сохранение результата
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportDiagramMistakes();
        }

        /// <summary>
        /// Проверка согласованности диаграмм
        /// </summary>
        private void checkDiffDiagrams()
        {
            // имя файла = тип_диаграммы.xml (тип_диаграммы = uc\ad\cd)
            if (Distribution.AllDiagrams.Count > 3)
            {
                ShowMsg("Загружено более трех диаграмм", "Exception");
                return;
            }
            Distribution.AllDiagrams.TryGetValue("ucd", out Diagram uc);
            Distribution.AllDiagrams.TryGetValue("ad", out Diagram ad);
            Distribution.AllDiagrams.TryGetValue("cd", out Diagram cd);
            var mistakes = new List<Mistake>();         // список ошибок для вывода

            if (uc != null)
                StartUCDVer(uc);
            if (ad != null)
                StartADVer(ad);
            if (cd != null)
                StartCDVer(cd);

            ConsistencyVerifier.Verify(uc, ad, cd, mistakes);
            if (uc != null)
                uc.Mistakes.AddRange(mistakes);
            if (ad != null)
                ad.Mistakes.AddRange(mistakes);
            if (cd != null)
                cd.Mistakes.AddRange(mistakes);
        }

        // Кнопка "верифицировать"
        private void btVerify_Click(object sender, EventArgs e)
        {
            string waitingFormMsg = "";
            var bw = new BackgroundWorker();
            waitingFormMsg = "Верификация";
            bw.DoWork += (obj, ex) => VerificateAll(ex);

            WaitingForm waitingForm = new WaitingForm();
            waitingForm.InitializationWaitingForm(this, waitingFormMsg);        // инициализация прогресс бара
            waitingForm.show();
            bw.RunWorkerCompleted += waitingForm.bw_RunWorkerCompleted;
            bw.RunWorkerCompleted += (obj, ex) => updateGUI(ex);

            bw.RunWorkerAsync();

        }
        private void updateGUI(RunWorkerCompletedEventArgs e)
        {
            UpdateDiagramOnGUI((List<string>)e.Result);
            WriteLog();
        }

        private void VerificateAll(DoWorkEventArgs args)
        {
            var verificatedNames = new List<string>();
            var diagramsCount = Distribution.AllDiagrams.Count;

            var keys = new string[diagramsCount];
            Distribution.AllDiagrams.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                var curDiagram = Distribution.AllDiagrams[key];
                if (!curDiagram.Verificated)
                {
                    Verificate(curDiagram);
                    verificatedNames.Add(key);
                }
            }
            args.Result = verificatedNames;
        }

        // записать ошибки в файл
        private void WriteLog()
        {

            string filename = @"\verificationResults.txt";
            // Get the current directory.
            string path = Directory.GetCurrentDirectory();
            string target = path + @"\results";
            bool clearFile = true;

            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            // если первая проверка в сессии, то надо почистить существующий файл
            clearFile = firstSessionCheck;
            firstSessionCheck = false;

            foreach (DataGridViewRow row in diagramsGV.Rows)
            {
                var selectedKey = row.Cells[0].Value.ToString();
                var curDiagram = Distribution.AllDiagrams[selectedKey];
                if (curDiagram.Verificated)
                {
                    //TODO(добавить итоговый балл)
                    MistakesPrinter.Print(curDiagram.Mistakes, target + filename, curDiagram.Name, clearFile);
                    clearFile = false;
                }
            }


        }
        private void Verificate(Diagram diagram)
        {
            var type = diagram.EType;
            diagram.Mistakes.Clear();

            switch (type)
            {
                case EDiagramTypes.AD:
                    {
                        StartADVer(diagram);
                        break;
                    }
                case EDiagramTypes.UCD:
                    {
                        StartUCDVer(diagram);
                        break;
                    }
                case EDiagramTypes.CD:
                    {
                        StartCDVer(diagram);
                        break;
                    }
                case EDiagramTypes.UNDEF:
                    {
                        MessageBox.Show("Тип диаграммы \"" + diagram.Name + "\" не определен", "Верификация диаграмм UML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
            }
        }

        private void ShowMsg(string msg, string title)
        {
            MessageBox.Show(
                msg,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void StartADVer(Diagram diagram)
        {
            ADVerifier.Verify(diagram);
            diagram.Verificated = true;
            Distribution.AllDiagrams[diagram.Name] = diagram;
        }

        private void StartUCDVer(Diagram diagram)
        {
            var verificatorUC = new VerificatorUC(diagram);
            verificatorUC.Verificate();
            diagram.Verificated = true;
            Distribution.AllDiagrams[diagram.Name] = diagram;
        }

        private void StartCDVer(Diagram diagram)
        {
            var verificatorDC = new CDVerificator(diagram);
            verificatorDC.Verify();
            diagram.Verificated = true;
            Distribution.AllDiagrams[diagram.Name] = diagram;
        }

        // Кнопка "добавить" диаграмму
        private void btAdd_Click(object sender, EventArgs e)
        {
            ChooseFiles();
        }

        // Кнопка "удалить" диаграмму
        private void btDelete_Click(object sender, EventArgs e)
        {
            if (diagramsGV.SelectedCells.Count == 0)
            {
                MessageBox.Show($"Необходимо выбрать диаграмму", "Верификация диаграмм UML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var curRow = diagramsGV.CurrentRow;
            var selectedName = curRow.Cells[0].Value.ToString();
            Distribution.AllDiagrams.Remove(selectedName);
            diagramsGV.Rows.RemoveAt(curRow.Index);
            diagramsGB.Text = $"Диаграммы ({diagramsGV.Rows.Count})";

            if (diagramsGV.Rows.Count == 0)
            {
                diagramsGB.Text = "Диаграммы";
                errorsGB.Text = "Ошибки";
                btVerify.Enabled = btDelete.Enabled = btOutput.Enabled = false;
            }
        }

        // Обновление выделенной диаграммы
        private void diagramsGV_SelectionChanged(object sender, EventArgs e)
        {
            var name = diagramsGV.Rows.Count == 0 ? null : new List<string>() { diagramsGV.CurrentCell.Value.ToString() };
            UpdateDiagramOnGUI(name);
        }

        // Обновление выделенной ошибки
        private void errorsGV_SelectionChanged(object sender, EventArgs e)
        {
            if (isClearingRows)
                return;

            UpdateMistakesOnGUI();
        }

        // При закрытии формы
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dialogResult = MessageBox.Show("Вы уверены, что хотите выйти?", "Верификация диаграмм UML",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = dialogResult != DialogResult.Yes;
        }

        private void btOutput_Click(object sender, EventArgs e)
        {
            ExportDiagramMistakes();
        }
        private void ExportDiagramMistakes()
        {
            if (diagramsGV == null || diagramsGV.CurrentCell == null || diagramsGV.CurrentCell.Value == null)
            {
                ShowMsg("Выберите диаграмму", "Экспорт ошибок");
                return;
            }
            var selectedKey = diagramsGV.CurrentCell.Value.ToString();
            var curDiagram = Distribution.AllDiagrams[selectedKey];

            if (curDiagram.Verificated)
            {
                var saveDialog = new SaveFileDialog
                {
                    Title = "Сохранение списка ошибок",
                    FileName = "Mistakes.txt",
                    Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*"
                };
                if (saveDialog.ShowDialog() == DialogResult.OK)
                    MistakesPrinter.Print(curDiagram.Mistakes, saveDialog.FileName);
            }
            else
            {
                var result = MessageBox.Show(
                    "Диаграмма не прошла верификацию.\nВерифицировать?",
                    "Верификация диаграмм UML",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    Verificate(curDiagram);
            }
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create or focus help form 
            if (helperForm != null && !helperForm.Disposing && helperForm.Text != "")
            {
                helperForm.Focus();
            }
            else
            {
                helperForm = new Helper();
                helperForm.Show();
            }

        }

        private void menuRate_Click(object sender, EventArgs e)
        {

            if (diagramsGV == null || diagramsGV.CurrentCell == null || diagramsGV.CurrentCell.Value == null)
            {
                ShowMsg("Выберите диаграмму", "Оценивание");
                return;
            }
            var selectedKey = diagramsGV.CurrentCell.Value.ToString();
            var curDiagram = Distribution.AllDiagrams[selectedKey];
            if (!curDiagram.Verificated)
            {
                ShowMsg("Диаграмма не прошла верификацию", "Верификация диаграмм UML");
                return;
            }
            var grade = RateDefiner.defineGrade(curDiagram, rateSettings.Max, rateSettings.Min);
            ShowMsg("Рекомендуемая оценка\n" + (grade.Item2 == "" ? grade.Item1.ToString() : grade.Item2), "Оценка");

        }

        private void баллыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rateSettings.createView();
        }

        MistakesSettingsController mController = new MistakesSettingsController();
        private void mistakesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mController.createView();
        }

        private void метрикиUCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calc(EDiagramTypes.UCD);
        }
        private void Calc(EDiagramTypes selType)
        {
            if (diagramsGV.CurrentCell == null)
            {
                MessageBox.Show("Выберите диаграмму, для которой необходимо вычислить метрики", "Ошибка при выборе опции");
                return;
            }

            string diagrammName = diagramsGV.CurrentCell.Value.ToString();
            Diagram currentDiagramm = null;
            if (Distribution.AllDiagrams.ContainsKey(diagrammName))
                currentDiagramm = Distribution.AllDiagrams[diagrammName];
            else
                throw new Exception("Выбранная диаграмма была не найдена");

            if (currentDiagramm.EType != selType && currentDiagramm.EType != EDiagramTypes.UNDEF)
            {
                MessageBox.Show("Выбранная опция не может быть выполнена для этого типа диаграмм", "Ошибка при выборе опции");
                return;
            }

            var type = currentDiagramm.EType;
            switch (type)
            {
                case EDiagramTypes.UCD:
                    try
                    {
                        UCDMetricsCalculator umc = new UCDMetricsCalculator(currentDiagramm);
                        fillMetricsForm(umc);
                    }
                    catch
                    {
                        MessageBox.Show("В xmi файле присутствует критическая ошибка, не позволяющая вычислить метрики.\nПожалуйста, исправьте xmi файл и попробуйте снова.", "Xmi файл поврежден");
                    }
                    break;
                case EDiagramTypes.AD:
                    MessageBox.Show("Coming soon");
                    break;
                case EDiagramTypes.CD:
                    MessageBox.Show("Coming soon");
                    break;
                default:
                    MessageBox.Show("Неудалось определить тип диаграмммы", "Xmi файл поврежден");
                    break;
            }
        }
        private void fillMetricsForm(MetricsCalculator mc)
        {
            Form mf = new metricsFrom(mc);
            mf.Show();
        }

        private void экспортМетрикUCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            export(EDiagramTypes.UCD);
        }
        private void export(EDiagramTypes selType)
        {
            if (Distribution.AllDiagrams.Values.Where(d => d.EType == selType).Count() == 0)
            {
                ShowMsg("Нет ни одной диаграммы выбранного типа", "Экспорт метрик");
                return;
            }
            var selectedKey = diagramsGV.CurrentCell.Value.ToString();

            var saveDialog = new SaveFileDialog
            {
                Title = "Сохранение вычисленных метрик",
                FileName = "Metrics.txt",
                Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveDialog.FileName);
                foreach (var diag in Distribution.AllDiagrams.Values)
                {
                    if (diag.EType == selType)
                    {
                        try
                        {
                            UCDMetricsCalculator umc = new UCDMetricsCalculator(diag);
                            umc.MetricsOutput(umc.metrics, sw);
                        }
                        catch
                        {
                            sw.WriteLine("Название диаграммы: " + diag.Name);
                            sw.WriteLine("Программа не может вычислить метрики.\nСкорее всего ошибка в xmi файле.\nПроверьте xmi-файл после чего попробуйте снова.\n");
                        }
                    }
                }
                sw.Close();
            }
        }
    }
}
