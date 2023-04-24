import {BrowserRouter as Router, Route, Routes, useNavigate} from "react-router-dom";
import Home from "./pages/Home";
import Nav from "./components/Nav";
import SubjectSelection from './pages/SubjectSelection';
import Grades from './pages/Grades';
import SemesterSelection from "./pages/SemesterSelection";
import Footer from "./components/Footer";
import Settings from "./pages/Settings";
import React, {useState} from "react";
import {AuthenticatedTemplate, UnauthenticatedTemplate, useMsal} from "@azure/msal-react";
import {SilentRequest} from "@azure/msal-browser";
import NotFound from "./components/NotFound";


export default function App() {
    return (
        <Router>
            <div className="dark:bg-slate-800 h-screen">
                <Nav/>
                <Routes>

                    <Route path="/home" element={<AuthenticatedTemplate><Home/></AuthenticatedTemplate>}></Route>
                    <Route path="/dashboard/:userId" element={<AuthenticatedTemplate><SemesterSelection/></AuthenticatedTemplate>}></Route>
                    <Route path="/settings" element={<AuthenticatedTemplate><Settings/></AuthenticatedTemplate>}></Route>
                    <Route path="/subjects/:userId/:semesterId" element={<AuthenticatedTemplate><SubjectSelection/></AuthenticatedTemplate>}></Route>
                    <Route path="/grades/:userId/:semesterId/:subjectId" element={<AuthenticatedTemplate><Grades/></AuthenticatedTemplate>}></Route>
                    <Route path="/" element={<UnauthenticatedTemplate><ProfileContent/></UnauthenticatedTemplate>}></Route>
                    <Route path="*" element={<NotFound/>}></Route>
                </Routes>
                <div className={"absolute w-full inset-x-0 bottom-0"}>
                    <Footer/>
                </div>
            </div>
        </Router>
    );
}

export const ProfileContent = (): any => {
    const {instance, accounts} = useMsal();
    const [graphData, setGraphData] = useState<any>(null);
    const navigate = useNavigate()
    const name = accounts[0] && accounts[0].name;


    function RequestProfileData() {
        const request: SilentRequest = {
            account: accounts[0],
            scopes: ["api://acf28637-a463-4afd-9592-64914f52b13c/Access.as.User"]
        };

        // Silently acquires an access token which is then attached to a request for Microsoft Graph data
        instance.acquireTokenSilent(request).then((response) => {
            console.log(response.accessToken)
            sessionStorage.setItem('aadToken', JSON.stringify(response))
            navigate(`/home`)
        }).catch((e) => {
            instance.acquireTokenPopup(request).then((response) => {
                console.log(response.accessToken)
                sessionStorage.setItem('aadToken', JSON.stringify(response))
                navigate(`/home`)
            });
        });
    }


    return (
        <div className="flex flex-col items-center py-12 px-4 sm:px-6 lg:px-8">
            <h1 className="text-3xl font-bold text-gray-900 dark:text-slate-100 text-center my-4">Click to
                Authenticate</h1>
            {graphData ?
                <>
                </>
                :
                <button type="button"
                        className="w-3/12 content-center rounded-full text-white bg-[#1E49E2] focus:ring-4 focus:outline-none focus:ring-blue-300 dark:focus:ring-blue-800 font-medium text-xl px-5 py-4 text-center mr-2 mb-2"
                        onClick={() => {
                            RequestProfileData();
                        }}>Authenticate with Azure</button>
            }
        </div>
    );
}