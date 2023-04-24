import {SubjectInterface} from "../Subject/Subject";
import {SemesterInterface} from "../Semester/Semester";

export interface GradeInterface {
    id: string
    grade: number
    date: Date
    comment: string;
    weight: number;
    subject_id: string
    subjects: SubjectInterface | undefined
    semester_id: string
    semester: SemesterInterface | undefined
}


export interface GradePostInterface {
    grade: number;
    date: Date;
    comment: string;
    subject_id: string;
    semester_id: string;
    weight: number;
}
