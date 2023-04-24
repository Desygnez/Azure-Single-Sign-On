import {SubjectUserPostInterface, SubjectUsers} from './SubjectUser';
import {BaseURL} from "../../statics";
import {CustomFetchBody} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/SubjectUser`;

export async function GetSubjectUserByUserId(id: string): Promise<SubjectUsers> {
    var sessionItem: any = sessionStorage.getItem('aadToken');
    var secret = JSON.parse(sessionItem).accessToken;
    const resp = await fetch(`${baseURL}/GetByUserId/${id}`, {
        credentials: "include",
        mode: "cors",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${secret}`
        },
    });

    if (resp.status == 404) {
        return [];
    }

    if (!resp.ok) {
        return Promise.reject(resp);
    }

    const data: SubjectUsers = await resp.json();
    return data;
}

export async function CreateSubjectUser(model: SubjectUserPostInterface): Promise<void> {
    return await CustomFetchBody(`${baseURL}`, model, "POST")
}

export async function DeleteSubjectUser(id: string) {
    var sessionItem: any = sessionStorage.getItem('aadToken');
    var secret = JSON.parse(sessionItem).accessToken;
    await fetch(`${baseURL}/${id}`, {
        method: "DELETE",
        credentials: "include",
        mode: "cors",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${secret}`
        }
    })
}