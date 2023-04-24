import React, {Fragment, useEffect, useState} from 'react';
import {UserInfoInterface} from "../lib/UserInfo/UserInfo";
import {CreateTrainerApprentice, GetUsers} from "../lib/UserInfo/UserInformationService";
import {Combobox, Transition} from "@headlessui/react";
import {ChevronDownIcon} from "@heroicons/react/24/solid";

const defaultUser: UserInfoInterface = {
    email: "",
    firstname: "",
    id: "",
    lastname: "",
    username: ""
}
function AssignTrainerForm() {
    const [user, setUser] = useState<UserInfoInterface[]>([]);
    const [trainerQuery, setTrainerQuery] = useState<string>("");
    const [apprenticeQuery, setApprenticeQuery] = useState<string>("");
    const [selectedTrainer, setSelectedTrainer] = useState<UserInfoInterface>(defaultUser);
    const [selectedApprentice, setSelectedApprentice] = useState<UserInfoInterface>(defaultUser);
    useEffect(() => {
        const load = async () => {
            const users = await GetUsers();
            setUser(users)
        }
        load()
    }, []);
    const filteredTrainers =
        trainerQuery === ''
            ? user
            : user.filter((user) => {
                return user.username.toLowerCase().includes(trainerQuery.toLowerCase())
            })
    const filteredApprentices =
        apprenticeQuery === ''
            ? user
            : user.filter((user) => {
                return user.username.toLowerCase().includes(apprenticeQuery.toLowerCase())
            })


    const handleSubmit = async () => {
        if (selectedTrainer.id == "" || selectedApprentice.id == "") return;
        try {
            await CreateTrainerApprentice({id: "00000000-0000-0000-0000-000000000000", trainerId: selectedTrainer.id, apprenticeId: selectedApprentice.id})
            window.location.reload()
        } catch (error) {
            console.log(error)
        }
    }

    return (
        <div>
            <div className="relative p-6 flex-auto">
                <div className="col-start-5 col-end-9">
                    <div className="my-3">
                        <label>Trainer</label>
                    </div>
                    <Combobox value={selectedTrainer} onChange={setSelectedTrainer}>
                        <div className="relative mt-1">
                            <div
                                className="relative w-full cursor-default overflow-hidden rounded-lg bg-white text-left shadow-md focus:outline-none focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-teal-300 sm:text-sm">
                                <Combobox.Input
                                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                    onChange={(event) => setTrainerQuery(event.target.value)}
                                    displayValue={(trainer: UserInfoInterface) => trainer.username}
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
                                afterLeave={() => setTrainerQuery('')}
                            >
                                <Combobox.Options
                                    className="absolute z-50 bg-gray-50 border border-gray-300 text-gray-900 text-sm focus:ring-blue-500 focus:border-blue-500 block w-full dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                                    {filteredTrainers.length === 0 && trainerQuery !== '' ? (
                                        <div
                                            className="relative cursor-default select-none py-2 px-4 text-gray-700">
                                            Nothing found.
                                        </div>
                                    ) : (
                                        filteredTrainers.map((trainer) => (
                                            <Combobox.Option
                                                key={trainer.id}
                                                className={({active}) =>
                                                    `relative cursor-default select-none py-1 pr-4 ${
                                                        active ? 'dark:bg-[#52525e] bg-[#e0e0e6] dark:text-white text-slate-900' : 'dark:text-white text-slate-900'
                                                    }`
                                                }
                                                value={trainer}

                                            >
                                                <div className={"ml-2"}>
                                                    {trainer.username}
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
                    <div className="my-3">
                        <label>Apprentice</label>
                    </div>
                    <Combobox value={selectedApprentice} onChange={setSelectedApprentice}>
                        <div className="relative mt-1">
                            <div
                                className="relative w-full cursor-default overflow-hidden rounded-lg bg-white text-left shadow-md focus:outline-none focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-teal-300 sm:text-sm">
                                <Combobox.Input
                                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                    onChange={(event) => setApprenticeQuery(event.target.value)}
                                    displayValue={(apprentice: UserInfoInterface) => apprentice.username}
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
                                afterLeave={() => setApprenticeQuery('')}
                            >
                                <Combobox.Options
                                    className="absolute z-50 bg-gray-50 border border-gray-300 text-gray-900 text-sm focus:ring-blue-500 focus:border-blue-500 block w-full dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                                    {filteredApprentices.length === 0 && trainerQuery !== '' ? (
                                        <div
                                            className="relative cursor-default select-none py-2 px-4 text-gray-700">
                                            Nothing found.
                                        </div>
                                    ) : (
                                        filteredApprentices.map((apprentice) => (
                                            <Combobox.Option
                                                key={apprentice.id}
                                                className={({active}) =>
                                                    `relative cursor-default select-none py-1 pr-4 ${
                                                        active ? 'dark:bg-[#52525e] bg-[#e0e0e6] dark:text-white text-slate-900' : 'dark:text-white text-slate-900'
                                                    }`
                                                }
                                                value={apprentice}

                                            >
                                                <div className={"ml-2"}>
                                                    {apprentice.username}
                                                </div>
                                            </Combobox.Option>
                                        ))
                                    )}
                                </Combobox.Options>
                            </Transition>
                        </div>
                    </Combobox>
                </div>
                <button onClick={handleSubmit}>Submit</button>
            </div>
        </div>
    );
}

export default AssignTrainerForm;