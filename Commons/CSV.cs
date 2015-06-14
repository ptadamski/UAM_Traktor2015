using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TraktorProj.Commons
{
    public class CSV
    {
        public CSV(string filePath, char separator, bool withHeader)
        {
            this.separator = separator;
            LoadFromFile(filePath, withHeader);
        }

        private char separator;

        public char Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        private DataTable table = new DataTable();

        public DataTable Table
        {
            get { return table; }
            set { table = value; }
        }


        public void LoadFromFile(string filePath, bool header = true)
        {
            StreamReader reader = new StreamReader(filePath);

            string line;
            table.Clear();

            while ((line = reader.ReadLine()) != null)
            {
                var items = line.Split(separator);

                if (header)
                {
                    for (int i = 0, length = items.Length; i < length; i++)
                        table.Columns.Add(new DataColumn(items[i]));
                    header = false;
                }
                else
                {
                    var row = table.NewRow();
                    row.ItemArray = items;
                    table.Rows.Add(row);
                }
            }

            reader.Close();
        }

        public void SaveToFile(string filePath)
        {
        }
    }
}
