using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testFamilyTree
{
    public class FamilyMemberChildren
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<FamilyMemberChildren> Childs { get; set; }

        public FamilyMemberChildren(int id, string name, List<FamilyMemberChildren> childs)
        {
            Id = id;
            Name = name;
            Childs = childs;
        }

        public FamilyMemberChildren() {
            Id = 0;
            Name = "Default Name";
            Childs = new List<FamilyMemberChildren>();
        }
    }
}
