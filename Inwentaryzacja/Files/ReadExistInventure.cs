using DataAccesLayer.Classes;
using DataAccesLayer.ProductModel;
using System;
using System.IO;
using System.Windows.Forms;

namespace Inwentaryzacja.Files
{
    public class ReadExistInventure
    {
        const string DIR = @"\Data\";
        public EventHandler ReloadProducts;
        public EventHandler ReloadProductsIsNull;

        public void ReadExistInv()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath + DIR;
            ofd.Filter = "Excel csv Files|*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ReadFile(ofd.FileName);
            }
        }

        private void ReadFile(string fileName)
        {
            try
            {
                string[] rows = File.ReadAllLines(fileName);
                if (rows.Length > 0)
                {
                    MainForm.ClearBindingSource();
                    for (int i = 0; i < rows.Length; i++)
                    {
                        string[] rowsSplit = rows[i].Split(';');
                        ProductModel product = new ProductModel
                        {
                            Code = rowsSplit[0],
                            ProductName = rowsSplit[1],
                            Quantity = Convert.ToInt32(rowsSplit[2]),
                            Unit = rowsSplit[3],
                            Price = Convert.ToDouble(rowsSplit[4]),
                            TotalPrice = Convert.ToDouble(rowsSplit[5])
                        };
                        ReloadProducts?.Invoke(fileName, new ProductEventArgs(product));
                    }
                }
                else
                {
                    MainForm.ClearBindingSource();
                    ReloadProductsIsNull?.Invoke(fileName, new ProductEventArgs(null));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}