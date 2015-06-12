using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

//AUTHOR: Roosevelt dos Santos Júnior
using System.Windows.Controls;
using System.Windows.Documents;

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

		private int countTotalPositives(DataTable samples)
		{
			int result = 0;

			foreach (DataRow aRow in samples.Rows)
			{
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

		private bool allSamplesPositives(DataTable samples, string targetAttribute)
		{			
			foreach (DataRow row in samples.Rows)
			{
				if ( row[targetAttribute].Equals(""))
					return false;
			}

			return true;
		}

		private bool allSamplesNegatives(DataTable samples, string targetAttribute)
		{
			foreach (DataRow row in samples.Rows)
			{
				if ( !row[targetAttribute].Equals(""))
					return false;
			}

			return true;			
		}

		private ArrayList getDistinctValues(DataTable samples, string targetAttribute)
		{
			ArrayList distinctValues = new ArrayList(samples.Rows.Count);

			foreach(DataRow row in samples.Rows)
			{
				if (distinctValues.IndexOf(row[targetAttribute]) == -1)
					distinctValues.Add(row[targetAttribute]);
			}

			return distinctValues;
		}

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
            } else{
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

		public DataTable getSampleData()
		{
            DataTable result = new DataTable("sample");

            DataColumn 
            column = result.Columns.Add("pora");
            column.DataType = typeof(string);
            column = result.Columns.Add("uprawa");
            column.DataType = typeof(string);
            column = result.Columns.Add("susza");
            column.DataType = typeof(string);
            column = result.Columns.Add("mineraly");
            column.DataType = typeof(string);
            column = result.Columns.Add("zbior");
            column.DataType = typeof(string);
            column = result.Columns.Add("zaorane");
            column.DataType = typeof(string);
            column = result.Columns.Add("bronowane");
            column.DataType = typeof(string);
            
            column = result.Columns.Add("maszyna");
            column.DataType = typeof(string);
		    foreach (object[] objects in objeclist)
		    {
		        result.Rows.Add(objects);
		    }
            result.Rows.Add(new object[] { "jesien", "warzywo", "nie", "nie", "nie", "tak", "nie", ":sadzarka" });
            result.Rows.Add(new object[] { "jesien", "warzywo", "nie", "nie", "nie", "tak", "tak", ":rozrzutnik" });
            result.Rows.Add(new object[] { "jesien", "warzywo", "nie", "nie", "nie", "nie", "tak", "" });

            result.Rows.Add(new object[] { "wiosna", "warzywo", "tak", "nie", "nie", "tak", "tak", ":deszczownia" });
            result.Rows.Add(new object[] { "wiosna", "warzywo", "nie", "tak", "nie", "tak", "tak", ":rozrzutnik" });

            result.Rows.Add(new object[] { "lato", "warzywo", "tak", "nie", "nie", "tak", "nie", ":deszczownia" });
            result.Rows.Add(new object[] { "lato", "warzywo", "nie", "nie", "tak", "tak", "nie", ":kopaczka" });
            result.Rows.Add(new object[] { "lato", "warzywo", "nie", "nie", "tak", "nie", "nie", ":plug" });
            result.Rows.Add(new object[] { "lato", "warzywo", "nie", "nie", "nie", "tak", "nie", ":brona" });
            //result.Rows.Add(new object[] { "jesien", "zboze", "susza", "mineraly", "zbior", "zasiane", "zaorane", "bronowane", "" });
            result.Rows.Add(new object[] { "jesien", "zboze", "nie", "nie", "nie", "nie", "nie", ":plug" });
            result.Rows.Add(new object[] { "jesien", "zboze", "nie", "nie", "nie", "tak", "nie", ":brona" });
            result.Rows.Add(new object[] { "wiosna", "zboze", "tak", "nie", "nie", "tak", "tak", ":deszczownia" });
            result.Rows.Add(new object[] { "wiosna", "zboze", "nie", "nie", "nie", "tak", "tak", ":rozrzutnik" });
            result.Rows.Add(new object[] { "wiosna", "zboze", "nie", "nie", "nie", "tak", "tak", ":rozrzutnik" });

            result.Rows.Add(new object[] { "lato", "zboze", "tak", "nie", "nie", "tak", "nie", ":deszczownia" });
            result.Rows.Add(new object[] { "lato", "zboze", "nie", "nie", "tak", "tak", "tak", ":kombajn" });
            result.Rows.Add(new object[] { "lato", "zboze", "nie", "nie", "nie", "nie", "nie", "" });
			return result;	
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
            DataTable samples = getSampleData();

            DecisionTreeID3 id3 = new DecisionTreeID3();
            TreeNode root = id3.buildTree(samples, "maszyna", attributes);

            displayNode(root, "");

	        return TreeList;
	    }

	}
}
