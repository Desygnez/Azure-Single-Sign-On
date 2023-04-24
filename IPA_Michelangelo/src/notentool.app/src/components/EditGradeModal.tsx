import { useEffect, useState } from "react";
import {GradeInterface} from "../lib/Grade/Grade";
import {GetGradeById, UpdateGrade} from "../lib/Grade/GradeService";
import {GradeErrors, ValidateGrade} from "../lib/Grade/GradeValidator";

type EditGradeModelProp = {
    grade_id: string
}


const defaultModel: GradeInterface = {
    weight: 0,
    id: "", semester: undefined, subjects: undefined,
    comment: "", semester_id: "",
    grade: 0,
    date: new Date(),
    subject_id: "",
};
const errorDefault: GradeErrors = {
    comment: "",
    date: "",
    grade: "",
    isValid: false,
    semester_id: "",
    subject_id: "",
    weight: ""
};

function EditGradeModal({grade_id}: EditGradeModelProp) {

    const [showModal, setShowModal] = useState(false);
    const [editGradeModel, setEditGradeModel] = useState<GradeInterface>({...defaultModel,});
    const [errors, setErrors] = useState<GradeErrors>(errorDefault);

    useEffect(() => {
        const loadGrade = async () => {
            const resp: GradeInterface = await GetGradeById(grade_id);
            setEditGradeModel(resp)
        }
        loadGrade()
    }, [grade_id])

    const handleSubmit = async () => {
        try {
            const validated = ValidateGrade(editGradeModel)
            setErrors(validated)
            if (!validated.isValid) return
            await UpdateGrade({
                ...editGradeModel,
                grade: parseFloat(String(editGradeModel.grade)),
                comment: editGradeModel.comment == "" ? "No Comment" : editGradeModel.comment
            })
            setShowModal(false)
            window.location.reload()
        } catch (error) {
            console.log(error)
        }
    }

    const handleEnter = (event: React.KeyboardEvent<HTMLDivElement | HTMLInputElement>) => {
        if (event.key === "Enter") {
            if (showModal) handleSubmit()
            setShowModal(false)
        }
        if (event.key == "Escape") setShowModal(false)
    }

    const handleChange = (e: { target: { name: string; value: string; }; }) => {
        const name = e.target.name
        const value = e.target.value
        setEditGradeModel({...editGradeModel, [name]: value})
    }

    return (
        <div tabIndex={2} onKeyDown={(e) => handleEnter(e)}>
            <button type="button"
                    onClick={() => setShowModal(true)}>
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5}
                     stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round"
                          d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L6.832 19.82a4.5 4.5 0 01-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 011.13-1.897L16.863 4.487zm0 0L19.5 7.125"/>
                </svg>
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
                                        Edit
                                    </h3>
                                </div>
                                <div className="relative p-6 flex-auto font-normal">
                                    <div className="col-start-5 col-end-9">
                                        <div className="my-3">
                                            <label>Grade</label>
                                        </div>
                                        <input
                                            id="grade"
                                            name="grade"
                                            value={editGradeModel.grade}
                                            type="number"
                                            step={0.1}
                                            min="1"
                                            max="6"
                                            onChange={handleChange}
                                            className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg w-full p-1 dark:bg-slate-500 dark:border-gray-600 dark:text-white"
                                            required
                                        />
                                        {errors.grade && <div className={"text-red-600"}>{errors.grade}</div>}
                                        <div className="my-3">
                                            <label>Weight</label>
                                        </div>

                                        <input
                                            id="weight"
                                            name="weight"
                                            type="number"
                                            step={0.1}
                                            value={editGradeModel.weight}
                                            onChange={handleChange}
                                            className="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg w-full p-1 dark:bg-slate-500 dark:border-gray-600 dark:text-white"
                                            required
                                        />
                                        {errors.weight && <div className={"text-red-600"}>{errors.weight}</div>}
                                        <div className="my-3">
                                            <label>Comment</label>
                                        </div>
                                        <textarea id="comment" name="comment" rows={4}
                                                  className="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                                  value={editGradeModel.comment}
                                                  onChange={handleChange}></textarea>
                                        {errors.comment && <div className={"text-red-600"}>{errors.comment}</div>}
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
                                        Save Grade
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

export default EditGradeModal;