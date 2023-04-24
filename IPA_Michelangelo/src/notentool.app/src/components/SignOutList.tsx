import React from "react";
import {useMsal} from "@azure/msal-react";

/**
 * Renders a List which, when selected, will redirect the page to the logout prompt
 */
export const SignOutList = () => {
    const {instance} = useMsal();
    const handleLogout = (logoutType: string) => {
        if (logoutType === "redirect") {
            instance.logoutRedirect({
                postLogoutRedirectUri: "/",
            });
        }
    }
    return (
        <li>
            <a onClick={() => handleLogout("redirect")}
               className="block py-2 pr-4 pl-3 cursor-pointer rounded md:border-0 md:p-0 ">
                Sign out
            </a>
        </li>
    );
}