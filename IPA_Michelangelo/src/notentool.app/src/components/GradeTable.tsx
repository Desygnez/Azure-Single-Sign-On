import React, {useEffect, useState} from 'react';
import {GradeSheetInterface, UserAndSubjectInterface} from "../lib/GradeSheet/GradSheet";
import {DeleteGradesByGradeId, GetGradesByUserIdAndSubjectId} from "../lib/GradeSheet/GradeSheetService";
import CalculateAverage from "../lib/services/AverageCalculator";
import EditGradeModal from "./EditGradeModal";
import {GradeInterface} from "../lib/Grade/Grade";
import DeletionWarning from './DeletionWarning';

type GradeTableProps = {
    gradeReq: UserAndSubjectInterface,
}

function GradeTable({gradeReq}: GradeTableProps) {
    const [grades, setGrades] = useState<GradeSheetInterface[]>([]);
    const [average, setAverage] = useState<number | string>(0);
    const [reload, setReload] = useState<boolean>(true);
    const [gradeRequest, setGradeRequest] = useState<UserAndSubjectInterface>(gradeReq);
    const [deleteId, setDeleteId] = useState<string>("");
    useEffect(() => {
        if (!reload) return;
        const loadGrades = async () => {
            const resp = await GetGradesByUserIdAndSubjectId(
                gradeRequest,
            );

            let gradeSheetList: GradeSheetInterface[] = [];
            resp.forEach(function (item) {
                console.log(gradeReq.semesterId)
                if (item.grades.semester_id === gradeReq.semesterId) {
                    gradeSheetList.push(item);
                }
            })
            setGrades(gradeSheetList);
            let gradeList: GradeInterface[] = [];
            gradeSheetList.forEach(function (item) {
                gradeList.push(item.grades);
            })

            setAverage(CalculateAverage(...gradeList))
            setReload(false)
        };


        loadGrades();
    }, [gradeReq, reload])

    useEffect(() => {
        if(deleteId == "") return
        const deleteGrade = async () => {
            await handleDelete(deleteId)
            setDeleteId("")
        }
        deleteGrade()
    }, [deleteId])

    const handleDelete = async (id: string) => {
        try {
            await DeleteGradesByGradeId(id)
            setReload(true);
        } catch (error) {
            console.log(error)
        }
    }
    return (
        <div>
            <table className="w-4/5 mx-auto text-sm text-left text-gray-500 dark:text-gray-400">
                <thead className="text-xs text-gray-700 bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                    <th scope="col" className="py-3 px-6">
                        Grade
                    </th>
                    <th scope="col" className="py-3 px-6">
                        Date
                    </th>
                    <th scope="col" className="py-3 px-6">
                        Comment
                    </th>
                    <th scope="col" className="py-3 px-6">
                        Weight
                    </th>
                    <th scope="col" className="py-3 px-6">
                        Actions
                    </th>
                </tr>
                </thead>
                <tbody>
                {grades.map((item) =>
                    <tr
                        key={item.id}
                        className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
                    >
                        <th
                            scope="row"
                            className="py-4 px-6 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                        >
                            {item.grades.grade}
                        </th>
                        <td className="py-4 px-6">{`${new Date(item.grades.date).getDate()}.${new Date(item.grades.date).getMonth() + 1}.${new Date(item.grades.date).getFullYear()}`}</td>
                        <td
                            className="py-4 px-6"
                        >
                            {item.grades.comment}
                        </td>
                        <td className={"py-4 px-6"}>
                            {item.grades.weight}
                        </td>
                        <th
                            scope="row"
                            className="py-4 px-6 cursor-pointer top-3 right-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center"
                        >

                            <DeletionWarning setDeleteId={setDeleteId}  id={item.grade_id} />
                        </th>
                        <th
                            scope="row"
                            className="py-4 px-6 top-3 cursor-pointer right-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center"
                        >
                            <EditGradeModal grade_id={item.grade_id}/>
                        </th>

                    </tr>
                )}
                </tbody>
                <tfoot className="text-xs text-gray-700 bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                    <th scope="col" className="py-2 px-6">Grade average:</th>
                    <th scope="col" className="py-2 px-6"></th>
                    <th scope="col" className="py-2 px-6"></th>
                    <th scope="col" className="py-2 px-6"></th>
                    <th scope="col" className="py-2 px-6">{average}</th>
                </tr>
                </tfoot>
            </table>
        </div>
    );
}

export default GradeTable;