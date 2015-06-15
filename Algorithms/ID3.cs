using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
//AUTHOR: Roosevelt dos Santos Júnior
using System.Windows.Controls;
using System.Windows.Documents;
using TraktorProj.Commons;
using System.Linq;

namespace TraktorProj.ID3Algorithm
{

	public class ID3Attrib
	{
		ArrayList mValues;
		string mName;
		object mLabel;

		public ID3Attrib(string name, string[] values)
		{
			mName = name;
			mValues = new ArrayList(values);
			mValues.Sort();
		}

		public ID3Attrib(object Label)
		{
			mLabel = Label;
			mName = string.Empty;
			mValues = null;
		}

		public string AttributeName
		{
			get
			{
				return mName;
			}
		}

        public Object AttributeLabel
        {
            get
            {
                return mLabel;
            }
        }

		public string[] values
		{
			get
			{
				if (mValues != null)
					return (string[])mValues.ToArray(typeof(string));
				else
					return null;
			}
		}

		public bool isValidValue(string value)
		{
			return indexValue(value) >= 0;
		}

		public int indexValue(string value)
		{
			if (mValues != null)
				return mValues.BinarySearch(value);
			else
				return -1;
		}

		public override string ToString()
		{
			if (mName != string.Empty)
			{
				return mName;
			}
			else
			{
				return mLabel.ToString();
			}
		}
	}

	public class TreeNode
	{
		private ArrayList mChilds = null;
		private ID3Attrib mAttribute;

		public TreeNode(ID3Attrib attribute)
		{
			if (attribute!=null && attribute.values != null)
			{
				mChilds = new ArrayList(attribute.values.Length);
				for (int i = 0; i < attribute.values.Length; i++)
					mChilds.Add(null);
			}
			else
			{
				mChilds = new ArrayList(1);
				mChilds.Add(null);
			}
			mAttribute = attribute;
		}

		public void AddTreeNode(TreeNode treeNode, string ValueName)
		{
			int index = mAttribute.indexValue(ValueName);
			mChilds[index] = treeNode;
		}

		public int totalChilds
		{
			get
			{
				return mChilds.Count;
			}
		}

		public TreeNode getChild(int index)
		{
			return (TreeNode)mChilds[index];
		}

		public ID3Attrib attribute
		{
			get
			{
				return mAttribute;
			}
		}
		public TreeNode getChildByBranchName(string branchName)
		{
			int index = mAttribute.indexValue(branchName);
			return (TreeNode)mChilds[index];
		}
	}
	
	public class DecisionTreeID3
	{
		private DataTable mSamples;
		private int mTotalPositives = 0;
		private int mTotal =32;
		private string mTargetAttribute = "result";
		private double mEntropySet = 0.0;

        /// <summary>
        /// Zlicza niepuste pola we wskazanej kolumnie tabeli
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
		private int countTotalPositives(DataTable samples)
		{
			int result = 0;

			foreach (DataRow aRow in samples.Rows)
			{
                var x = samples.Columns;
                var t = aRow[mTargetAttribute];
				if (!aRow[mTargetAttribute].Equals(""))
					result++;
			}

			return result;
		}

		private double calcEntropy(int positives, int negatives)
		{
			int total = positives + negatives;
			double ratioPositive = (double)positives/total;
			double ratioNegative = (double)negatives/total;

			if (ratioPositive != 0)
				ratioPositive = -(ratioPositive) * System.Math.Log(ratioPositive, 2);
			if (ratioNegative != 0)
				ratioNegative = - (ratioNegative) * System.Math.Log(ratioNegative, 2);

			double result =  ratioPositive + ratioNegative;

			return result;
		}

        /// <summary>
        /// Zlicza w zadanej kolumnie ile jest wartosci pozytywnych (niepustych) a ile negatywnych (pustych) odpowiadajacych wskazanemu wzorcowi
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="positives"></param>
        /// <param name="negatives"></param>
		private void getValuesToAttribute(DataTable samples, ID3Attrib attribute, string value, out int positives, out int negatives)
		{
			positives = 0;
			negatives = 0;

			foreach (DataRow aRow in samples.Rows)
			{
				if (  ((string)aRow[attribute.AttributeName] == value) )
					if ( !aRow[mTargetAttribute].Equals("")) 
						positives++;
					else
						negatives++;
			}		
		}

        /// <summary>
        /// Oblicza przyrosc informacji wybranego atrybutu
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
		private double calculateGain(DataTable samples, ID3Attrib attribute)
		{
			string[] values = attribute.values;
			double sum = 0.0;

			for (int i = 0; i < values.Length; i++)
			{
				int positives, negatives;
				
				positives = negatives = 0;
				
				getValuesToAttribute(samples, attribute, values[i], out positives, out negatives);
				
				double entropy = calcEntropy(positives, negatives);
				sum += -(double)(positives + negatives)/mTotal * entropy;
			}
			return mEntropySet + sum;
		}

