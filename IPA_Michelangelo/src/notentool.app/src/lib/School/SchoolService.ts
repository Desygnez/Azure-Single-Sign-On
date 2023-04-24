import {BaseURL} from "../../statics";
import {SchoolInterface} from "./School";
import {CustomFetch} from "../UserInfo/CustomFetch";

const baseURL = `${BaseURL}/School`


export async function GetSchools(): Promise<SchoolInterface[]> {
    return await CustomFetch(`${baseURL}`)

}