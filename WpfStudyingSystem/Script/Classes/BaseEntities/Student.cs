using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Classes.BaseEntities
{
    public class Student: IUnique, ICompositNameHolder, IAged
    {
        private int id;
        private string firstName;
        private string lastName;
        private int age;

        public int Id => id;
        public string FirstName => firstName;
        public string LastName => lastName;
        public string FullName => FirstName + " " + LastName;
        public int Age => age;

        public Student(int gIndex, string gFirstName, string glastName, int gAge)
        {
            id = gIndex;
            firstName = gFirstName;
            lastName = glastName;
            age = gAge;
        }
    }
}