        /// <summary>
        /// Znajduje atrybut o maksymalnym przyroscie informacji
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
		private ID3Attrib   findBestAttribute(DataTable samples, ID3Attrib[] attributes)
		{
			double maxGain = 0.0;
			ID3Attrib result = new ID3Attrib("");

			foreach (ID3Attrib attribute in attributes)
			{
				double gain = calculateGain(samples, attribute);
				if (gain > maxGain)
				{
					maxGain = gain;
					result = attribute;
				}
			}
           
			return result;
		}

        /// <summary>
        /// Sprawdza czy wszystkie komorki z zadanej kolumny sa niepuste
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="targetAttribute"></param>
        /// <returns></returns>
		private bool allSamplesPositives(DataTable samples, string targetAttribute)
		{			
			foreach (DataRow row in samples.Rows)
			{
				if ( row[targetAttribute].Equals(""))
					return false;
			}

			return true;
		}

        /// <summary>
        /// Sprawdza czy wszystkie komorki z zadanej kolumny sa puste
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="targetAttribute"></param>
        /// <returns></returns>
		private bool allSamplesNegatives(DataTable samples, string targetAttribute)
        {
            //czy wszystkie komorki tabeli sa puste?
			foreach (DataRow row in samples.Rows)
			{
				if ( !row[targetAttribute].Equals(""))
					return false;
			}

			return true;			
		}
        
        /// <summary>
        /// Wyluskuje zbior unikatow z zadanej kolumny tabeli
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="targetAttribute"></param>
        /// <returns></returns>
		private ArrayList getDistinctValues(DataTable samples, string targetAttribute)
		{
            //wyluskaj zbior unikalnych wartosci z zadanej columny
            //samples.AsEnumerable().Select(x=>x[targetAttribute]).Distinct().ToList();
			ArrayList distinctValues = new ArrayList(samples.Rows.Count);

			foreach(DataRow row in samples.Rows)
			{
				if (distinctValues.IndexOf(row[targetAttribute]) == -1)
					distinctValues.Add(row[targetAttribute]);
			}

			return distinctValues;
		}

        /// <summary>
        /// Znajduje obiekt o maksymalnej liczbie wystapien
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="targetAttribute"></param>
        /// <returns></returns>
		private object getMostCommonValue(DataTable samples, string targetAttribute)
		{
			ArrayList distinctValues = getDistinctValues(samples, targetAttribute);
			int[] count = new int[distinctValues.Count];

			foreach(DataRow row in samples.Rows)
			{
				int index = distinctValues.IndexOf(row[targetAttribute]);
				count[index]++;
			}
			
			int MaxIndex = 0;
			int MaxCount = 0;

			for (int i = 0; i < count.Length; i++)
			{
				if (count[i] > MaxCount)
				{
					MaxCount = count[i];
					MaxIndex = i;
				}
			}

			return distinctValues[MaxIndex];
		}

        private TreeNode internalMountTree(DataTable samples, string targetAttribute, ID3Attrib[] attributes)
        {
            if (allSamplesPositives(samples, targetAttribute) == true)
            //return new TreeNode(new Attribute(true));

            if (allSamplesNegatives(samples, targetAttribute) == true)
            //return new TreeNode(new Attribute(getMostCommonValue(samples, targetAttribute)));

            //if (attributes.Length == 0)
            //return new TreeNode(new Attribute(getMostCommonValue(samples, targetAttribute)));			

            mTotal = samples.Rows.Count;//16;
            mTargetAttribute = targetAttribute;
            mTotalPositives = countTotalPositives(samples);

            mEntropySet = calcEntropy(mTotalPositives, mTotal - mTotalPositives);

            ID3Attrib bestAttribute = findBestAttribute(samples, attributes);

            TreeNode root = new TreeNode(bestAttribute);

            DataTable aSample = samples.Clone();
            if (bestAttribute.values == null)
            {
                return new TreeNode(new ID3Attrib(getMostCommonValue(samples, targetAttribute)));
            }
            else
            {
                foreach (string value in bestAttribute.values)
                {

                    aSample.Rows.Clear();

                    DataRow[] rows = samples.Select(bestAttribute.AttributeName + " = " + "'" + value + "'");

                    foreach (DataRow row in rows)
                    {
                        aSample.Rows.Add(row.ItemArray);
                    }

                    ArrayList aAttributes = new ArrayList(attributes.Length - 1);
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        if (attributes[i].AttributeName != bestAttribute.AttributeName)
                            aAttributes.Add(attributes[i]);
                    }


                    if (aSample.Rows.Count == 0)
                    {
                        return new TreeNode(new ID3Attrib(getMostCommonValue(aSample, targetAttribute)));
                    }
                    else
                    {
                        DecisionTreeID3 dc3 = new DecisionTreeID3();
                        TreeNode ChildNode = dc3.buildTree(aSample, targetAttribute, (ID3Attrib[])aAttributes.ToArray(typeof(ID3Attrib)));
                        root.AddTreeNode(ChildNode, value);
                    }
                }
            }

