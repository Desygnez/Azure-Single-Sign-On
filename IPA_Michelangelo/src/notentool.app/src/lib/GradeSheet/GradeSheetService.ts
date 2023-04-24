import {GradeSheet, UserAndSubjectInterface} from "./GradSheet";
import {BaseURL} from "../../statics";
import {CustomFetch, CustomFetchBody} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/AllGrades`;

export async function GetGradesByUserId(
    id: string,
): Promise<GradeSheet> {
    return await CustomFetch(`${baseURL}/GetByUserID/${id}`)
}

export async function GetGradesByUserIdAndSubjectId(
    model: UserAndSubjectInterface,
): Promise<GradeSheet> {
    return await CustomFetchBody(`${baseURL}/GetByUserAndSubjectId`, model, "POST")
}


export async function DeleteGradesByGradeId(id: string): Promise<void> {
    var sessionItem: any = sessionStorage.getItem('aadToken');
    var secret = JSON.parse(sessionItem).secret
    await fetch(`${baseURL}/ByGrade/${id}`, {
        method: "DELETE",
        credentials: "include",
        mode: "cors",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${secret}`
        },
    });
}