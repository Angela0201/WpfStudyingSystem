The project is a WPF studying system. 
The main screen of the application is used to manage courses, students, assignments and grading. Teachers can create courses, add students to the course, create assignments and grade them for each student.
It also record results, filters data, and export information to CSV.

The MAIN WINDOW is divided into several functional areas:
  1. Courses list (left)
  2. Teacher name (center, top)
  3. Students list (center, middle)
  4. Assignments list (center, bottom)
  5. Information panels about students and assignments (right)
  6. Grades/Results panel (right, bottom)
  7. Control buttons:
     1) Courses and assignments: Create/Delete/Redact;
     2) Students: Add/Delete/Sort;
     3) Grades: Set points
     4) To see the results of all students for the selected assessment method: Filter
     5) Export CSV

WORKING WITH COURSES:
     Create a course:
  1. Click Create in the Courses section.
  2. Enter: Course nameTeacher full name (first name + last name)
  3. Click OK to save the course.

     Edit a Course:
  1. Select a course from the courses list.
  2. Click Redact.
  3. Change the course name or teacher name.
  4. Click OK to apply changes.

     Delete a Course:
  1. Select a course.
  2. Click Delete.

     Changing the selected course automatically reloads students and assignments.

WORKING WITH STUDENTS:
     Adding:
  1. Add a Student to a Course
  2. Select a course.
  3. Click Add in the Students section.
  4. Enter: First name, last name, age.
  5. Click OK.
     The student will be added to the selected course.

     Deleting a Student from a Course:
  1. Select a course.
  2. Select a student.
  3. Click Delete.

WOWKING WITH ASSIGNMENTS:
     Create an Assignment:
  1. Select a course.
  2. Click Create in the Assignments section.
  3. Enter: Assignment name, due date, assignment type (Grade/Credit/EAP), description (optional)
  4. Click OK.
     The assignment will be created and linked to the selected course.

     Edit an Assignment:
  1. Select a course.
  2. Select an assignment.
  3. Click Redact.
  4. Modify the assignment data.
  5. Click OK.

     Delete an Assignment:
  1. Select a course.
  2. Select an assignment.
  3. Click Delete.

WORKING WITH GRADES/RESULTS:
     Loading Grades:
  1. Select a course.
  2. Click Filter to load grade data.
     There must be at least one student and at least one assignment for the selected assessment method, otherwise a corresponding message will be displayed.

     Grade lists are cleared when switching between courses.

WORKING WITH FILTER grades by assesment type:
  Each click on Filter cycles through: all assignments, grade assignments, credit assignments, EAP assignments

SETTING POINTS: 
  1. Select a grade record in the Grades/Results list.
  2. Click Set points.
  3. Enter points according to assignment type:
     1. Grade and EAP: integer value from 0 to 100
     2. Credit: 0 = Failed; 1 = Passed
  4. Click OK.

Export to CSV: Click Export, choose a file location, save the file, the exported CSV contains structured data for: courses, teachers, students, assignments, assignment dependencies, assignment statistics

DISPLAY DATA:
  1. Assignment Information:
     When an assignment is selected, its details (due date, type, description) are shown in the Assignment info panel.
  2. Student Information:
     When a student is added, their details (full name and age) are displayed in the student info panel.
