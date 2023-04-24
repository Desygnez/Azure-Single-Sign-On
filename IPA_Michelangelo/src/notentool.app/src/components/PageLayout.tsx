import React from "react";
import { useIsAuthenticated } from "@azure/msal-react";
import {SignOutList} from "./SignOutList";


/**
 * Renders the list component with a sign-out if a user is not authenticated
 */
export const PageLayout = (props : any) => {
    const isAuthenticated = useIsAuthenticated();

    return (
        <>
            { isAuthenticated ? <SignOutList /> : <p/> }
        </>
    );
};