using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicManager
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// TODO: This line of code loads data into the 'magicDataSet1.Matches' table. You can move, or remove it, as needed.
			this.matchesTableAdapter.Fill(this.magicDataSet1.Matches);
			// TODO: This line of code loads data into the 'magicDataSet.Players' table. You can move, or remove it, as needed.
			this.playersTableAdapter.Fill(this.magicDataSet.Players);

		}

		private void fillByToolStripButton_Click(object sender, EventArgs e)
		{
			try
			{
				this.playersTableAdapter.Fill(this.magicDataSet.Players);
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
			}

		}
	}
}
