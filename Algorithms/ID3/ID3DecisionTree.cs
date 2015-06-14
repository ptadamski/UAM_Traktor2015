using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//AUTHOR: Roosevelt dos Santos Júnior
using System.Windows.Controls;
using System.Windows.Documents;
using TraktorProj.Commons;
using System.IO;
using TraktorProj.Algorithms;

namespace TraktorProj.Algorithms
{
	
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

		private ID3TreeNode internalMountTree(DataTable samples, string targetAttribute, ID3Attrib[] attributes)
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

			ID3TreeNode root = new ID3TreeNode(bestAttribute);
			
			DataTable aSample = samples.Clone();
            if (bestAttribute.values == null)
            {
                return new ID3TreeNode(new ID3Attrib(getMostCommonValue(samples, targetAttribute)));	
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
                        return new ID3TreeNode(new ID3Attrib(getMostCommonValue(aSample, targetAttribute)));
                    }
                    else
                    {
                        DecisionTreeID3 dc3 = new DecisionTreeID3();
                        ID3TreeNode ChildNode = dc3.buildTree(aSample, targetAttribute, (ID3Attrib[])aAttributes.ToArray(typeof(ID3Attrib)));
                        root.AddTreeNode(ChildNode, value);
                    }
                }
            }

			return root;
		}

		public ID3TreeNode buildTree(DataTable samples, string targetAttribute, ID3Attrib[] attributes)
		{
			mSamples = samples;
			return internalMountTree(mSamples, targetAttribute, attributes);
		}
                      
    }
}
