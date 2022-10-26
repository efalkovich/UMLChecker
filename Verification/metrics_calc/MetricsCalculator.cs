using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.metrics_calc
{
    public class MetricsCalculator
    {
        protected Diagram model;
        public Dictionary<string, double> metrics;
        public MetricsCalculator(Diagram model)
        {
            this.model = model;
            metrics = new Dictionary<string, double>();
        }
        public void MetricsOutput(Dictionary<string, double> mc, StreamWriter sw)
        {
            sw.WriteLine("Название диаграммы: " + model.Name);
            foreach (var key in mc.Keys)
                sw.WriteLine(key + ": " + mc[key]);
            sw.WriteLine();
        }
    }
}
