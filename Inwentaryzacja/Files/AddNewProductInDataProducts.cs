using DataAccesLayer.ProductModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inwentaryzacja.Files
{
    public class AddNewProductInDataProducts
    {
        const string DIR = @"\Data\";
        const string FILENAME = "ProductsDB.ini";

        public void ProductAdd(string code, string productName)
        {
            try
            {
                string fileSource = $"{Application.StartupPath}{DIR}{FILENAME}";
                if (File.Exists(fileSource))
                {
                    string[] lineread = File.ReadAllLines(fileSource);
                    for (int i = 0; i < lineread.Length; i++)
                    {
                        if (lineread[i].ToString().Contains(code) || lineread[i].ToString().Contains(productName))
                        {
                            Modify(fileSource, i, code, productName);
                            return;
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(fileSource, true))
                    {
                        var value = code + ";" + productName;
                        writer.Write(value);
                        writer.WriteLine();
                        writer.Close();
                        ProductListDB product = new ProductListDB()
                        {
                            Code = code,
                            ProductName = productName
                        };
                        ReadListProduct.productLists.Add(product);
                    }
                }
                else
                {
                    ReadListProduct.productLists = new List<ProductListDB>();
                    File.Create(fileSource).Dispose();
                    if (File.Exists(fileSource))
                    {
                        using (StreamWriter writer = new StreamWriter(fileSource, true))
                        {
                            var value = code + ";" + productName;
                            writer.Write(value);
                            writer.WriteLine();
                            writer.Close();
                            ProductListDB product = new ProductListDB()
                            {
                                Code = code,
                                ProductName = productName
                            };
                            ReadListProduct.productLists.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Modify(string fileName, int productIndex, string code, string productName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                List<string> lines = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                reader.Close();
                lines[productIndex] = code + ";" + productName;
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (string l in lines)
                    {
                        writer.WriteLine(l);
                    }
                    writer.Close();
                }
            }
            ReadListProduct readListProduct = new ReadListProduct();
            readListProduct.Read();
        }
    }
}
