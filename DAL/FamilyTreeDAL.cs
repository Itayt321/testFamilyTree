using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testFamilyTree.DAL
{
    public class FamilyTreeDAL
    {
        public List<FamilyMemberChildren> createTree(List<FamilyMemberParent> peopleList)
        {       
            List<FamilyMemberChildren> list = new List<FamilyMemberChildren>();
            FamilyMemberChildren mainTree = new FamilyMemberChildren();
            mainTree.Id = peopleList[0].Id;
            mainTree.Name = peopleList[0].Name;
            list.Add(mainTree);
            int i = 1;

            while(i < peopleList.Count)
            {
                FamilyMemberChildren Node = list.First();
                if (peopleList[i].Parent==Node.Id)
                {
                    FamilyMemberChildren newChild = new FamilyMemberChildren();
                    newChild.Id = peopleList[i].Id;
                    newChild.Name = peopleList[i].Name;
                    Node.Childs.Add(newChild);
                    list.Add(newChild);
                }         
                else
                {
                    i--;
                    list.RemoveAt(0);
                }
                i++;
            }

            List<FamilyMemberChildren> mainTreeList = new List<FamilyMemberChildren>() { mainTree };
            return mainTreeList;
        }
    }
}
