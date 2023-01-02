using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Inwentaryzacja.Files
{
    public class CreatePDFfile
    {
        public void CreateDocument(string fileName, DataGridView dgv)
        {
            try
            {
                if (fileName != "Baza danych:")
                {
                    string path = fileName.Replace(".csv", ".pdf");
                    // Utwórz nowy dokument PDF
                    var pdfdoc = new Document();


                    // Utwórz obiekt PdfWriter i podłącz go do strumienia wyjściowego
                    PdfWriter.GetInstance(pdfdoc, new FileStream(path, FileMode.OpenOrCreate));

                    // Otwórz dokument do edycji
                    pdfdoc.Open();

                    // Dodaj treść do dokumentu
                    pdfdoc.Add(new Paragraph("text"));


                    //pusta linijka
                    var spacer = new Paragraph("")
                    {
                        SpacingBefore = 10f,
                        SpacingAfter = 10f,
                    };
                    pdfdoc.Add(spacer);

                    // tabela z dgvProducts
                    var columnCount = dgv.ColumnCount;
                    var columnWidth = new[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                    var table = new PdfPTable(columnWidth)
                    {
                        HorizontalAlignment = (int)LeftRightAlignment.Left,
                        WidthPercentage = 100,
                        DefaultCell = { MinimumHeight = 10f }
                    };

                    dgv.Columns
                        .OfType<DataGridViewColumn>()
                        .ToList()
                        .ForEach(c => table.AddCell(c.Name));
                    dgv.Rows
                        .OfType<DataGridViewRow>()
                        .ToList()
                        .ForEach(r =>
                        {
                            var cells = r.Cells.OfType<DataGridViewCell>().ToList();
                            cells.ForEach(c => table.AddCell(c?.Value.ToString()));
                        });

                    pdfdoc.Add(table);

                    pdfdoc.Add(spacer);


                    // Zamykamy dokument
                    pdfdoc.Close();

                }
                else
                {
                    MessageBox.Show("Nie wybrano bazydanych która ma być wyeksportowana", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
            }
        }
    }

 //   dodawanie obrazka

 //   var imagepath = "sciezka do obrazka";
 //   using(FileStream fs = new FileStream(imagepath, FileMode.Open))
	//{
 //       var png = Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Png);
 //       png.ScalePercent(5f);
 //       png.AbsolutePosition(pdfdoc.Left, pdfdoc.Top);
 //       pdfdoc.add(png);
	//}
}
