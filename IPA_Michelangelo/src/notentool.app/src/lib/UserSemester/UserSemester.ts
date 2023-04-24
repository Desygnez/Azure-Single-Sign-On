import {UserInfoInterface} from "../UserInfo/UserInfo";
import {SemesterInterface} from "../Semester/Semester";

export interface UserSemesterInterface {
    id: string
    userInfo_id: string
    userInformation: UserInfoInterface
    semester_id: string
    semester: SemesterInterface
}

export interface UserSemesterPostInterface {
    userInfo_id: string
    semester_id: string
}