using Inwentaryzacja.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inwentaryzacja.Forms
{
    public partial class ProductListForm : Form
    {
        public ProductListForm()
        {
            InitializeComponent();
        }

        private void ReadList()
        {
            dgvProductList.DataSource = ReadListProduct.productLists;
            dgvProductList.RowHeadersVisible = false;
            dgvProductList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProductList.Enabled = false;
            dgvProductList.Columns[0].HeaderText = "Kod";
            dgvProductList.Columns[1].HeaderText = "Nazwa produktu";
            dgvProductList.ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProductListForm_Load(object sender, EventArgs e)
        {
            ReadList();
        }
    }
}
