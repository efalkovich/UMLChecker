using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Verification.metrics_calc;

namespace Verification
{
    public partial class metricsFrom : Form
    {
        public metricsFrom(MetricsCalculator mc)
        {
            MaximizeBox = false;
            InitializeComponent();
            dgvInit(mc);
        }

        private void dgvInit(MetricsCalculator mc)
        {
            dgvMetrics.AutoSize = true;
            foreach(string key in mc.metrics.Keys)
            {
                dgvMetrics.Rows.Add(key, mc.metrics[key]);
            }
        }
    }
}
