using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Classes.BaseEntities
{
    public abstract class Assignment: IUnique, ISimpleNameHolder
    {
        private int id;
        private string name;

        private DateTime date;
        private string description;
        private int type;

        public int Id => id;
        public string Name => name;
        public DateTime Date => date;
        public string Description => description;
        public int Type => type;

        public Assignment(int gId, string gName, DateTime gDate, string gDescription, int gType)
        {
            id = gId;
            name = gName;
            date = gDate;
            description = gDescription;
            type = gType;
        }

        public abstract int GetResults(int points);
    }
}
