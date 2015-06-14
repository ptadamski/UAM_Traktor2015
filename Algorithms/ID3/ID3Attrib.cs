﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Algorithms
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
}
