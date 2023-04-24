import React, {useEffect, useState} from 'react';
import {GetAllSemesters} from "../lib/Semester/SemesterService";
import {SemesterInterface} from "../lib/Semester/Semester";
import {UserSemesterInterface, UserSemesterPostInterface} from "../lib/UserSemester/UserSemester";
import {CreateUserSemester, GetUserSemesterByUserId} from "../lib/UserSemester/UserSemesterService";
import {SemesterParams} from "../pages/SemesterSelection";

const defaultModel: UserSemesterPostInterface = {semester_id: "", userInfo_id: ""};

function CreateSemesterModal({userId}: SemesterParams) {
    const [semesters, setSemesters] = useState<SemesterInterface[]>([]);
    const [showModal, setShowModal] = useState<boolean>(false);
    const [createUserSemester, setCreateUserSemester] = useState<UserSemesterPostInterface>(defaultModel);

    useEffect(() => {
        const loadSemesters = async () => {
            if (typeof userId != "string") return;
            const allSemester: SemesterInterface[] = await GetAllSemesters();
            setSemesters(allSemester)
            const gotSemester: UserSemesterInterface[] = await GetUserSemesterByUserId(userId);
            let remainingSemester: SemesterInterface[] = []
            allSemester.forEach(function (semester) {
                if (!gotSemester.find(e => e.semester_id == semester.id)) remainingSemester.push(semester)
            })
            setSemesters(remainingSemester)
            setCreateUserSemester({...createUserSemester, semester_id: remainingSemester[0].id})
        }
        loadSemesters()
    }, [showModal]);

    const handleChange = (e: { target: { name: string; value: string; }; }) => {
        const name = e.target.name
        const value = e.target.value
        if (value === "INVALID" || value === "") return;
        setCreateUserSemester({...createUserSemester, [name]: value})
    }
    const handleEnter = (event: React.KeyboardEvent<HTMLDivElement>) => {
        if (event.key === "Enter") {
            if (showModal) handleSubmit()
            setShowModal(false)
        }
        if (event.key == "Escape") setShowModal(false)
    }


    const handleSubmit = async () => {
        try {
            if (userId == undefined) return
            console.log(createUserSemester)
            await CreateUserSemester({
                ...createUserSemester,
                userInfo_id: userId
            })
            setShowModal(false)
            window.location.reload();
        } catch (error) {
            console.log(error)
        }
    }

    return (
        <div tabIndex={1} onKeyDown={(e) => handleEnter(e)}>
            <button type="button"
                    onClick={() => setShowModal(true)}
                    className="text-white h-12 bg-gradient-to-br from-purple-600 to-blue-500 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:focus:ring-blue-800 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">
                Add
            </button>
            {showModal ? (
                <>
                    <div
                        className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none"
                    >
                        <div className="relative w-auto my-6 mx-auto max-w-3xl">
                            <div
                                className="border-0 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none">
                                <div
                                    className="flex items-start justify-between p-5 border-b border-solid border-slate-200 rounded-t">
                                    <h3 className="text-3xl font-semibold">
                                        Create Semester
                                    </h3>
                                    <button
                                        className="p-1 ml-auto bg-transparent border-0 text-black opacity-5 float-right text-3xl leading-none font-semibold outline-none focus:outline-none"
                                        onClick={() => setShowModal(false)}
                                    >
                                    </button>
                                </div>
                                <div className="relative p-6 flex-auto">
                                    <div className="col-start-5 col-end-9">
                                        <div className="my-3">
                                            <label>Semester</label>
                                        </div>
                                        <select id="semester_id"
                                                name="semester_id"
                                                className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                                onChange={handleChange}
                                                required
                                        >
                                            {
                                                semesters.map((semesterItter) => (
                                                    <option key={semesterItter.id}
                                                            value={semesterItter.id}>{semesterItter.semester}</option>
                                                ))
                                            }
                                        </select>

                                    </div>
                                </div>
                                <div
                                    className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                                    <button
                                        className="text-red-500 background-transparent font-bold uppercase px-6 py-2 text-sm outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                                        type="button"
                                        onClick={() => setShowModal(false)}
                                    >
                                        Close
                                    </button>
                                    <button
                                        className="bg-emerald-500 text-white active:bg-emerald-600 font-bold uppercase text-sm px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                                        onClick={handleSubmit}>
                                        Save Semester
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
                </>
            ) : null}
        </div>
    );
}

export default CreateSemesterModal;