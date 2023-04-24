import {GradeInterface, GradePostInterface} from "./Grade";

export interface GradeErrors {
    grade: string;
    date: string;
    comment: string;
    subject_id: string;
    semester_id: string;
    weight: string;
    isValid: boolean
}

export function ValidateGrade(grade: GradePostInterface | GradeInterface): GradeErrors {
    const gradeError: GradeErrors = {
        grade: "",
        date: "",
        comment: "",
        subject_id: "",
        semester_id: "",
        weight: "",
        isValid: false
    }
    if (grade.grade > 6) gradeError.grade = "The grade cannot be greater than 6"
    if (grade.grade < 1) gradeError.grade = "The grade cannot be smaller than 1!"
    if (grade.subject_id == "") gradeError.subject_id = "A grade must have a subject!"
    if (grade.semester_id == "") gradeError.semester_id = "A grade must have a semester!"
    if (grade.weight <= 0) gradeError.weight = "A grade must have a weight!"

    if (gradeError.grade == "" && gradeError.subject_id == "" && gradeError.date == "" && gradeError.semester_id == "" && gradeError.weight == "")
        gradeError.isValid = true

    return gradeError
}