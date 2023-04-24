import React from 'react';
import {UserInfoInterface} from "../lib/UserInfo/UserInfo";
import {useNavigate} from "react-router-dom";

type UserParams = {
    users: UserInfoInterface[]
}

function UserTable({users}: UserParams) {
    const navigate = useNavigate();
    return (
        <table className="w-4/5 text-sm mx-auto text-left text-gray-500 dark:text-gray-400">
            <thead className="text-xs text-gray-700 bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
                <th scope="col" className="py-3 px-6">
                    Username
                </th>
                <th scope="col" className="py-3 px-6">
                    Firstname
                </th>
                <th scope="col" className="py-3 px-6">
                    Lastname
                </th>
                <th scope="col" className="py-3 px-6">
                    Email
                </th>
            </tr>
            </thead>
            <tbody>
            {users.map((user) =>
                <tr
                    key={user.id}
                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                    onClick={() => navigate(`/dashboard/${user.id}`)}
                >
                    <th
                        scope="row"
                        className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                    >
                        {user.username}
                    </th>
                    <th
                        scope="row"
                        className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                    >
                        {user.firstname}
                    </th>
                    <th
                        scope="row"
                        className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                    >
                        {user.lastname}
                    </th>
                    <th
                        scope="row"
                        className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                    >
                        {user.email}
                    </th>

                </tr>
            )}
            </tbody>
        </table>
    );
}

export default UserTable;