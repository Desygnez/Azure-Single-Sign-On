import {BaseURL} from "../../statics";
import {UserSemesterInterface, UserSemesterPostInterface} from "./UserSemester";
import {CustomFetch, CustomFetchBody} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/UserSemester`;

export async function GetUserSemesterByUserId(id: string): Promise<UserSemesterInterface[]> {
    return await CustomFetch(`${baseURL}/GetByUserId/${id}`)

}

export async function CreateUserSemester(model: UserSemesterPostInterface): Promise<void> {
    return await CustomFetchBody(`${baseURL}`, model, "POST")

}

export async function DeleteUserSemester(id: string): Promise<void> {
    var sessionItem: any = sessionStorage.getItem('aadToken');
    var secret = JSON.parse(sessionItem).secret
    await fetch(`${baseURL}/${id}`, {
        method: "DELETE",
        credentials: "include",
        mode: "cors",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${secret}`
        },
    })
}
