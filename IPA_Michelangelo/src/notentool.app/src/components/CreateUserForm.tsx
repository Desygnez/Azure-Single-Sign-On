import React, {useState} from 'react';
import {UserInfoInterface} from "../lib/UserInfo/UserInfo";
import {CreateUser} from "../lib/UserInfo/UserInformationService";

const defaultUser: UserInfoInterface = {
    id: "00000000-0000-0000-0000-000000000000",
    email: "",
    firstname: "",
    lastname: "",
    username: ""
}

function CreateUserForm() {
    const [user, setUser] = useState<UserInfoInterface>(defaultUser);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const name = e.target.name
        const value = e.target.value
        console.log(value)
        if (value === "INVALID" || value === "") return;
        setUser({...user, [name]: value})
    }
    const handleSubmit = async () => {
        if (user.username == "" || user.lastname == "" || user.firstname == "" || user.email == "") return;
        try {
            await CreateUser(user)
            window.location.reload()
        } catch (error) {
            console.log(error)
        }
    }
    return (
        <div>
            <div>
                <div className="my-2 dark:text-white">
                    <label>Email:</label>
                </div>
                <input
                    id="email"
                    name="email"
                    type="email"
                    className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg w-full p-1 dark:bg-slate-500 dark:border-gray-600 dark:text-white"
                    onChange={handleChange}
                    required
                />
            </div>
            <div className={"mt-2"}>
                <div className="my-2 dark:text-white">
                    <label>Firstname:</label>
                </div>
                <input
                    id="firstname"
                    name="firstname"
                    type="text"
                    onChange={handleChange}
                    className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg w-full p-1 dark:bg-slate-500 dark:border-gray-600 dark:text-white"
                    required
                />
            </div>
            <div className={"mt-2"}>
                <div className="my-2 dark:text-white">
                    <label>Lastname:</label>
                </div>
                <input
                    id="lastname"
                    name="lastname"
                    type="text"
                    onChange={handleChange}
                    className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg w-full p-1 dark:bg-slate-500 dark:border-gray-600 dark:text-white"
                    required
                />
            </div>
            <div className={"mt-2 "}>
                <div className="my-2 dark:text-white">
                    <label>Username:</label>
                </div>
                <input
                    id="username"
                    name="username"
                    type="text"
                    onChange={handleChange}
                    className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg w-full p-1 dark:bg-slate-500 dark:border-gray-600 dark:text-white"
                    required
                />
            </div>
            <button className={"dark:text-white"} onClick={handleSubmit}>Submit</button>
        </div>
    );
}

export default CreateUserForm;