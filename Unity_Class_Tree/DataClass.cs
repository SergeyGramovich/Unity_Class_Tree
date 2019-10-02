using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Unity_Class_Tree
{
    [DataContractAttribute]
    class DataClass
    {
        [DataMember]
        public string className { get; set; }
        [DataMember]
        public string classDescription { get; set; }
        [DataMember]
        public Point canvasPointClass { get; set; }

        // Right RichTextBoxes

        [DataMember]
        public bool rightADataClassCheckBox { get; set; }
        [DataMember]
        public List<string> rightAInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> rightAInlineText = new List<string>();
        [DataMember]
        public List<int> rightAInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> rightAInlineTextColor = new List<string>();

        [DataMember]
        public bool rightStaticADataClassCheckBox { get; set; }
        [DataMember]
        public List<string> rightStaticAInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> rightStaticAInlineText = new List<string>();
        [DataMember]
        public List<int> rightStaticAInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> rightStaticAInlineTextColor = new List<string>();

        [DataMember]
        public bool rightConstrDataClassCheckBox { get; set; }
        [DataMember]
        public List<string> rightConstrInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> rightConstrInlineText = new List<string>();
        [DataMember]
        public List<int> rightConstrInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> rightConstrInlineTextColor = new List<string>();

        [DataMember]
        public bool rightMethDataClassCheckBox { get; set; }
        [DataMember]
        public List<string> rightMethInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> rightMethInlineText = new List<string>();
        [DataMember]
        public List<int> rightMethInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> rightMethInlineTextColor = new List<string>();

        [DataMember]
        public bool rightStaticMethDataClassCheckBox { get; set; }
        [DataMember]
        public List<string> rightStaticMethInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> rightStaticMethInlineText = new List<string>();
        [DataMember]
        public List<int> rightStaticMethInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> rightStaticMethInlineTextColor = new List<string>();

        [DataMember]
        public bool rightMessageDataClassCheckBox { get; set; }
        [DataMember]
        public List<string> rightMessageInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> rightMessageInlineText = new List<string>();
        [DataMember]
        public List<int> rightMessageInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> rightMessageInlineTextColor = new List<string>();

        // Left RichTextBoxes

        [DataMember]
        public bool leftADataClassCheckBox { get; set; }
        [DataMember]
        public List<string> leftAInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> leftAInlineText = new List<string>();
        [DataMember]
        public List<int> leftAInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> leftAInlineTextColor = new List<string>();

        [DataMember]
        public bool leftMethDataClassCheckBox { get; set; }
        [DataMember]
        public List<string> leftMethInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> leftMethInlineText = new List<string>();
        [DataMember]
        public List<int> leftMethInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> leftMethInlineTextColor = new List<string>();

        [DataMember]
        public bool leftStaticMethDataClassCheckBox { get; set; }
        [DataMember]
        public List<string> leftStaticMethInlineTextFontWeight = new List<string>();
        [DataMember]
        public List<string> leftStaticMethInlineText = new List<string>();
        [DataMember]
        public List<int> leftStaticMethInlineTextFontSize = new List<int>();
        [DataMember]
        public List<string> leftStaticMethInlineTextColor = new List<string>();

    }
}
