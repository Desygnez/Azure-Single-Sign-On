import {useState} from 'react';
import {UserAndSubjectInterface} from "../lib/GradeSheet/GradSheet";
import {GradePostInterface} from "../lib/Grade/Grade";

import {CreateNewGrade} from "../lib/Grade/GradeService";
import {GradeErrors, ValidateGrade} from "../lib/Grade/GradeValidator";


type GradeProp = {
    gradeReq: UserAndSubjectInterface,
}

// replace the default semester ID with correct ID
const defaultModel: GradePostInterface = {
    weight: 1,
    grade: 0,
    date: new Date(),
    subject_id: "",
    comment: "No comment",
    semester_id: ""
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



function CreateGradeModal({gradeReq}: GradeProp) {
    const [showModal, setShowModal] = useState<boolean>(false);
    const [errors, setErrors] = useState<GradeErrors>(errorDefault);
    const [gradeRequest, setGradeRequest] = useState<UserAndSubjectInterface>(gradeReq)
    const [createGradeModel, setCreateGradeModel] = useState<GradePostInterface>({
        ...defaultModel,
        subject_id: typeof gradeRequest.subjectId == "string" ? gradeRequest.subjectId : "",
        semester_id: typeof gradeRequest.semesterId == "string" ? gradeRequest.semesterId : ""
    });

    const handleSubmit = async () => {

        try {

            const validated = ValidateGrade(createGradeModel)
            setErrors(validated)
            console.log(validated.isValid)
            if (!validated.isValid || typeof gradeRequest.subjectId != "string" || typeof gradeRequest.semesterId != "string" || typeof gradeRequest.userId != "string") return;

            await CreateNewGrade({
                ...createGradeModel,
                grade: parseFloat(String(createGradeModel.grade)),

            }, gradeRequest.userId)

            setShowModal(false)
            window.location.reload()
        } catch (error) {
            console.log(error)
        }
    }

    const handleEnter = (event: React.KeyboardEvent<HTMLDivElement>) => {
        if (event.key === "Enter") {
            if (showModal) handleSubmit()
            setShowModal(false)
        }
        if (event.key == "Escape") setShowModal(false)
    }

    const handleChange = (e: { target: { name: string; value: string; }; }) => {
        const name = e.target.name
        const value = e.target.value
        if (value === "INVALID" || value === "") return;
        setCreateGradeModel({...createGradeModel, [name]: value})
    }

    return (
        <div tabIndex={2} onKeyDown={(e) => handleEnter(e)}>
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
                                        Create Grade
                                    </h3>
                                </div>
                                <div className="relative p-6 flex-auto">
                                    <div className="col-start-5 col-end-9">
                                        <div className="my-3">
                                            <label>Grade</label>
                                        </div>
                                        <input
                                            id="grade"
                                            name="grade"
                                            type="number"
                                            step={0.1}
                                            min={1}
                                            max={6}
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
                                            placeholder={"1"}
                                            min={1}
                                            max={10}
                                            step={0.1}
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
                                            placeholder="Write a comment in here"
                                            onChange={handleChange}>    
                                        </textarea>

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

export default CreateGradeModal;