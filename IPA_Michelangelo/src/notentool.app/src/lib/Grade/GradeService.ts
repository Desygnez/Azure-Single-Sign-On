import {GradeInterface, GradePostInterface} from "./Grade";
import {BaseURL} from "../../statics";
import {CustomFetch, CustomFetchBody} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/Grade`;

export async function GetGradeById(id: string): Promise<GradeInterface> {
    return await CustomFetch(`${baseURL}/${id}`)
}

export async function UpdateGrade(model: GradeInterface): Promise<void> {
    return await CustomFetchBody(`${baseURL}`, model, "PUT")
}


export async function CreateNewGrade(model: GradePostInterface, userId: string): Promise<any> {

    return await CustomFetchBody(`${baseURL}/${userId}`, model, "POST")
}