            return root;
        }

		public TreeNode buildTree(DataTable samples, string targetAttribute, ID3Attrib[] attributes)
		{
			mSamples = samples;
			return internalMountTree(mSamples, targetAttribute, attributes);
		}
	}

    //tragedia, trzeba to pobierac jakos z zewnatrz
	public class ID3Sample
	{
	    public List<string> TreeList = new List<string>();
        private List<object[]> objeclist = new List<object[]>();
                               
		public void displayNode(TreeNode root, string tabs)
		{
			Console.WriteLine(tabs + "|" + root.attribute + '|');
		    TreeList.Add(tabs + '|' + root.attribute + '|');

			if (root.attribute.values != null)
			{
				for (int i = 0; i < root.attribute.values.Length; i++)
				{
					Console.WriteLine(tabs + "\t" + "<" + root.attribute.values[i] + ">");
                    TreeList.Add(tabs + "\t" + "<" + root.attribute.values[i] + ">");
					TreeNode childNode = root.getChildByBranchName(root.attribute.values[i]);
					displayNode(childNode, "\t" + tabs);
				}
			}
		}

	    public void AddToResultDataTable(object[] objects)
	    {
            objeclist.Add(objects);
	    }

		public DataTable getSampleData(string filepath)
		{
            CSV csv = new CSV(filepath, ',', true);
            return csv.Table;
		}

	    public List<string> GenerateTree(string filepath, string mainAttribName)
	    {
           

            /*ID3Attrib pora = new ID3Attrib("pora", new string[] { "jesien", "wiosna", "lato" });
            ID3Attrib uprawa = new ID3Attrib("uprawa", new string[] { "zboze", "warzywo"});
            ID3Attrib susza = new ID3Attrib("susza", new string[] { "tak", "nie" });
            ID3Attrib mineraly = new ID3Attrib("mineraly", new string[] { "tak", "nie" });
            ID3Attrib zbior = new ID3Attrib("zbior", new string[] { "tak", "nie" });
            ID3Attrib zaorane = new ID3Attrib("zaorane", new string[] { "tak", "nie" });
            ID3Attrib bronowane = new ID3Attrib("bronowane", new string[] { "tak", "nie" });    */


            // { pora, uprawa, susza, mineraly, zbior,zaorane, bronowane };
            DataTable samples = getSampleData(filepath);
            var table = samples.AsEnumerable();


            ID3Attrib[] attributes = new ID3Attrib[samples.Columns.Count-1];
            for (int i = 0, j=0, length = samples.Columns.Count; i < length; i++)
            {
                if (samples.Columns[i].ColumnName == mainAttribName)
                    continue;

                var values = table.Select(x => x[i] as string ).Distinct().ToArray();

                attributes[j] = new ID3Attrib(samples.Columns[i].ColumnName, values);
                j++;
            }

            DecisionTreeID3 id3 = new DecisionTreeID3();
            TreeNode root = id3.buildTree(samples, mainAttribName, attributes);

            displayNode(root, "");

	        return TreeList;
	    }

        public List<string> GenerateTree2()
        {

          
            ID3Attrib pora = new ID3Attrib("pora", new string[] { "jesien", "wiosna", "lato" });
            ID3Attrib rozrost = new ID3Attrib("rozrost", new string[] { "tak", "nie" });
            ID3Attrib chodnik = new ID3Attrib("chodnik", new string[] { "tak", "nie" });
            ID3Attrib trawa = new ID3Attrib("trawa", new string[] { "tak", "nie" });
            ID3Attrib pole = new ID3Attrib("pole", new string[] { "tak", "nie" });
            ID3Attrib uprawy = new ID3Attrib("uprawy", new string[] { "tak", "nie" });
            ID3Attrib[] attributes = new ID3Attrib[] { pora, rozrost, chodnik, trawa, pole, uprawy};
            DataTable samples = getSampleData(@"..\..\szkodniki");

            DecisionTreeID3 id3 = new DecisionTreeID3();
            TreeNode root = id3.buildTree(samples, "szkodnik", attributes);

            displayNode(root, "");

            return TreeList;
        }

	}
}
