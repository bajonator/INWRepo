using DataAccesLayer.Classes;
using DataAccesLayer.Files;
using DataAccesLayer.ProductModel;
using Inwentaryzacja.Files;
using Inwentaryzacja.Forms;
using Inwentaryzacja.Forms.CrateDatabase;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Inwentaryzacja
{
    public partial class MainForm : Form
    {
        public static Panel pButtons;
        public static DataGridView dgv;
        public static Action<string> ButtonEnabled;
        public static Action ClearBindingSource;
        static double AllProductsTotalPrice = 0;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int Lparam);

        public MainForm()
        {
            InitializeComponent();
            pButtons = pButton;
            dgv = dgvProducts;
            tsslUser.Text = $"Użytkownik: {Environment.UserName}";
            ReadListProductDB();
        }

        private void ReadListProductDB()
        {
            ReadListProduct readListProduct = new ReadListProduct();
            readListProduct.Read();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductAddForm frm = new ProductAddForm();
            frm.ReloadProducts += (s, ea) =>
            {
                ProductEventArgs eventArgs = ea as ProductEventArgs;
                if (eventArgs != null)
                {
                    ProductModel product = eventArgs.Product;
                    bsProducts.Add(product);
                    lblTotalProducts.Text = $"Suma wszystkich produktów: {dgvProducts.Rows.Count}";
                    dgvProducts.ClearSelection();
                    dgvProducts.Rows[dgvProducts.Rows.Count - 1].Selected = true;
                    AddProductToCsv addProductToCsv = new AddProductToCsv();
                    addProductToCsv.Add(product, tsslDatabase.Text);
                }
                TotalPriceModify();
            };
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvProducts.Rows.Count > 0)
            {
                int selectedRowIndex = dgvProducts.CurrentRow.Index;
                if (dgvProducts.Rows.Count > 0)
                {
                    bsProducts.RemoveAt(selectedRowIndex);
                    RemoveProductInCsv remove = new RemoveProductInCsv();
                    remove.Remove(tsslDatabase.Text, selectedRowIndex);
                    if (dgvProducts.Rows.Count > 0)
                    {
                        ProductModel product = (ProductModel)bsProducts[selectedRowIndex];
                        lblTotalProducts.Text = $"Suma wszystkich produktów: {dgvProducts.Rows.Count}";

                        dgvProducts.ClearSelection();
                        dgvProducts.Rows[dgvProducts.Rows.Count - 1].Selected = true;
                    }
                }
            }
            TotalPriceModify();
        }


        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvProducts.SelectedRows[0].Index;
                ProductModel product = (ProductModel)bsProducts[selectedRowIndex];
                ProductModifyForm frm = new ProductModifyForm(product);
                frm.ReloadProducts += (s, ea) =>
                {
                    ProductEventArgs eventArgs = ea as ProductEventArgs;
                    if (eventArgs != null)
                    {
                        product = eventArgs.Product;
                        bsProducts[selectedRowIndex] = product;
                        lblTotalProducts.Text = $"Suma wszystkich produktów: {dgvProducts.Rows.Count}";
                        TotalPriceModify();
                        ModifyProductCsv modify = new ModifyProductCsv();
                        modify.ModifyProduct(tsslDatabase.Text, selectedRowIndex, product);
                    }
                };
                frm.ShowDialog();
            }
        }

        private void tsmiCreateNew_Click(object sender, EventArgs e)
        {
            Point point = new Point(MousePosition.X, MousePosition.Y);
            ButtonEnabled = new Action<string>(EnableButton);
            ClearBindingSource = new Action(ClearBs);
            CreateNewDBForm frm = new CreateNewDBForm(point);
            frm.ShowDialog();
        }

        public void EnableButton(string fileName)
        {
            tsslDatabase.Text = fileName;
            pButtons.Enabled = true;
        }
        public void ClearBs()
        {
            bsProducts.Clear();
        }

        private void tsmiLoadExist_Click(object sender, EventArgs e)
        {
            ReadExistInventure read = new ReadExistInventure();
            ClearBindingSource = new Action(ClearBs);
            read.ReloadProducts += (s, ea) =>
            {
                ProductEventArgs eventArgs = ea as ProductEventArgs;
                if (eventArgs != null)
                {
                    ProductModel product = eventArgs.Product;
                    bsProducts.Add(product);
                    lblTotalProducts.Text = $"Suma wszystkich produktów: {dgvProducts.Rows.Count}";
                    dgvProducts.ClearSelection();
                    dgvProducts.Rows[dgvProducts.Rows.Count - 1].Selected = true;
                }
                tsslDatabase.Text = s.ToString();
                pButton.Enabled = true;
            };
            read.ReloadProductsIsNull += (s, ea) =>
            {
                tsslDatabase.Text = s.ToString();
                pButton.Enabled = true;
            };
            read.ReadExistInv();
            TotalPriceModify();
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void tsmiCreatePDF_Click(object sender, EventArgs e)
        {
            CreatePDFfile create = new CreatePDFfile();
            create.CreateDocument(tsslDatabase.Text, dgvProducts);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmiOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath);
        }

        private void TotalPriceMinus(ProductModel product)
        {
            AllProductsTotalPrice -= product.TotalPrice;
            lblTotalPrice.Text = $"Cena wszystkich produktów: {AllProductsTotalPrice}";
        }
        private void TotalPricePlus( ProductModel product)
        {
            AllProductsTotalPrice += product.TotalPrice;
            lblTotalPrice.Text = $"Cena wszystkich produktów: {AllProductsTotalPrice}";
        }

        private void TotalPriceModify()
        {
            AllProductsTotalPrice = 0;
            foreach (ProductModel product in bsProducts)
            {
                AllProductsTotalPrice += product.TotalPrice;
            }
            lblTotalPrice.Text = $"Cena wszystkich produktów: {AllProductsTotalPrice}";
        }

        private void tssmiListProdukts_Click(object sender, EventArgs e)
        {
            ProductListForm frm = new ProductListForm();
            frm.ShowDialog();
        }
    }
}