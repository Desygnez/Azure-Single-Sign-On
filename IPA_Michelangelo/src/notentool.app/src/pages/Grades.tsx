import {useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {UserAndSubjectInterface} from "../lib/GradeSheet/GradSheet";
import {Headers} from "../components/Headers";
import CreateGradeModal from "../components/CreateGradeModal";
import GradeTable from "../components/GradeTable";
import {SubjectInterface} from "../lib/Subject/Subject";
import {GetSubjectById} from "../lib/Subject/SubjectService";
import {SemesterInterface} from "../lib/Semester/Semester";
import {GetSemesterById} from "../lib/Semester/SemesterService";
import BackButton from "../components/BackButton";

export type GradesParams = {
    userId: string | undefined;
    semesterId: string | undefined;
    subjectId: string | undefined;
}

export default function Grades(): JSX.Element {

    document.title = "Grades | KPMG Notentool";

    let {userId, semesterId, subjectId} = useParams<GradesParams>()
    const [gradeRequest, setGradeRequest] = useState<UserAndSubjectInterface>({
        userId: userId,
        subjectId: subjectId,
        semesterId: semesterId
    });
    const [subject, setSubject] = useState<string>("");
    const [semester, setSemester] = useState<string>("");


    useEffect(() => {
        const loadSubject = async () => {
            if (typeof gradeRequest.subjectId != "string") return;
            const resp: SubjectInterface = await GetSubjectById(gradeRequest.subjectId);
            setSubject(resp.subject)
        }
        const loadSemester = async () => {
            if (typeof gradeRequest.semesterId != "string") return;
            const resp: SemesterInterface = await GetSemesterById(gradeRequest.semesterId);
            setSemester(resp.semester +". Semester")
        }
        loadSubject();
        loadSemester()
    }, []);


    return (
        <div>
            <Headers title={`${subject} in ${semester}`}/>
            <div className="grid grid-cols-10 w-full mt-5">
                <div className={"inline-flex col-start-2 mr-12"}>
                    <BackButton/>
                    <CreateGradeModal gradeReq={gradeRequest}/>
                </div>
            </div>
            <div className={"grid-cols-5 overflow-x-auto relative sm:rounded-lg"}>
                <GradeTable gradeReq={gradeRequest}/>
            </div>
        </div>
    );
}
