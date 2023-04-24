import {BaseURL} from "../../statics";
import {SemesterInterface} from "./Semester";
import {CustomFetch} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/Semester`;


export async function GetAllSemesters(): Promise<SemesterInterface[]> {

    return await CustomFetch(`${baseURL}`)

}

export async function GetSemesterById(id: string): Promise<SemesterInterface> {

    return await CustomFetch(`${baseURL}/${id}`)

}
