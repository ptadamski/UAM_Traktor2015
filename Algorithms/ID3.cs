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

namespace TraktorProj.ID3Algorithm
{

	public class Attribute
	{
		ArrayList mValues;
		string mName;
		object mLabel;

		public Attribute(string name, string[] values)
		{
			mName = name;
			mValues = new ArrayList(values);
			mValues.Sort();
		}

		public Attribute(object Label)
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
		private Attribute mAttribute;

		public TreeNode(Attribute attribute)
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

		public Attribute attribute
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
		private int mTotal = 16;
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
		private void getValuesToAttribute(DataTable samples, Attribute attribute, string value, out int positives, out int negatives)
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
		private double calculateGain(DataTable samples, Attribute attribute)
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
		private Attribute   findBestAttribute(DataTable samples, Attribute[] attributes)
		{
			double maxGain = 0.0;
			Attribute result = new Attribute("");

			foreach (Attribute attribute in attributes)
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

        private TreeNode internalMountTree(DataTable samples, string targetAttribute, Attribute[] attributes)
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

            Attribute bestAttribute = findBestAttribute(samples, attributes);

            TreeNode root = new TreeNode(bestAttribute);

            DataTable aSample = samples.Clone();
            if (bestAttribute.values == null)
            {
                return new TreeNode(new Attribute(getMostCommonValue(samples, targetAttribute)));
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
                        return new TreeNode(new Attribute(getMostCommonValue(aSample, targetAttribute)));
                    }
                    else
                    {
                        DecisionTreeID3 dc3 = new DecisionTreeID3();
                        TreeNode ChildNode = dc3.buildTree(aSample, targetAttribute, (Attribute[])aAttributes.ToArray(typeof(Attribute)));
                        root.AddTreeNode(ChildNode, value);
                    }
                }
            }

            return root;
        }

		public TreeNode buildTree(DataTable samples, string targetAttribute, Attribute[] attributes)
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

        public ID3Sample(string sampleFilepath)
        {             
        }

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
            return csv;
		}

	    public List<string> GenerateTree()
	    {
            Attribute pora = new Attribute("pora", new string[] { "jesien", "wiosna", "lato" });
            Attribute uprawa = new Attribute("uprawa", new string[] { "zboze", "warzywo"});
            Attribute susza = new Attribute("susza", new string[] { "tak", "nie" });
            Attribute mineraly = new Attribute("mineraly", new string[] { "tak", "nie" });
            Attribute zbior = new Attribute("zbior", new string[] { "tak", "nie" });
            Attribute zaorane = new Attribute("zaorane", new string[] { "tak", "nie" });
            Attribute bronowane = new Attribute("bronowane", new string[] { "tak", "nie" });
            Attribute[] attributes = new Attribute[] { pora, uprawa, susza, mineraly, zbior,zaorane, bronowane };
            DataTable samples = getSampleData("maszyny");

            DecisionTreeID3 id3 = new DecisionTreeID3();
            TreeNode root = id3.buildTree(samples, "maszyna", attributes);

            displayNode(root, "");

	        return TreeList;
	    }

	}
}
