using ActivityDiagramVer.parser;
using ActivityDiagramVer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification.metrics_calc
{
    internal class ADMetricsCalculator : MetricsCalculator
    {
        private int non;
        private int nolip;
        private int noe;
        private int cm;
        public ADMetricsCalculator(Diagram diag) : base(diag)
        {
            calcMetrics();
        }
        private void calcMetrics()
        {
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);
            bool hasJoinOrFork = false;
            parser.Parse(model, ref hasJoinOrFork);
            if (!model.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL))
            {
                adNodesList.connect();
            } else
            {
                MessageBox.Show("В диаграмме присутствуют критические ошибки не позволяющие вычислить метрики.\nПожалуйста, исправьте ошибки в xmi файле и попробуйте снова.", "Xmi файл поврежден");
                return;
            }

            List<ADNodesList.ADNode> nodesForMetrics = new List<ADNodesList.ADNode>();
            //foreach()

            non = calcNon();
            nolip = calcNolip();
            noe = calcNoe();
            cm = calcCm();
        }
        private int calcNon()
        {
            return 0;
        }
        private int calcNolip()
        {
            return 0;
        }
        private int calcNoe()
        {
            return 0;
        }
        private int calcCm()
        {
            return 0;
        }
    }
}
