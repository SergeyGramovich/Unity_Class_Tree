using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Unity_Class_Tree
{
    [DataContractAttribute]
    class DataClass
    {
        [DataMember]
        public string className { get; set; }
        [DataMember]
        public string classDescription { get; set; }

        // Right TextBoxes

        [DataMember]
        public string RightA { get; set; }
        [DataMember]
        public string RightStaticA { get; set; }
        [DataMember]
        public string RightConstr { get; set; }
        [DataMember]
        public string RightMeth { get; set; }
        [DataMember]
        public string RightStaticMeth { get; set; }
        [DataMember]
        public string RightMessage { get; set; }

        // Left TextBoxes

        [DataMember]
        public string LeftA { get; set; }
        [DataMember]
        public string LeftMeth { get; set; }
        [DataMember]
        public string LeftStaticMeth { get; set; }

    }
}
