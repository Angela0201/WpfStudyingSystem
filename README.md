## Project Overview

The project is a **WPF studying system**.

The main screen of the application is used to manage **courses, students, assignments, and grading**.  
Teachers can create courses, add students to courses, create assignments, and grade them for each student.

The system also:
- records results
- filters data
- exports information to CSV

---

## Main Window Layout

The **Main Window** is divided into several functional areas:

1. **Courses list** (left)
2. **Teacher name** (center, top)
3. **Students list** (center, middle)
4. **Assignments list** (center, bottom)
5. **Information panels** about students and assignments (right)
6. **Grades / Results panel** (right, bottom)

### Control Buttons

- **Courses and assignments**: Create / Delete / Redact
- **Students**: Add / Delete / Sort
- **Grades**: Set points
- **Results filtering**: Filter by assessment method
- **Export**: CSV

---

## Working with Courses

### Create a Course
1. Click **Create** in the *Courses* section.
2. Enter:
   - Course name
   - Teacher full name (first name + last name)
3. Click **OK** to save the course.

---

### Edit a Course
1. Select a course from the courses list.
2. Click **Redact**.
3. Change the course name or teacher name.
4. Click **OK** to apply changes.

---

### Delete a Course
1. Select a course.
2. Click **Delete**.

> Changing the selected course automatically reloads students and assignments.

---

## Working with Students

### Add a Student to a Course
1. Select a course.
2. Click **Add** in the *Students* section.
3. Enter:
   - First name
   - Last name
   - Age
4. Click **OK**.

> The student will be added to the selected course.

---

### Delete a Student from a Course
1. Select a course.
2. Select a student.
3. Click **Delete**.

---

## Working with Assignments

### Create an Assignment
1. Select a course.
2. Click **Create** in the *Assignments* section.
3. Enter:
   - Assignment name
   - Due date
   - Assignment type (**Grade / Credit / EAP**)
   - Description (optional)
4. Click **OK**.

> The assignment will be created and linked to the selected course.

---

### Edit an Assignment
1. Select a course.
2. Select an assignment.
3. Click **Redact**.
4. Modify the assignment data.
5. Click **OK**.

---

### Delete an Assignment
1. Select a course.
2. Select an assignment.
3. Click **Delete**.

---

## Working with Grades / Results

### Loading Grades
1. Select a course.
2. Click **Filter** to load grade data.

Requirements:
- At least one student
- At least one assignment for the selected assessment method  

Otherwise, a corresponding message will be displayed.

>  When switching between courses, the grade list is automatically updated based on on the selected course.

---

### Filtering Grades by Assessment Type

Each click on **Filter** cycles through:
- All assignments
- Grade assignments
- Credit assignments
- EAP assignments

---

### Setting Points
1. Select a grade record in the *Grades / Results* list.
2. Click **Set points**.
3. Enter points according to assignment type:
   - **Grade / EAP**: integer value from `0` to `100`
   - **Credit**:
     - `0` — Failed
     - `1` — Passed
4. Click **OK**.

---

## Export to CSV

1. Click **Export**.
2. Choose a file location.
3. Save the file.

The exported CSV contains structured data for:
- courses
- teachers
- students
- assignments
- assignment dependencies
- assignment statistics

---

## Displayed Information

### Assignment Information
When an assignment is selected, its details are shown in the *Assignment Information* panel:
- due date
- assignment type
- description

---

### Student Information
When a student is selected, their details are shown in the *Student Information* panel:
- full name
- age
