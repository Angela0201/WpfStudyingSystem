using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDatabaseSetter
    {
        void SetHunman(Human human);
        void SetTeacher(Human teacher, int humanId = -1);
        void SetStudent(Human student, int humanId = -1);
        void SetAssignment(Assignment assignment, int courseId);
        void SetCourse(Course course);

        void RemoveHunman(int humanId);
        void RemoveTeacher(int teacherId);
        void RemoveStudent(int studentId);
        void RemoveAssignment(int assignmentId);
        void RemoveCourse(int courceId);

        void ChangeStudentAssignmentPoints(int studentId, int assignmentId, int points);

        void AssignStudentToCourse(int studentId, int courseId);
        void AssignTeacherToCourse(int teacherId, int courseId);

        void RemoveStudentFromCourse(int studentId, int courseId);
        void RemoveTeacherFromCourse(int teacherId, int courseId);
        void UpdateCourseName(int courseId, string newName);
        void UpdateAssignment(Assignment assignment);
    }
}
