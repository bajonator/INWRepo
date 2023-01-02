using System.IO;
namespace DataAccesLayer.Files
{
    public class AddProductToCsv
    {
        public void Add(ProductModel.ProductModel product, string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    var value = product.Code + ";" + product.ProductName + ";" + product.Quantity + ";" + product.Unit + ";" + product.Price + ";" + product.TotalPrice;
                    writer.Write(value);
                    writer.WriteLine();
                    writer.Close();
                }
            }
        }
    }
}
