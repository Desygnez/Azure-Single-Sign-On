import React from 'react';
import {useNavigate} from "react-router-dom";

function BackButton() {
    const navigate = useNavigate()
    return (
        <button type="button"
                className="w-1/1 mx-auto text-white bg-gradient-to-br from-purple-600 to-blue-500 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:focus:ring-blue-800 font-medium rounded-lg text-sm px-5 py-2.5 text-center mb-2"
                onClick={() => navigate(-1)}
        >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5"
                 stroke="currentColor" className="w-6 h-6">
                <path strokeLinecap="round" strokeLinejoin="round"
                      d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75"/>
            </svg>
            <span className="sr-only">Back button</span>
        </button>);
}

export default BackButton;