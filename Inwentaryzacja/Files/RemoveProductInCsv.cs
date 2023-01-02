using System.Collections.Generic;
using System.IO;

namespace Inwentaryzacja.Files
{
    public class RemoveProductInCsv
    {
        public void Remove(string fileName, int productId)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                List<string> lines = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                lines.RemoveAt(productId);
                reader.Close();
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
