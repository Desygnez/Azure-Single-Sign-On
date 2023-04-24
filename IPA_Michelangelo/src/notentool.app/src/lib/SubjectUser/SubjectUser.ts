import {SubjectInterface} from '../Subject/Subject';
import {UserInfoInterface} from '../UserInfo/UserInfo';
import {SchoolInterface} from "../School/School";
import {SemesterInterface} from "../Semester/Semester";

export interface SubjectUserInterface {
    id: string;
    subject_id: string;
    subject: SubjectInterface;
    userInfo_id: string;
    userInformation: UserInfoInterface;
    school_id: string;
    school: SchoolInterface;
    semester_id: string;
    semester: SemesterInterface
}

export type SubjectUsers = Array<SubjectUserInterface>;

export interface SubjectUserPostInterface {
    subject_id: string;
    userInfo_id: string;
    school_id: string;
    semester_id: string;
}
