import React from "react";

type HeaderProps = {
    title: string
}

function Headers({title}: HeaderProps) {
    return (
        <div>
            <header className="bg-white shadow dark:bg-slate-800">
                <div className="max-w-[1600px] mx-auto py-6 px-4 sm:px-6 lg:px-8">
                    <h1 className="text-3xl font-bold text-gray-900 dark:text-slate-100 text-left">
                        {title}
                    </h1>
                </div>
            </header>
        </div>
    );
}
function SubHeader({title}: HeaderProps) {
    return (
        <div>
            <header className="bg-white dark:bg-slate-800">
                <div className="max-w-[1600px] mx-auto py-6 px-4 sm:px-6 lg:px-8">
                    <h1 className="text-2xl font-bold text-gray-900 dark:text-slate-100 text-left">
                        {title}
                    </h1>
                </div>
            </header>
        </div>
    );
}

export {Headers, SubHeader}