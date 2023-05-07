using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using testFamilyTree.DAL;

namespace testFamilyTree
{
    public class FamilyMemberParent
    {
        [Required(ErrorMessage ="Every object must have an id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Every object must have a name")]
        [MinLength(1,ErrorMessage = "Every object name must contain at least 1 letter")]
        public string Name { get; set; }

        public int? Parent { get; set; }


        public FamilyMemberParent(int id, string name, int parent)
        {
            Id = id;
            Name = name;
            Parent = parent;
        }
        public FamilyMemberParent() { }


        public List<FamilyMemberChildren> familyMembersFunc(List<FamilyMemberParent> peopleList)
        {
            FamilyTreeDAL familyTree = new FamilyTreeDAL();

            return familyTree.createTree(peopleList);
        }

    }
}
