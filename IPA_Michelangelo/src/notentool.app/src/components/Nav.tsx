import React, {useEffect, useState} from "react";
import {useNavigate} from 'react-router-dom';
import {GetCurrentUser} from "../lib/UserInfo/UserInformationService";
import {User} from "../lib/UserInfo/UserInfo";
import {PageLayout} from "./PageLayout";

/**
 * Renders the Navbar which, when selected, will redirect the page to the logout prompt
 */

const defaultUser: User = {
    apprentices: [],
    email: "",
    firstname: "",
    id: "",
    isVocationalTrainer: false,
    lastname: "",
    username: ""
}
export default function Nav(): JSX.Element {

    const navigate = useNavigate()
    const [user, setUser] = useState<User>(defaultUser);

    useEffect(() => {
        const loadUser = async () => {
            const user = await GetCurrentUser();
            setUser(user)
        }
        loadUser()
    }, []);

    const handleDashboard = async () => {
        navigate(`/dashboard/${user.id}`)
    }
    return (
        <div>
            <nav className="px-2 sm:px-4 py-2.5 bg-[#1E49E2]">
                <div className="container flex flex-wrap justify-between items-center mx-auto">
                    <a href="/home" className="flex items-center">
                        <img
                            src="/kpmgWhite.png"
                            className="mr-3 w-20"
                            alt="KPMG Logo"
                        />
                        <span className="self-center text-xl font-semibold whitespace-nowrap text-white">
              Notentool
            </span>
                    </a>
                    <div className="hidden w-full md:block text-white md:w-auto" id="navbar-default">
                        <ul className="flex mt-0 text-white flex-row space-x-10 text-sm font-medium border-0 bg-[#1E49E2] text-white">
                            <li>
                                <a
                                    href="/home"
                                    className="block py-2 pr-4 pl-3 cursor-pointer text-white rounded md:p-0"
                                    aria-current="page"
                                >
                                    Home
                                </a>
                            </li>
                            {
                                !user.isVocationalTrainer && user.apprentices.length == 0 ?
                                    <li>
                                        <a
                                            onClick={handleDashboard}
                                            className="block py-2 pr-4 pl-3 cursor-pointer rounded md:border-0 md:p-0 "
                                        >
                                            Dashboard
                                        </a>
                                    </li> :
                                    <li>
                                        <a  href={"/settings"}
                                            className="block py-2 pr-4 pl-3 cursor-pointer rounded md:border-0 md:p-0 ">
                                            Settings
                                        </a>
                                    </li>
                            }
                            <PageLayout></PageLayout>
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
    );
}
