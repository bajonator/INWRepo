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
    public class ReadListProduct
    {
        const string DIR = @"\Data\";
        const string FILENAME = "ProductsDB.ini";
        public static IList<ProductListDB> productLists;

        public void Read()
        {
            string fileSource = $"{Application.StartupPath}{DIR}{FILENAME}";
            if (File.Exists(fileSource))
            {
                productLists = new List<ProductListDB>();
                string[] prod = File.ReadAllLines(fileSource);
                for (int i = 0; i < prod.Length; i++)
                {
                    string[] prodSplit = prod[i].Split(';');
                    ProductListDB productList = new ProductListDB()
                    {
                        Code = prodSplit[0],
                        ProductName = prodSplit[1],
                    };
                    productLists.Add(productList);
                }
            }
        }
    }
}
