using Inwentaryzacja;
using System;
using System.IO;
using System.Windows.Forms;

namespace DataAccesLayer.Files
{
    public class AddNewInventure
    {
        const string DIR = @"\Data\";
        const string FILEEXTENSION = ".csv";

        public void CreateNewDatabase(string FileName)
        {
            string databaseSource = Application.StartupPath + $"{DIR}{FileName}{FILEEXTENSION}"; 
            if (!File.Exists(databaseSource))
            {
                try
                {
                    Directory.CreateDirectory(Application.StartupPath + DIR);
                    File.Create(databaseSource).Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Inwentaryzacja już istnieje, chcesz usunąć istniejąca bazę danych i zastąpić nową?", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(databaseSource);
                        File.Create(databaseSource).Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    return;
                }
            }
            if (File.Exists(databaseSource))
            {
                MainForm.ClearBindingSource();
                MainForm.ButtonEnabled(databaseSource);
            }
        }
    }
}