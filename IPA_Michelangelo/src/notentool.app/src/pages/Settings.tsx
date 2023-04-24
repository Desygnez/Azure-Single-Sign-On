import React, {useEffect, useState} from 'react';
import {Headers, SubHeader} from "../components/Headers";
import CreateUserForm from "../components/CreateUserForm";
import AssignTrainerForm from "../components/AssignTrainerForm";
import {GetCurrentUser} from "../lib/UserInfo/UserInformationService";
import {User} from "../lib/UserInfo/UserInfo";
import {useRedirectToDashboard} from "../lib/services/Redirect";

const defaultUser: User = {
    apprentices: [],
    email: "",
    firstname: "",
    id: "",
    isVocationalTrainer: false,
    lastname: "",
    username: ""
}
function Settings() {
    useRedirectToDashboard()
    const [user, setUser] = useState<User>(defaultUser);
    useEffect(() => {
        const loadUser = async () => {
            const user = await GetCurrentUser();
            setUser(user)
        }
        loadUser()
    }, []);

    return (
        <div>
            <Headers title={"Settings"}/>
            {
                user.isVocationalTrainer &&
                <div>
                    <SubHeader title={"Create User"}/>
                    <div className={"flex justify-center items-center h-scree"}>
                        <CreateUserForm/>
                    </div>
                    <SubHeader title={"Assign Trainer to Apprentice"}/>
                    <div className={"flex justify-center items-center h-scree"}>
                        <AssignTrainerForm/>
                    </div>
                </div>
            }

        </div>
    );
}

export default Settings;