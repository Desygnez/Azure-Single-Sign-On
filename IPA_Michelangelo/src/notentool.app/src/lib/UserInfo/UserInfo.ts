import {RoleInterface} from "../Role/Role";

export interface UserInfoInterface {
    id: string;
    firstname: string;
    lastname: string;
    username: string;
    firstLogin: boolean;
    email: string;
    password: string;
    role_id: string;
    roles: RoleInterface;
}


export interface User {
    id: string;
    firstname: string;
    lastname: string;
    email: string;
    username: string;
    isVocationalTrainer: boolean;
    apprentices: string[]
}


export interface TrainerApprentice {
    id: string;
    trainerId : string;
    apprenticeId : string;

}