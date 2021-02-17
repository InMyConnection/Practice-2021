using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tanks
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void ResumeLoad(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Form1.entities;
        }
    }
}
