import React, { ReducerAction, useState } from 'react'

type DeletionParams = {
    setDeleteId : React.Dispatch<React.SetStateAction<string>>
    id : string
}

export default function DeletionWarning({setDeleteId, id} : DeletionParams) {
    const [showModal, setShowModal] = useState<boolean>(false);

    function handleEnter(e: React.KeyboardEvent<HTMLDivElement>): void {
        if (e.key === "Enter") {
            if (showModal) handleSubmit()
            setShowModal(false)
        }
        if (e.key == "Escape") setShowModal(false)

    }

    const handleSubmit = async () => {
        setDeleteId(id)
        setShowModal(false)
        window.location.reload();

    }

    return (
        <div tabIndex={1} onKeyDown={(e) => handleEnter(e)}>
            <div onClick={() => setShowModal(!showModal)}>
                <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6"
                     fill="none"
                     viewBox="0 0 24 24" stroke="currentColor" strokeWidth="2">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"/>
                </svg>
            </div>
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
                                        Delete
                                    </h3>
                                </div>
                                <div className="relative p-6 flex-auto">
                                    <div className="col-start-5 col-end-9">
                                        <p>Do you really want to delete this?</p>
                                    </div>
                                </div>
                                <div
                                    className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                                    <button
                                        className="text-red-500 background-transparent font-bold uppercase px-6 py-2 text-sm outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                                        type="button"
                                        onClick={() => setShowModal(false)}
                                    >
                                        Cancel
                                    </button>
                                    <button
                                        className="bg-emerald-500 text-white active:bg-emerald-600 font-bold uppercase text-sm px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                                        onClick={handleSubmit}>
                                        Yes
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
                </>
            ) : null}
        </div>
    )
}