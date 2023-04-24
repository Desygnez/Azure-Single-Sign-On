import React, {Fragment, useEffect, useState} from 'react';
import {SubjectInterface} from "../lib/Subject/Subject";
import {GetAllSubjects} from "../lib/Subject/SubjectService";
import {GetSchools} from "../lib/School/SchoolService";
import {SubjectUserPostInterface, SubjectUsers} from "../lib/SubjectUser/SubjectUser";
import {CreateSubjectUser, GetSubjectUserByUserId} from "../lib/SubjectUser/SubjectUserService";
import {SubjectParams} from "../pages/SubjectSelection";
import {Combobox, Transition} from "@headlessui/react";
import {ChevronDownIcon} from "@heroicons/react/24/solid";
import {SchoolInterface} from "../lib/School/School";

const defaultModel: SubjectUserPostInterface = {
    school_id: "", semester_id: "", subject_id: "", userInfo_id: ""

}

function CreateSubjectModal({userId, semesterId}: SubjectParams) {
    const [showModal, setShowModal] = useState<boolean>(false);
    const [subjects, setSubjects] = useState<SubjectInterface[]>([]);
    const [schools, setSchools] = useState<SchoolInterface[]>([]);
    const [model, setModel] = useState<SubjectUserPostInterface>(defaultModel);

    const [selectedSubject, setSelectedSubject] = useState<SubjectInterface>();
    const [selectedSchool, setSelectedSchool] = useState<SchoolInterface>();

    const [subjectQuery, setSubjectQuery] = useState<string>("")
    const [schoolQuery, setSchoolQuery] = useState<string>("")

    useEffect(() => {
        if (typeof userId != "string") return;
        const load = async () => {
            const schoolResponse = await GetSchools();
            const allSubjects: SubjectInterface[] = await GetAllSubjects();
            const gotSubjects: SubjectUsers = await GetSubjectUserByUserId(userId);

            setSchools(schoolResponse)
            setSubjects(allSubjects)

            let remainingSubjects: SubjectInterface[] = []
            allSubjects.forEach(function (subject) {
                if (!gotSubjects.find(e => e.subject_id == subject.id && e.semester_id == semesterId)) remainingSubjects.push(subject)
            })
            setSubjects(remainingSubjects)
        }
        load()
    }, [showModal]);

    const handleEnter = (event: React.KeyboardEvent<HTMLDivElement | HTMLInputElement>) => {
        if(selectedSchool?.id == null || selectedSubject?.id == null) return
        if (event.key === "Enter") {
            if (showModal) handleSubmit()
            setShowModal(false)
        }
        if (event.key == "Escape") setShowModal(false)
    }
    const handleEsc = (event: React.KeyboardEvent<HTMLDivElement | HTMLInputElement>) => {
        if (event.key == "Escape") setShowModal(false)
    }

    const handleSubmit = async () => {
        if (typeof userId != "string" || typeof semesterId != "string" || selectedSubject?.id == null || selectedSchool?.id == null) return;

        try {
            await CreateSubjectUser({
                ...model,
                userInfo_id: userId,
                semester_id: semesterId,
                subject_id: selectedSubject.id,
                school_id: selectedSchool.id
            })
            setShowModal(false)
            window.location.reload()
        } catch (error) {
            console.log(error)
        }

    }
    const filteredSubject =
        subjectQuery === ''
            ? subjects
            : subjects.filter((subject) => {
                return subject.subject.toLowerCase().includes(subjectQuery.toLowerCase())
            })
    const filteredSchools =
        schoolQuery === ''
            ? schools
            : schools.filter((school) => {
                return school.school.toLowerCase().includes(schoolQuery.toLowerCase())
            })

    return (
        <div tabIndex={1} onKeyDown={(e) => handleEsc(e)}>
            <button type="button"
                    onClick={() => setShowModal(true)}
                    className="h-12 text-white bg-gradient-to-br from-purple-600 to-blue-500 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:focus:ring-blue-800 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">
                Add
            </button>
            {showModal ? (
                <>
                    <div
                        className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none"
                    >
                        <div className="relative w-auto my-6 mx-auto max-w-3xl" >

                            <div
                                className="border-0 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none">
                                <div
                                    className="flex items-start justify-between p-5 border-b border-solid border-slate-200 rounded-t">
                                    <h3 className="text-3xl font-semibold">
                                        Create Subject
                                    </h3>

                                </div>
                                <div className="relative p-6 flex-auto" >
                                    <div className="col-start-5 col-end-9">
                                        <div className="my-3">
                                            <label>Subject</label>
                                        </div>
                                        <Combobox value={selectedSubject} onChange={setSelectedSubject}>
                                            <div className="relative mt-1" >
                                                <div
                                                    className="relative w-full cursor-default overflow-hidden rounded-lg bg-white text-left shadow-md focus:outline-none focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-teal-300 sm:text-sm">
                                                    <Combobox.Input
                                                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                                        onChange={(event) => setSubjectQuery(event.target.value)}
                                                        onKeyDown={(e: React.KeyboardEvent<HTMLInputElement>) => handleEnter(e)}
                                                        displayValue={(subject: SubjectInterface) => subject.subject}
                                                    />
                                                    <Combobox.Button
                                                        className="absolute inset-y-0 right-0 flex items-center pr-2">
                                                        <ChevronDownIcon
                                                            className="h-5 w-3 dark:text-white text-slate-900"
                                                            aria-hidden="true"
                                                        />
                                                    </Combobox.Button>
                                                </div>
                                                <Transition
                                                    as={Fragment}
                                                    leave="transition ease-in duration-100"
                                                    leaveFrom="opacity-100"
                                                    leaveTo="opacity-0"
                                                    afterLeave={() => setSubjectQuery('')}
                                                >
                                                    <Combobox.Options
                                                        className="absolute bg-gray-50 border z-50 border-gray-300 text-gray-900 text-sm focus:ring-blue-500 focus:border-blue-500 block w-full dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                                                        {filteredSubject.length === 0 && subjectQuery !== '' ? (
                                                            <div
                                                                className="relative cursor-default dark:text-white select-none py-2 px-4 text-gray-700">
                                                                Nothing found.
                                                            </div>
                                                        ) : (
                                                            filteredSubject.map((subject) => (
                                                                <Combobox.Option
                                                                    key={subject.id}
                                                                    className={({active}) =>
                                                                        `relative cursor-default select-none py-1 pr-4 ${
                                                                            active ? 'dark:bg-[#52525e] bg-[#e0e0e6] dark:text-white text-slate-900' : 'dark:text-white text-slate-900'
                                                                        }`
                                                                    }
                                                                    value={subject}

                                                                >
                                                                    <div className={"ml-2"}>
                                                                        {subject.subject}
                                                                    </div>
                                                                </Combobox.Option>
                                                            ))
                                                        )}
                                                    </Combobox.Options>
                                                </Transition>
                                            </div>
                                        </Combobox>
                                    </div>
                                    <div className="col-start-5 col-end-9">
                                        <label>School</label>
                                        <Combobox value={selectedSchool} onChange={setSelectedSchool}>
                                            <div className="relative mt-1">
                                                <div
                                                    className="relative w-full cursor-default overflow-hidden rounded-lg bg-white text-left shadow-md focus:outline-none focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-teal-300 sm:text-sm">
                                                    <Combobox.Input
                                                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                                        onChange={(event) => setSchoolQuery(event.target.value)}
                                                        onKeyDown={(e: React.KeyboardEvent<HTMLInputElement>) => handleEnter(e)}
                                                        displayValue={(school: SchoolInterface) => school.school}
                                                    />
                                                    <Combobox.Button
                                                        className="absolute inset-y-0 right-0 flex items-center pr-2">
                                                        <ChevronDownIcon
                                                            className="h-5 w-3 dark:text-white text-slate-900"
                                                            aria-hidden="true"
                                                        />
                                                    </Combobox.Button>
                                                </div>
                                                <Transition
                                                    as={Fragment}
                                                    leave="transition ease-in duration-100"
                                                    leaveFrom="opacity-100"
                                                    leaveTo="opacity-0"
                                                    afterLeave={() => setSchoolQuery('')}
                                                >
                                                    <Combobox.Options
                                                        className="absolute bg-gray-50 z-50 border border-gray-300 text-gray-900 text-sm focus:ring-blue-500 focus:border-blue-500 block w-full dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                                                        {filteredSchools.length === 0 && schoolQuery !== '' ? (
                                                            <div
                                                                className="relative cursor-default select-none py-2 px-4 text-gray-700 dark:text-white">
                                                                Nothing found.
                                                            </div>
                                                        ) : (
                                                            filteredSchools.map((school) => (
                                                                <Combobox.Option
                                                                    key={school.id}
                                                                    className={({active}) =>
                                                                        `relative cursor-default select-none py-1 pr-4 ${
                                                                            active ? 'dark:bg-[#52525e] bg-[#e0e0e6] dark:text-white text-slate-900' : 'dark:text-white text-slate-900'
                                                                        }`
                                                                    }
                                                                    value={school}

                                                                >
                                                                    <div className={"ml-2"}>
                                                                        {school.school}
                                                                    </div>
                                                                </Combobox.Option>
                                                            ))
                                                        )}
                                                    </Combobox.Options>
                                                </Transition>
                                            </div>
                                        </Combobox>
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
                                        Save Subject
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

export default CreateSubjectModal;