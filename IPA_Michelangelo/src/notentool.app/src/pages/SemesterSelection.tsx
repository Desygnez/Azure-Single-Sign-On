import React from 'react';
import {Headers} from "../components/Headers";
import SemesterTable from "../components/SemesterTable";
import CreateSemesterModal from "../components/CreateSemesterModal";
import BackButton from "../components/BackButton";
import {useParams} from "react-router-dom";

export type SemesterParams = {
    userId: string | undefined;
}

function SemesterSelection() {
    document.title = "Semester | KPMG Notentool"
    let {userId} = useParams<SemesterParams>()

    return (
        <div>
            <Headers title={"Semester"}/>
            <div className="grid grid-cols-10 w-full mt-5">
                <div className={"inline-flex col-start-2 mr-12"}>
                    <BackButton/>
                    <CreateSemesterModal userId={userId}/>
                </div>
            </div>
            <div className={"grid-cols-5 overflow-x-auto relative sm:rounded-lg"}>
                <SemesterTable userId={userId}/>
            </div>
        </div>
    );
}

export default SemesterSelection;