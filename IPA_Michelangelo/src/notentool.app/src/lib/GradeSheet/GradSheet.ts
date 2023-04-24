import {GradeInterface} from "../Grade/Grade";
import {UserInfoInterface} from "../UserInfo/UserInfo";

export interface GradeSheetInterface {
    id: string;
    userInfo_id: string;
    userInformations: UserInfoInterface;
    grade_id: string;
    grades: GradeInterface;
}

export type GradeSheet = Array<GradeSheetInterface>;

export interface UserAndSubjectInterface {
    userId: string | undefined;
    subjectId: string | undefined;
    semesterId: string | undefined;
}
