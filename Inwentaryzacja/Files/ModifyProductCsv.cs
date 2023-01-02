using DataAccesLayer.ProductModel;
using System.Collections.Generic;
using System.IO;

namespace Inwentaryzacja.Files
{
    public class ModifyProductCsv
    {
        public void ModifyProduct(string fileName, int productIndex, ProductModel product)
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
                lines[productIndex] = product.Code + ";" + product.ProductName + ";" + product.Quantity + ";" + product.Unit + ";" + product.Price + ";" + product.TotalPrice;
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (string l in lines)
                    {
                        writer.WriteLine(l);
                    }
                    writer.Close();
                }
            }
        }
    }
}
