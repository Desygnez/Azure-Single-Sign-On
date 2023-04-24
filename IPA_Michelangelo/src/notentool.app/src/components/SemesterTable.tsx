import React, {useEffect, useState} from 'react';
import {DeleteUserSemester, GetUserSemesterByUserId} from "../lib/UserSemester/UserSemesterService";
import {useNavigate} from "react-router-dom";
import {UserSemesterInterface} from "../lib/UserSemester/UserSemester";
import {SemesterParams} from "../pages/SemesterSelection";


function SemesterTable({userId}: SemesterParams) {
    const navigate = useNavigate();
    const [semester, setSemester] = useState<UserSemesterInterface[]>([]);
    const [reload, setReload] = useState<boolean>(true);

    useEffect(() => {
        if (!reload || typeof userId !== "string") return;
        const loadSemester = async () => {
            const resp = await GetUserSemesterByUserId(userId)
            setSemester(resp)
            setReload(false)
        }
        loadSemester()
    }, [reload]);


    async function handleDelete(id: string) {
        try {
            await DeleteUserSemester(id)
            setReload(true)
        } catch (error) {
            console.log(error)
        }
    }

    function handleRedirect(id: string) {
        navigate(`/subjects/${userId}/${id}`)
    }

    return (
        <div>
            <table className="w-4/5 text-sm mx-auto text-left text-gray-500 dark:text-gray-400">
                <thead className="text-xs text-gray-700 bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                    <th scope="col" className="py-3 px-6">
                        Semester
                    </th>
                    <th scope="col" className="py-3 px-6">
                        Operations
                    </th>
                </tr>
                </thead>
                <tbody>
                {semester.map((item) =>
                    <tr
                        key={item.id}
                        className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                    >
                        <th
                            scope="row"
                            className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                            onClick={() => handleRedirect(item.semester_id)}
                        >
                            {item.semester.semester}. Semester
                        </th>
                        <th
                            scope="row"
                            className="py-4 px-6 top-3 right-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center"
                            onClick={() => handleDelete(item.id)}
                        >

                            <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6"
                                 fill="none"
                                 viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                                <path strokeLinecap="round" strokeLinejoin="round"
                                      d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"/>
                            </svg>
                        </th>
                    </tr>
                )}
                </tbody>
            </table>
        </div>
    );
}

export default SemesterTable;