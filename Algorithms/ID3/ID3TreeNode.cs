using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Algorithms
{

    public class ID3TreeNode
    {
        private ArrayList mChilds = null;
        private ID3Attrib mAttribute;

        public ID3TreeNode(ID3Attrib attribute)
        {
            if (attribute != null && attribute.values != null)
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

        public void AddTreeNode(ID3TreeNode treeNode, string ValueName)
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

        public ID3TreeNode getChild(int index)
        {
            return (ID3TreeNode)mChilds[index];
        }

        public ID3Attrib attribute
        {
            get
            {
                return mAttribute;
            }
        }
        public ID3TreeNode getChildByBranchName(string branchName)
        {
            int index = mAttribute.indexValue(branchName);
            return (ID3TreeNode)mChilds[index];
        }

        public string decide(IList<ID3Attrib> attribs)
        {
            var index = attribs.IndexOf(attribute);
            var child = getChildByBranchName(attribs[index].AttributeName); 
            attribs.RemoveAt(index);
            return attribs.Count > 0 ? child.decide(attribs) : attribute.AttributeName;
        }      
    }
}
