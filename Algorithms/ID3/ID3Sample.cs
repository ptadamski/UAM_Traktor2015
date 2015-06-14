using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraktorProj.Commons;

namespace TraktorProj.Algorithms
{
    public class ID3Sample
    {
        public List<string> TreeList = new List<string>();
        private List<object[]> objeclist = new List<object[]>();

        public ID3Sample()
        {
        }

        public void displayNode(ID3TreeNode root, string tabs, TextWriter stream)
        {
            stream.WriteLine(tabs + "|" + root.attribute + '|');
            TreeList.Add(tabs + '|' + root.attribute + '|');

            if (root.attribute.values != null)
            {
                for (int i = 0; i < root.attribute.values.Length; i++)
                {
                    stream.WriteLine(tabs + "\t" + "<" + root.attribute.values[i] + ">");
                    TreeList.Add(tabs + "\t" + "<" + root.attribute.values[i] + ">");
                    ID3TreeNode childNode = root.getChildByBranchName(root.attribute.values[i]);
                    displayNode(childNode, "\t" + tabs, stream);
                }
            }
        }

        public void AddToResultDataTable(object[] objects)
        {
            objeclist.Add(objects);
        }

        public DataTable getSampleData(string filepath)
        {
            return new CSV(filepath, ',', true).Table;
        }

        public ID3TreeNode GenerateTree(string filepath, string targetAttribute)
        {
            DataTable table = getSampleData(filepath);

            ID3Attrib[] attribs = new ID3Attrib[table.Columns.Count - 1];


            var data = table.AsEnumerable();

            for (int i = 0, j = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].ColumnName == targetAttribute)
                    continue;
                attribs[j++] = new ID3Attrib(table.Columns[i].ColumnName, data.Select(x => x[table.Columns[i]] as string).Distinct().ToArray());
            }

            DecisionTreeID3 id3 = new DecisionTreeID3();
            ID3TreeNode root = id3.buildTree(table, targetAttribute, attribs);

            var mem = new MemoryStream();
            var writter = new StreamWriter(mem);
            displayNode(root, "", writter);
            //writter.Flush();
            // mem.Position = 0;
            //var reader = new StreamReader(mem);

            // Console.WriteLine(reader.ReadToEnd());

            //return TreeList;

            return root;
        }

    }
}
