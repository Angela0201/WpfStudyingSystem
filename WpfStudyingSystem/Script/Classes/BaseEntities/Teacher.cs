using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Classes.BaseEntities
{
    public class Teacher : IUnique, ICompositNameHolder
    {
        private int id;
        private string firstName;
        private string lastName;

        public int Id => id;
        public string FirstName => firstName;
        public string LastName => lastName;
        public string FullName => FirstName+" "+LastName;

        public Teacher(int gIndex ,string gFirstName, string glastName)
        {
            id = gIndex;
            firstName = gFirstName;
            lastName = glastName;
        }
    }
}
