import {BaseURL} from "../../statics";
import {SubjectInterface} from "./Subject";
import {CustomFetch} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/Subject`;


export async function GetAllSubjects(): Promise<SubjectInterface[]> {
    return await CustomFetch(`${baseURL}`)
}


export async function GetSubjectById(id: string): Promise<SubjectInterface> {
    return await CustomFetch(`${baseURL}/${id}`)
}