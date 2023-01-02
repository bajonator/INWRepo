using DataAccesLayer.Files;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Inwentaryzacja.Forms.CrateDatabase
{
    public partial class CreateNewDBForm : Form
    {
        Point point;
        public CreateNewDBForm(Point point)
        {
            InitializeComponent();
            this.point = point;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFileName.Text))
            {
                AddNewInventure addNew = new AddNewInventure();
                addNew.CreateNewDatabase(tbFileName.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Nie podano nazwy dla bazy danych produktów.", "Nie można utworzyć nowej inwentaryzacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
            }
        }

        private void CreateNewDBForm_Load(object sender, EventArgs e)
        {
            Location = new Point(point.X, point.Y);
        }
    }
}