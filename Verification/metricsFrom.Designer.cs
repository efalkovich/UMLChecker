namespace Verification
{
    partial class metricsFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMetrics = new System.Windows.Forms.DataGridView();
            this.Значение = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Наименование = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetrics)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMetrics
            // 
            this.dgvMetrics.AllowUserToAddRows = false;
            this.dgvMetrics.AllowUserToDeleteRows = false;
            this.dgvMetrics.AllowUserToResizeColumns = false;
            this.dgvMetrics.AllowUserToResizeRows = false;
            this.dgvMetrics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMetrics.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMetrics.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMetrics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetrics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Наименование,
            this.Значение});
            this.dgvMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMetrics.Location = new System.Drawing.Point(0, 0);
            this.dgvMetrics.Name = "dgvMetrics";
            this.dgvMetrics.RowHeadersVisible = false;
            this.dgvMetrics.RowHeadersWidth = 62;
            this.dgvMetrics.RowTemplate.Height = 28;
            this.dgvMetrics.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvMetrics.Size = new System.Drawing.Size(543, 272);
            this.dgvMetrics.TabIndex = 0;
            // 
            // Значение
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Значение.DefaultCellStyle = dataGridViewCellStyle3;
            this.Значение.HeaderText = "Значение";
            this.Значение.MinimumWidth = 8;
            this.Значение.Name = "Значение";
            // 
            // Наименование
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Наименование.DefaultCellStyle = dataGridViewCellStyle2;
            this.Наименование.HeaderText = "Наименование";
            this.Наименование.MinimumWidth = 8;
            this.Наименование.Name = "Наименование";
            // 
            // metricsFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(543, 272);
            this.Controls.Add(this.dgvMetrics);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "metricsFrom";
            this.Text = "metricsFrom";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetrics)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMetrics;
        private System.Windows.Forms.DataGridViewTextBoxColumn Наименование;
        private System.Windows.Forms.DataGridViewTextBoxColumn Значение;
    }
}