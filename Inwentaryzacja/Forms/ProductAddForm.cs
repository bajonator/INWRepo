using DataAccesLayer.Classes;
using DataAccesLayer.ProductModel;
using Inwentaryzacja.Forms.Base;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using Inwentaryzacja.Files;

namespace Inwentaryzacja.Forms
{
    public partial class ProductAddForm : BaseForm
    {
        public ProductModel product;
        public EventHandler ReloadProducts;
        Stopwatch sw;

        public ProductAddForm()
        {
            InitializeComponent();
            ValidateControls();
        }

        private void InformationShow()
        {

        }

        private void ValidateControls()
        {
            if (string.IsNullOrWhiteSpace(tbProductName.Text))
            {
                epProductName.SetError(tbProductName, "Pole nazwa produktu jest wymagane.\n");
            }
            else
            {
                epProductName.Clear();
            }

            if (string.IsNullOrWhiteSpace(cbUnit.Text))
            {
                epUnit.SetError(cbUnit, "Pole jednostka miary jest wymagane.\n");
            }
            else
            {
                epUnit.Clear();
            }

            if (nudQuantity.Value == 0)
            {
                epQuantity.SetError(nudQuantity, "Pole ilość nie może mieć wartości 0.\n");
            }
            else
            {
                epQuantity.Clear();
            }

            if (nudPrice.Value == 0)
            {
                epPrice.SetError(nudPrice, "Pole cena nie może mieć wartości 0.\n");
            }
            else
            {
                epPrice.Clear();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                ProductModel product = new ProductModel()
                {
                    ProductName = tbProductName.Text,
                    Code = tbBarcode.Text,
                    Unit = cbUnit.Text,
                    Quantity = (int)nudQuantity.Value,
                    Price = (double)nudPrice.Value,
                    TotalPrice = Convert.ToDouble(lblTotalPrice.Text),
                };
                ReloadProducts?.Invoke(btnSave, new ProductEventArgs(product));
                NewProduct();
                this.ActiveControl = tbBarcode;
            }
        }

        private void NewProduct()
        {
            tbBarcode.Text = "";
            tbProductName.Text = "";
            cbUnit.Text = "";
            nudQuantity.Value = 0;
            nudPrice.Value = 0;
            lblTotalPrice.Text = "0";
        }

        private bool ValidateForm()
        {
            StringBuilder sbErrorMessage = new StringBuilder();

            string productNameErrorMessage = epProductName.GetError(tbProductName);
            if (!string.IsNullOrEmpty(productNameErrorMessage))
            {
                sbErrorMessage.Append(productNameErrorMessage);
            }

            string unitErrorMessage = epUnit.GetError(cbUnit);
            if (!string.IsNullOrEmpty(unitErrorMessage))
            {
                sbErrorMessage.Append(unitErrorMessage);
            }

            string quantityErrorMessage = epQuantity.GetError(nudQuantity);
            if (nudQuantity.Value == 0)
            {
                sbErrorMessage.Append(quantityErrorMessage);
            }

            string priceErrorMessage = epPrice.GetError(nudPrice);
            if (nudPrice.Value == 0)
            {
                sbErrorMessage.Append(priceErrorMessage);
            }

            if (sbErrorMessage.Length > 0)
            {
                MessageBox.Show(
                    sbErrorMessage.ToString(),
                    "Dodawanie productu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbProductName_TextChanged(object sender, EventArgs e)
        {
            ValidateControls();
        }

        private void nudQuantity_ValueChanged(object sender, EventArgs e)
        {
            lblTotalPrice.Text = TotalPrice();
            ValidateControls();
        }

        private string TotalPrice()
        {
            double total = (double)nudPrice.Value * (int)nudQuantity.Value;

            return total.ToString();
        }

        private void nudPrice_ValueChanged(object sender, EventArgs e)
        {
            lblTotalPrice.Text = TotalPrice();
            ValidateControls();
        }


        private void cbUnit_TextChanged(object sender, EventArgs e)
        {
            ValidateControls();
        }

        private void tbBarcode_TextChanged(object sender, EventArgs e)
        {
            if (tbBarcode.Text.Length == 0)
            {
                sw.Start();
                lblScan.Visible = true;
                
            }
            else
            {
                FindProduct();
                sw.Stop();
                lblScan.Visible = false;
            }
        }

        private void FindProduct()
        {
            if (ReadListProduct.productLists != null)
            {
                foreach (ProductListDB product in ReadListProduct.productLists)
                {
                    if (product.Code.ToString() == tbBarcode.Text)
                    {
                        tbProductName.Text = product.ProductName;
                        ActiveControl = tbProductName;
                        return;
                    }
                    else
                    {
                        tbProductName.Text = "Nie znaleziono produktu";
                    }
                }
            }
            else
            {
                tbProductName.Text = "Nie znaleziono produktu";
            }
        }

        private async void SoftBlink(Control ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
        {
            sw = new Stopwatch();
            sw.Start();
            short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
            while (true)
            {
                await Task.Delay(1);
                var n = sw.ElapsedMilliseconds % CycleTime_ms;
                var per = (double)Math.Abs(n - halfCycle) / halfCycle;
                var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
                var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
                var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
                var clr = Color.FromArgb(red, grn, blw);
                if (BkClr) ctrl.BackColor = clr; else ctrl.ForeColor = clr;
            }
        }

        private void ProductAddForm_Load(object sender, EventArgs e)
        {
            SoftBlink(lblScan, Color.FromArgb(0, 0, 0), Color.Red, 2000, false);
            //SoftBlink(lblScan, Color.FromArgb(30, 30, 30), Color.Green, 2000, true);
        }

        private void btnAddNewProduct_Click(object sender, EventArgs e)
        {
            AddNewProductInDataProducts add = new AddNewProductInDataProducts();
            add.ProductAdd(tbBarcode.Text, tbProductName.Text);
        }
    }
}
