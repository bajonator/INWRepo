using DataAccesLayer.Classes;
using DataAccesLayer.ProductModel;
using Inwentaryzacja.Forms.Base;
using System;
using System.Text;
using System.Windows.Forms;

namespace Inwentaryzacja.Forms
{
    public partial class ProductModifyForm : BaseForm
    {
        public ProductModel product;
        public EventHandler ReloadProducts;
        public ProductModifyForm(ProductModel product)
        {
            InitializeComponent();
            this.product = product;
            PrepareProduct(product);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                product.ProductName = tbProductName.Text;
                product.Code = tbBarcode.Text;
                product.Unit = cbUnit.Text;
                product.Quantity = (int)nudQuantity.Value;
                product.Price = (double)nudPrice.Value;
                product.TotalPrice = Convert.ToDouble(lblTotalPrice.Text);

                ReloadProducts?.Invoke(btnSave, new ProductEventArgs(product));
                Close();
            }
        }

        private void PrepareProduct(ProductModel product)
        {
            tbProductName.Text = product.ProductName;
            tbBarcode.Text = product.Code;
            cbUnit.Text = product.Unit;
            nudQuantity.Value = product.Quantity;
            nudPrice.Text = product.Price.ToString();
            lblTotalPrice.Text = product.TotalPrice.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}
