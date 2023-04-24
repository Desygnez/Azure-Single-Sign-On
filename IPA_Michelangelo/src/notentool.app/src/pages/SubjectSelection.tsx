import {useNavigate, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {DeleteSubjectUser, GetSubjectUserByUserId} from "../lib/SubjectUser/SubjectUserService";
import {SubjectUserInterface} from "../lib/SubjectUser/SubjectUser";
import {Headers} from "../components/Headers";
import CreateSubjectModal from "../components/CreateSubjectModal";
import {SemesterInterface} from "../lib/Semester/Semester";
import {GetSemesterById} from "../lib/Semester/SemesterService";
import BackButton from "../components/BackButton";
import DeletionWarning from "../components/DeletionWarning";

export type SubjectParams = {
    userId: string | undefined;
    semesterId: string | undefined;
}


export default function SubjectSelection(): JSX.Element {
    const navigate = useNavigate();
    let {userId, semesterId} = useParams<SubjectParams>()
    const [subjectUser, setSubjectUser] = useState<SubjectUserInterface[]>([]);
    const [semester, setSemester] = useState<number>(0);
    const [reload, setReload] = useState<boolean>(false);
    const [deleteId, setDeleteId] = useState<string>("");

    useEffect(() => {
        if(reload) setReload(false)
        const loadUserSubject = async () => {
            if (semesterId === "" || typeof semesterId != "string" || typeof userId != "string") return;
            const resp: SubjectUserInterface[] = await GetSubjectUserByUserId(
                userId,
            );
            let subjectUserArray: SubjectUserInterface[] = [];
            resp.forEach(function (item) {
                if (item.semester_id === semesterId) {
                    subjectUserArray.push(item)
                }
            })
            setSubjectUser(subjectUserArray)
        };
        const loadSemester = async () => {
            if (semesterId === "" || typeof semesterId != "string") return;
            const resp: SemesterInterface = await GetSemesterById(semesterId);
            setSemester(resp.semester)
        }
        loadUserSubject();
        loadSemester();

    }, [reload]);

    useEffect(() => {
        if(deleteId == "") return
        const deleteSubject = async () => {
            await handleDelete(deleteId)
            setDeleteId("")
        }
        deleteSubject()
    }, [deleteId])

    const handleRedirect = async (subject: SubjectUserInterface) => {
        navigate(`/grades/${userId}/${semesterId}/${subject.subject_id}`);
    };

    document.title = "SubjectSelection | KPMG Notentool";

    const handleDelete = async (id: string): Promise<void> => {
        try {
            await DeleteSubjectUser(id)
            setReload(true)
        } catch (error) {
            console.log(error)
        }
    };

    return (
        <div>
            <Headers
                title={`Subjects in ${semester}. Semester`}
            />
            <div className="grid grid-cols-10 w-full mt-5">
                <div className={"inline-flex col-start-2 mr-12"}>
                    <BackButton/>
                    <CreateSubjectModal userId={userId} semesterId={semesterId}/>
                </div>
            </div>

            <div className="grid-cols-5 overflow-x-auto relative sm:rounded-lg">
                <table className="w-4/5 mx-auto text-sm text-left text-gray-500 dark:text-gray-400">
                    <thead className="text-m text-gray-700 bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th scope="col" className="py-3 px-6">
                            Subject
                        </th>
                        <th scope="col" className="py-3 px-6">
                            School
                        </th>
                        <th scope="col" className="py-3 px-6">
                            Delete
                        </th>
                    </tr>
                    </thead>
                    <tbody>
                    {subjectUser.map((subjectUser) => (
                        <tr
                            key={subjectUser.id}
                            className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                        >
                            <th
                                scope="row"
                                className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                                onClick={(e: any) => handleRedirect(subjectUser)}
                            >
                                {subjectUser.subject.subject}
                            </th>
                            <td className="py-4 px-6" onClick={(e: any) => handleRedirect(subjectUser)}>{subjectUser.school.school}</td>
                            <th
                                scope="row"
                                className="py-4 px-6 top-3 right-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center"
                            >
                                <DeletionWarning setDeleteId={setDeleteId} id={subjectUser.id}/>
                            </th>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
