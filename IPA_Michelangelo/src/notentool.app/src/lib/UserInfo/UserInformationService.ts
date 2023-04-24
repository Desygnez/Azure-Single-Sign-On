import {BaseURL} from "../../statics";
import {TrainerApprentice, User, UserInfoInterface} from "./UserInfo";
import {CustomFetch, CustomFetchBody} from "./CustomFetch";

/**
 * Get user from URL, will redirect the page
 */

const baseURL = `${BaseURL}/UserInformation`;

export async function GetCurrentUser(): Promise<User> {

    return await CustomFetch(`${baseURL}/currentUser`)

}

export async function GetUsers(): Promise<UserInfoInterface[]> {

    return await CustomFetch(`${baseURL}/apprentices`)
}

export async function CreateUser(user: UserInfoInterface): Promise<void> {
    return await CustomFetchBody(`${baseURL}`, user, "POST")
}

export async function CreateTrainerApprentice(user: TrainerApprentice): Promise<void> {
    return await CustomFetchBody(`${baseURL}/trainerlist`, user, "POST")
}

